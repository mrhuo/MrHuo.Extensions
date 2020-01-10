using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace MrHuo.Extensions
{
    internal static class ObjectFormatterHelpers
    {
        internal static readonly object VoidValue = new object();

        internal static bool HasOverriddenToString(System.Reflection.TypeInfo type)
        {
            if (type.IsInterface)
                return false;
            for (; type.AsType() != (object)typeof(object); type = IntrospectionExtensions.GetTypeInfo(type.BaseType))
            {
                if ((object)type.GetDeclaredMethod("ToString") != null)
                    return true;
            }
            return false;
        }

        internal static object GetMemberValue(MemberInfo member, object obj, out Exception exception)
        {
            exception = null;
            try
            {
                if (member is FieldInfo fieldInfo)
                    return fieldInfo.GetValue(obj);
                if (member is MethodInfo methodInfo)
                    return methodInfo.ReturnType == (object)typeof(void) ? ObjectFormatterHelpers.VoidValue : methodInfo.Invoke(obj, Array.Empty<object>());
                PropertyInfo propertyInfo = (PropertyInfo)member;
                if ((object)propertyInfo.GetMethod == null)
                    return null;
                return propertyInfo.GetValue(obj, Array.Empty<object>());
            }
            catch (TargetInvocationException ex)
            {
                exception = ex.InnerException;
            }
            return null;
        }

        internal static SpecialType GetPrimitiveSpecialType(Type type)
        {
            if (type == (object)typeof(int))
                return SpecialType.System_Int32;
            if (type == (object)typeof(string))
                return SpecialType.System_String;
            if (type == (object)typeof(bool))
                return SpecialType.System_Boolean;
            if (type == (object)typeof(char))
                return SpecialType.System_Char;
            if (type == (object)typeof(long))
                return SpecialType.System_Int64;
            if (type == (object)typeof(double))
                return SpecialType.System_Double;
            if (type == (object)typeof(byte))
                return SpecialType.System_Byte;
            if (type == (object)typeof(Decimal))
                return SpecialType.System_Decimal;
            if (type == (object)typeof(uint))
                return SpecialType.System_UInt32;
            if (type == (object)typeof(ulong))
                return SpecialType.System_UInt64;
            if (type == (object)typeof(float))
                return SpecialType.System_Single;
            if (type == (object)typeof(short))
                return SpecialType.System_Int16;
            if (type == (object)typeof(ushort))
                return SpecialType.System_UInt16;
            if (type == (object)typeof(DateTime))
                return SpecialType.System_DateTime;
            if (type == (object)typeof(sbyte))
                return SpecialType.System_SByte;
            if (type == (object)typeof(object))
                return SpecialType.System_Object;
            return type == (object)typeof(void) ? SpecialType.System_Void : SpecialType.None;
        }

        internal static ObjectDisplayOptions GetObjectDisplayOptions(
          bool useQuotes = false,
          bool escapeNonPrintable = false,
          bool includeCodePoints = false)
        {
            ObjectDisplayOptions objectDisplayOptions = ObjectDisplayOptions.None;
            if (useQuotes)
                objectDisplayOptions |= ObjectDisplayOptions.UseQuotes;
            if (escapeNonPrintable)
                objectDisplayOptions |= ObjectDisplayOptions.EscapeNonPrintableCharacters;
            if (includeCodePoints)
                objectDisplayOptions |= ObjectDisplayOptions.IncludeCodePoints;
            return objectDisplayOptions;
        }
    }
}
