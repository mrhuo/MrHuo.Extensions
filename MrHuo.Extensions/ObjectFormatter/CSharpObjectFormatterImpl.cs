namespace MrHuo.Extensions
{
    internal class CSharpObjectFormatterImpl : CommonObjectFormatter
    {
        protected override CommonTypeNameFormatter TypeNameFormatter { get; }

        protected override CommonPrimitiveFormatter PrimitiveFormatter { get; }

        internal CSharpObjectFormatterImpl()
        {
            this.PrimitiveFormatter = new CSharpPrimitiveFormatter();
            this.TypeNameFormatter = new CSharpTypeNameFormatter(this.PrimitiveFormatter);
        }

        protected override string FormatRefKind(System.Reflection.ParameterInfo parameter)
        {
            return !parameter.IsOut ? "ref" : "out";
        }
    }
}
