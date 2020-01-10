namespace MrHuo.Extensions
{
    /// <summary>
    /// 任意对象格式化工具
    /// <para>Code from Microsoft.CodeAnalysis.Scripting</para>
    /// </summary>
    public abstract class ObjectFormatter
    {
        public static ObjectFormatter Instance = CSharpObjectFormatter.Instance;
        public abstract string FormatObject(object obj);
    }
}
