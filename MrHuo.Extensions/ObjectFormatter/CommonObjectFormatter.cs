using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace MrHuo.Extensions
{
    internal abstract class CommonObjectFormatter : ObjectFormatter
    {
        public override string FormatObject(object obj)
        {
            return new CommonObjectFormatter.Visitor(this).FormatObject(obj);
        }

        protected abstract CommonTypeNameFormatter TypeNameFormatter { get; }

        protected abstract CommonPrimitiveFormatter PrimitiveFormatter { get; }

        protected abstract string FormatRefKind(System.Reflection.ParameterInfo parameter);

        private sealed class Builder
        {
            private readonly StringBuilder _sb = new StringBuilder();
            
            public void Append(char c)
            {
                this._sb.Append(c);
            }

            public void Append(string str, int start = 0, int count = 2147483647)
            {
                if (str == null)
                    return;
                count = Math.Min(count, str.Length - start);
                this._sb.Append(str, start, count);
            }

            public void AppendGroupOpening()
            {
                this.Append('{');
            }

            public void AppendGroupClosing()
            {
                this.Append(" }", 0, int.MaxValue);
            }

            public void AppendCollectionItemSeparator(bool isFirst)
            {
                if (isFirst)
                {
                    this.Append(' ');
                }
                else
                {
                    this.Append(", ", 0, int.MaxValue);
                }
            }

            internal void AppendInfiniteRecursionMarker()
            {
                this.AppendGroupOpening();
                this.AppendCollectionItemSeparator(true);
                this.Append("...", 0, int.MaxValue);
                this.AppendGroupClosing();
            }

            public override string ToString()
            {
                return this._sb.ToString();
            }
        }

        internal class ReferenceEqualityComparer : IEqualityComparer<object>
        {
            public static readonly ReferenceEqualityComparer Instance = new ReferenceEqualityComparer();

            private ReferenceEqualityComparer()
            {
            }

            bool IEqualityComparer<object>.Equals(object a, object b)
            {
                return a == b;
            }

            int IEqualityComparer<object>.GetHashCode(object a)
            {
                return ReferenceEqualityComparer.GetHashCode(a);
            }

            public static int GetHashCode(object a)
            {
                return RuntimeHelpers.GetHashCode(a);
            }
        }

        private sealed class Visitor
        {
            private readonly CommonObjectFormatter _formatter;
            private CommonPrimitiveFormatterOptions _primitiveOptions = new CommonPrimitiveFormatterOptions();
            private HashSet<object> _lazyVisitedObjects;

            private HashSet<object> VisitedObjects
            {
                get
                {
                    if (this._lazyVisitedObjects == null)
                        this._lazyVisitedObjects = new HashSet<object>(ReferenceEqualityComparer.Instance);
                    return this._lazyVisitedObjects;
                }
            }

            public Visitor(CommonObjectFormatter formatter)
            {
                this._formatter = formatter;
            }

            public string FormatObject(object obj)
            {
                try
                {
                    return this.FormatObjectRecursive(new CommonObjectFormatter.Builder(), obj, true).ToString();
                }
                catch (InsufficientExecutionStackException ex)
                {
                    return ex.ToString();
                }
            }

            private CommonObjectFormatter.Builder FormatObjectRecursive(
              CommonObjectFormatter.Builder result,
              object obj,
              bool isRoot)
            {
                string str = this._formatter.PrimitiveFormatter.FormatPrimitive(obj, this._primitiveOptions);
                if (str != null)
                {
                    result.Append(str, 0, int.MaxValue);
                    return result;
                }
                Type type = obj.GetType();
                System.Reflection.TypeInfo typeInfo = IntrospectionExtensions.GetTypeInfo(type);
                if (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition() == (object)typeof(KeyValuePair<,>))
                {
                    if (isRoot)
                    {
                        result.Append(this._formatter.TypeNameFormatter.FormatTypeName(type), 0, int.MaxValue);
                        result.Append(' ');
                    }
                    this.FormatKeyValuePair(result, obj);
                    return result;
                }
                if (typeInfo.IsArray)
                {
                    if (this.VisitedObjects.Add(obj))
                    {
                        this.FormatArray(result, (Array)obj);
                        this.VisitedObjects.Remove(obj);
                    }
                    else
                        result.AppendInfiniteRecursionMarker();
                    return result;
                }
                bool flag = false;
                ICollection collection = null;
                if (obj is ICollection)
                {
                    collection = (ICollection)obj;
                    this.FormatCollectionHeader(result, collection);
                }
                else if (ObjectFormatterHelpers.HasOverriddenToString(typeInfo))
                {
                    this.ObjectToString(result, obj);
                    flag = true;
                }
                else
                    result.Append(this._formatter.TypeNameFormatter.FormatTypeName(type), 0, int.MaxValue);

                if (!flag)
                    this.FormatMembers(result, obj);
                return result;
            }

            private void FormatMembers(
              CommonObjectFormatter.Builder result,
              object obj)
            {
                RuntimeHelpers.EnsureSufficientExecutionStack();
                result.Append(' ');
                if (!this.VisitedObjects.Add(obj))
                {
                    result.AppendInfiniteRecursionMarker();
                }
                else
                {
                    bool flag = false;
                    if (obj is IDictionary dict)
                    {
                        this.FormatDictionaryMembers(result, dict);
                        flag = true;
                    }
                    else if (obj is IEnumerable sequence)
                    {
                        this.FormatSequenceMembers(result, sequence);
                        flag = true;
                    }
                    if (!flag)
                        this.FormatObjectMembers(result, obj);
                    this.VisitedObjects.Remove(obj);
                }
            }

            private void FormatObjectMembers(
              CommonObjectFormatter.Builder result,
              object obj)
            {
                List<CommonObjectFormatter.Visitor.FormattedMember> result1 = new List<CommonObjectFormatter.Visitor.FormattedMember>();
                this.FormatObjectMembersRecursive(result1, obj);
                bool flag = CommonObjectFormatter.Visitor.UseCollectionFormat(result1, IntrospectionExtensions.GetTypeInfo(obj.GetType()));
                result.AppendGroupOpening();
                for (int index = 0; index < result1.Count; ++index)
                {
                    result.AppendCollectionItemSeparator(index == 0);
                    if (flag)
                        result1[index].AppendAsCollectionEntry(result);
                    else
                        result1[index].Append(result, "=");
                }
                result.AppendGroupClosing();
            }

            private static bool UseCollectionFormat(
              IEnumerable<CommonObjectFormatter.Visitor.FormattedMember> members,
              System.Reflection.TypeInfo originalType)
            {
                if (IntrospectionExtensions.GetTypeInfo(typeof(IEnumerable)).IsAssignableFrom(originalType))
                    return Enumerable.All<CommonObjectFormatter.Visitor.FormattedMember>(members, member => member.Index >= 0);
                return false;
            }

            private void FormatObjectMembersRecursive(
              List<CommonObjectFormatter.Visitor.FormattedMember> result,
              object obj)
            {
                List<MemberInfo> memberInfoList = new List<MemberInfo>();
                Type baseType;
                for (System.Reflection.TypeInfo typeInfo = IntrospectionExtensions.GetTypeInfo(obj.GetType());
                    typeInfo != null;
                    typeInfo = (object)baseType != null ? IntrospectionExtensions.GetTypeInfo(baseType) : null)
                {
                    memberInfoList.AddRange(Enumerable.Where<FieldInfo>(typeInfo.DeclaredFields, f => !f.IsStatic));
                    memberInfoList.AddRange(Enumerable.Where<PropertyInfo>(typeInfo.DeclaredProperties, f =>
                    {
                        if ((object)f.GetMethod != null)
                            return !f.GetMethod.IsStatic;
                        return false;
                    }));
                    baseType = typeInfo.BaseType;
                }
                memberInfoList.Sort((x, y) =>
                {
                    int num = StringComparer.OrdinalIgnoreCase.Compare(x.Name, y.Name);
                    if (num == 0)
                        num = StringComparer.Ordinal.Compare(x.Name, y.Name);
                    return num;
                });
                foreach (MemberInfo member in memberInfoList)
                {
                    bool flag1 = false;
                    bool flag2 = false;
                    if (member is FieldInfo fieldInfo)
                    {
                        if (!(flag2) && !fieldInfo.IsPublic && (!fieldInfo.IsFamily && !fieldInfo.IsFamilyOrAssembly))
                            continue;
                    }
                    else
                    {
                        PropertyInfo propertyInfo = (PropertyInfo)member;
                        MethodInfo getMethod = propertyInfo.GetMethod;
                        if ((object)getMethod != null)
                        {
                            MethodInfo setMethod = propertyInfo.SetMethod;
                            if (!(flag2) && !getMethod.IsPublic && (!getMethod.IsFamily && !getMethod.IsFamilyOrAssembly) && ((object)setMethod == null || !setMethod.IsPublic && !setMethod.IsFamily && !setMethod.IsFamilyOrAssembly) || getMethod.GetParameters().Length != 0)
                                continue;
                        }
                        else
                            continue;
                    }
                    Exception exception;
                    object memberValue = ObjectFormatterHelpers.GetMemberValue(member, obj, out exception);
                    if (exception != null)
                    {
                        CommonObjectFormatter.Builder result1 = new Builder();
                        this.FormatException(result1, exception);
                        if (!this.AddMember(result, new CommonObjectFormatter.Visitor.FormattedMember(-1, member.Name, result1.ToString())))
                            break;
                    }
                    else if (flag1)
                    {
                        if (memberValue != null && !this.VisitedObjects.Contains(memberValue))
                        {
                            if (memberValue is Array array)
                            {
                                int index = 0;
                                foreach (object obj1 in array)
                                {
                                    CommonObjectFormatter.Builder result1 = new Builder();
                                    this.FormatObjectRecursive(result1, obj1, false);
                                    if (!this.AddMember(result, new CommonObjectFormatter.Visitor.FormattedMember(index, "", result1.ToString())))
                                        return;
                                    ++index;
                                }
                            }
                            else if (this._formatter.PrimitiveFormatter.FormatPrimitive(memberValue, this._primitiveOptions) == null && this.VisitedObjects.Add(memberValue))
                            {
                                this.FormatObjectMembersRecursive(result, memberValue);
                                this.VisitedObjects.Remove(memberValue);
                            }
                        }
                    }
                    else
                    {
                        CommonObjectFormatter.Builder result1 = new Builder();
                        this.FormatObjectRecursive(result1, memberValue, false);
                        if (!this.AddMember(result, new CommonObjectFormatter.Visitor.FormattedMember(-1, "", result1.ToString())))
                            break;
                    }
                }
            }

            private bool AddMember(
              List<CommonObjectFormatter.Visitor.FormattedMember> members,
              CommonObjectFormatter.Visitor.FormattedMember member)
            {
                members.Add(member);
                return true;
            }

            private void FormatException(CommonObjectFormatter.Builder result, Exception exception)
            {
                result.Append("!<", 0, int.MaxValue);
                result.Append(this._formatter.TypeNameFormatter.FormatTypeName(((object)exception).GetType()), 0, int.MaxValue);
                result.Append('>');
            }

            private void FormatKeyValuePair(CommonObjectFormatter.Builder result, object obj)
            {
                System.Reflection.TypeInfo typeInfo = IntrospectionExtensions.GetTypeInfo(obj.GetType());
                object obj1 = typeInfo.GetDeclaredProperty("Key").GetValue(obj, Array.Empty<object>());
                object obj2 = typeInfo.GetDeclaredProperty("Value").GetValue(obj, Array.Empty<object>());
                result.AppendGroupOpening();
                result.AppendCollectionItemSeparator(true);
                this.FormatObjectRecursive(result, obj1, false);
                result.AppendCollectionItemSeparator(false);
                this.FormatObjectRecursive(result, obj2, false);
                result.AppendGroupClosing();
            }

            private void FormatCollectionHeader(
              CommonObjectFormatter.Builder result,
              ICollection collection)
            {
                if (collection is Array arrayOpt)
                {
                    result.Append(this._formatter.TypeNameFormatter.FormatArrayTypeName(arrayOpt.GetType(), arrayOpt), 0, int.MaxValue);
                }
                else
                {
                    result.Append(this._formatter.TypeNameFormatter.FormatTypeName(collection.GetType()), 0, int.MaxValue);
                    try
                    {
                        result.Append('(');
                        result.Append(collection.Count.ToString(), 0, int.MaxValue);
                        result.Append(')');
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }

            private void FormatArray(CommonObjectFormatter.Builder result, Array array)
            {
                this.FormatCollectionHeader(result, array);
                if (array.Rank > 1)
                {
                    this.FormatMultidimensionalArrayElements(result, array);
                }
                else
                {
                    result.Append(' ');
                    this.FormatSequenceMembers(result, array);
                }
            }

            private void FormatDictionaryMembers(
              CommonObjectFormatter.Builder result,
              IDictionary dict)
            {
                result.AppendGroupOpening();
                int num = 0;
                try
                {
                    IDictionaryEnumerator enumerator = dict.GetEnumerator();
                    using (enumerator as IDisposable)
                    {
                        while (enumerator.MoveNext())
                        {
                            DictionaryEntry entry = enumerator.Entry;
                            result.AppendCollectionItemSeparator(num == 0);
                            result.AppendGroupOpening();
                            result.AppendCollectionItemSeparator(true);
                            this.FormatObjectRecursive(result, entry.Key, false);
                            result.AppendCollectionItemSeparator(false);
                            this.FormatObjectRecursive(result, entry.Value, false);
                            result.AppendGroupClosing();
                            ++num;
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.AppendCollectionItemSeparator(num == 0);
                    this.FormatException(result, ex);
                    result.Append(' ');
                }
                result.AppendGroupClosing();
            }

            private void FormatSequenceMembers(
              CommonObjectFormatter.Builder result,
              IEnumerable sequence)
            {
                result.AppendGroupOpening();
                int num = 0;
                try
                {
                    foreach (object obj in sequence)
                    {
                        result.AppendCollectionItemSeparator(num == 0);
                        this.FormatObjectRecursive(result, obj, false);
                        ++num;
                    }
                }
                catch (Exception ex)
                {
                    result.AppendCollectionItemSeparator(num == 0);
                    this.FormatException(result, ex);
                    result.Append(" ...", 0, int.MaxValue);
                }
                result.AppendGroupClosing();
            }

            private void FormatMultidimensionalArrayElements(
              CommonObjectFormatter.Builder result,
              Array array)
            {
                if (array.Length == 0)
                {
                    result.AppendCollectionItemSeparator(true);
                    result.AppendGroupOpening();
                    result.AppendGroupClosing();
                }
                else
                {
                    int[] numArray = new int[array.Rank];
                    for (int dimension = array.Rank - 1; dimension >= 0; --dimension)
                        numArray[dimension] = array.GetLowerBound(dimension);
                    int num1 = 0;
                    int num2 = 0;
                    while (true)
                    {
                        int dimension1 = numArray.Length - 1;
                        while (numArray[dimension1] > array.GetUpperBound(dimension1))
                        {
                            numArray[dimension1] = array.GetLowerBound(dimension1);
                            result.AppendGroupClosing();
                            --num1;
                            --dimension1;
                            if (dimension1 < 0)
                                return;
                            ++numArray[dimension1];
                        }
                        result.AppendCollectionItemSeparator(num2 == 0);
                        for (int dimension2 = numArray.Length - 1; dimension2 >= 0 && numArray[dimension2] == array.GetLowerBound(dimension2); --dimension2)
                        {
                            result.AppendGroupOpening();
                            ++num1;
                            result.AppendCollectionItemSeparator(true);
                        }
                        this.FormatObjectRecursive(result, array.GetValue(numArray), false);
                        ++numArray[numArray.Length - 1];
                        ++num2;
                    }
                }
            }

            private void ObjectToString(CommonObjectFormatter.Builder result, object obj)
            {
                try
                {
                    string str = obj.ToString();
                    result.Append('[');
                    result.Append(str, 0, int.MaxValue);
                    result.Append(']');
                }
                catch (Exception ex)
                {
                    this.FormatException(result, ex);
                }
            }

            private struct FormattedMember
            {
                public readonly int Index;
                public readonly string Name;
                public readonly string Value;

                public FormattedMember(int index, string name, string value)
                {
                    this.Name = name;
                    this.Index = index;
                    this.Value = value;
                }

                public string GetDisplayName()
                {
                    return this.Name ?? "[" + this.Index.ToString() + "]";
                }

                public bool HasKeyName()
                {
                    if (this.Index >= 0 && this.Name != null && (this.Name.Length >= 2 && this.Name[0] == '['))
                        return this.Name[this.Name.Length - 1] == ']';
                    return false;
                }

                public bool AppendAsCollectionEntry(CommonObjectFormatter.Builder result)
                {
                    if (this.HasKeyName())
                    {
                        result.AppendGroupOpening();
                        result.AppendCollectionItemSeparator(true);
                        result.Append(this.Name, 1, this.Name.Length - 2);
                        result.AppendCollectionItemSeparator(false);
                        result.Append(this.Value, 0, int.MaxValue);
                        result.AppendGroupClosing();
                    }
                    else
                        result.Append(this.Value, 0, int.MaxValue);
                    return true;
                }

                public bool Append(CommonObjectFormatter.Builder result, string separator)
                {
                    result.Append(this.GetDisplayName(), 0, int.MaxValue);
                    result.Append(separator, 0, int.MaxValue);
                    result.Append(this.Value, 0, int.MaxValue);
                    return true;
                }
            }
        }
    }
}
