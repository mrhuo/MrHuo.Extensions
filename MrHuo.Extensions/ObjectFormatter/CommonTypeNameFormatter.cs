using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace MrHuo.Extensions
{
    internal abstract class CommonTypeNameFormatter
    {
        protected abstract string GetPrimitiveTypeName(SpecialType type);

        protected abstract string GenericParameterOpening { get; }

        protected abstract string GenericParameterClosing { get; }

        protected abstract string ArrayOpening { get; }

        protected abstract string ArrayClosing { get; }

        protected abstract CommonPrimitiveFormatter PrimitiveFormatter { get; }

        public virtual string FormatTypeName(Type type)
        {
            if ((object)type == null)
                throw new ArgumentNullException(nameof(type));
            string primitiveTypeName = this.GetPrimitiveTypeName(ObjectFormatterHelpers.GetPrimitiveSpecialType(type));
            if (primitiveTypeName != null)
                return primitiveTypeName;
            if (type.IsGenericParameter)
                return type.Name;
            if (type.IsArray)
                return this.FormatArrayTypeName(type, null);
            System.Reflection.TypeInfo typeInfo = IntrospectionExtensions.GetTypeInfo(type);
            if (typeInfo.IsGenericType)
                return this.FormatGenericTypeName(typeInfo);
            return CommonTypeNameFormatter.FormatNonGenericTypeName(typeInfo);
        }

        private static string FormatNonGenericTypeName(System.Reflection.TypeInfo typeInfo)
        {
            if ((object)typeInfo.DeclaringType == null)
                return typeInfo.Name;
            List<string> instance = new List<string>();
            do
            {
                instance.Add(typeInfo.Name);
                Type declaringType = typeInfo.DeclaringType;
                typeInfo = (object)declaringType != null ? IntrospectionExtensions.GetTypeInfo(declaringType) : null;
            }
            while (typeInfo != null);
            instance.Reverse();
            return string.Join(".", instance);
        }

        public virtual string FormatTypeArguments(Type[] typeArguments)
        {
            if (typeArguments == null)
                throw new ArgumentNullException(nameof(typeArguments));
            if (typeArguments.Length == 0)
                throw new ArgumentException(null, nameof(typeArguments));
            StringBuilder builder = new StringBuilder();
            builder.Append(this.GenericParameterOpening);
            bool flag = true;
            foreach (Type typeArgument in typeArguments)
            {
                if (flag)
                    flag = false;
                else
                    builder.Append(", ");
                builder.Append(this.FormatTypeName(typeArgument));
            }
            builder.Append(this.GenericParameterClosing);
            return builder.ToString();
        }

        public virtual string FormatArrayTypeName(
          Type arrayType,
          Array arrayOpt)
        {
            if ((object)arrayType == null)
                throw new ArgumentNullException(nameof(arrayType));
            StringBuilder sb = new StringBuilder();
            Type elementType = arrayType.GetElementType();
            while (elementType.IsArray)
                elementType = elementType.GetElementType();
            sb.Append(this.FormatTypeName(elementType));
            Type arrayType1 = arrayType;
            do
            {
                if (arrayOpt != null)
                {
                    sb.Append(this.ArrayOpening);
                    int arrayRank = arrayType1.GetArrayRank();
                    bool flag = false;
                    for (int dimension = 0; dimension < arrayRank; ++dimension)
                    {
                        if (arrayOpt.GetLowerBound(dimension) > 0)
                        {
                            flag = true;
                            break;
                        }
                    }
                    for (int dimension = 0; dimension < arrayRank; ++dimension)
                    {
                        int lowerBound = arrayOpt.GetLowerBound(dimension);
                        int length = arrayOpt.GetLength(dimension);
                        if (dimension > 0)
                            sb.Append(", ");
                        if (flag)
                        {
                            this.AppendArrayBound(sb, lowerBound);
                            sb.Append("..");
                            this.AppendArrayBound(sb, length + lowerBound);
                        }
                        else
                            this.AppendArrayBound(sb, length);
                    }
                    sb.Append(this.ArrayClosing);
                    arrayOpt = null;
                }
                else
                    this.AppendArrayRank(sb, arrayType1);
                arrayType1 = arrayType1.GetElementType();
            }
            while (arrayType1.IsArray);
            return sb.ToString();
        }

        private void AppendArrayBound(StringBuilder sb, long bound)
        {
            CommonPrimitiveFormatterOptions options = new CommonPrimitiveFormatterOptions();
            string str = int.MinValue > bound || bound > int.MaxValue ? this.PrimitiveFormatter.FormatPrimitive(bound, options) : this.PrimitiveFormatter.FormatPrimitive((int)bound, options);
            sb.Append(str);
        }

        private void AppendArrayRank(StringBuilder sb, Type arrayType)
        {
            sb.Append('[');
            int arrayRank = arrayType.GetArrayRank();
            if (arrayRank > 1)
                sb.Append(',', arrayRank - 1);
            sb.Append(']');
        }

        private string FormatGenericTypeName(System.Reflection.TypeInfo typeInfo)
        {
            StringBuilder builder = new StringBuilder();
            Type[] genericArguments = typeInfo.IsGenericTypeDefinition ? typeInfo.GenericTypeParameters : typeInfo.GenericTypeArguments;
            if ((object)typeInfo.DeclaringType != null)
            {
                List<TypeInfo> instance2 = new List<TypeInfo>();
                do
                {
                    instance2.Add(typeInfo);
                    Type declaringType = typeInfo.DeclaringType;
                    typeInfo = (object)declaringType != null ? IntrospectionExtensions.GetTypeInfo(declaringType) : null;
                }
                while (typeInfo != null);
                int genericArgIndex = 0;
                for (int index = instance2.Count - 1; index >= 0; --index)
                {
                    this.AppendTypeInstantiation(builder, instance2[index], genericArguments, ref genericArgIndex);
                    if (index > 0)
                        builder.Append('.');
                }
            }
            else
            {
                int genericArgIndex = 0;
                this.AppendTypeInstantiation(builder, typeInfo, genericArguments, ref genericArgIndex);
            }
            return builder.ToString();
        }

        private void AppendTypeInstantiation(
          StringBuilder builder,
          System.Reflection.TypeInfo typeInfo,
          Type[] genericArguments,
          ref int genericArgIndex)
        {
            int num = (typeInfo.IsGenericTypeDefinition ? typeInfo.GenericTypeParameters.Length : typeInfo.GenericTypeArguments.Length) - genericArgIndex;
            if (num > 0)
            {
                string name = typeInfo.Name;
                int length = name.IndexOf('`');
                if (length > 0)
                    builder.Append(name.Substring(0, length));
                else
                    builder.Append(name);
                builder.Append(this.GenericParameterOpening);
                for (int index = 0; index < num; ++index)
                {
                    if (index > 0)
                        builder.Append(", ");
                    builder.Append(this.FormatTypeName(genericArguments[genericArgIndex++]));
                }
                builder.Append(this.GenericParameterClosing);
            }
            else
                builder.Append(typeInfo.Name);
        }
    }
}
