using System;

namespace MrHuo.Extensions
{
    internal class CSharpTypeNameFormatter : CommonTypeNameFormatter
    {
        protected override CommonPrimitiveFormatter PrimitiveFormatter { get; }

        public CSharpTypeNameFormatter(CommonPrimitiveFormatter primitiveFormatter)
        {
            this.PrimitiveFormatter = primitiveFormatter;
        }

        protected override string GenericParameterOpening
        {
            get
            {
                return "<";
            }
        }

        protected override string GenericParameterClosing
        {
            get
            {
                return ">";
            }
        }

        protected override string ArrayOpening
        {
            get
            {
                return "[";
            }
        }

        protected override string ArrayClosing
        {
            get
            {
                return "]";
            }
        }

        protected override string GetPrimitiveTypeName(SpecialType type)
        {
            switch (type)
            {
                case SpecialType.System_Object:
                    return "object";
                case SpecialType.System_Boolean:
                    return "bool";
                case SpecialType.System_Char:
                    return "char";
                case SpecialType.System_SByte:
                    return "sbyte";
                case SpecialType.System_Byte:
                    return "byte";
                case SpecialType.System_Int16:
                    return "short";
                case SpecialType.System_UInt16:
                    return "ushort";
                case SpecialType.System_Int32:
                    return "int";
                case SpecialType.System_UInt32:
                    return "uint";
                case SpecialType.System_Int64:
                    return "long";
                case SpecialType.System_UInt64:
                    return "ulong";
                case SpecialType.System_Decimal:
                    return "decimal";
                case SpecialType.System_Single:
                    return "float";
                case SpecialType.System_Double:
                    return "double";
                case SpecialType.System_String:
                    return "string";
                default:
                    return null;
            }
        }

        public override string FormatTypeName(Type type)
        {
            string methodName;
            if (GeneratedNames.TryParseSourceMethodNameFromGeneratedName(type.Name, GeneratedNameKind.StateMachineType, out methodName))
                return methodName;
            return base.FormatTypeName(type);
        }
    }
}
