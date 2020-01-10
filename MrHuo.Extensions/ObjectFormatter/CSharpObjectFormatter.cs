namespace MrHuo.Extensions
{
    internal sealed class CSharpObjectFormatter : ObjectFormatter
    {
        private static readonly ObjectFormatter s_impl = new CSharpObjectFormatterImpl();

        public static CSharpObjectFormatter Instance { get; } = new CSharpObjectFormatter();

        private CSharpObjectFormatter()
        {
        }

        public override string FormatObject(object obj)
        {
            return CSharpObjectFormatter.s_impl.FormatObject(obj);
        }
    }
}
