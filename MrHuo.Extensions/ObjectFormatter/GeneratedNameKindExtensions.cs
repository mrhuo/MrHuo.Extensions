namespace MrHuo.Extensions
{
    internal static class GeneratedNameKindExtensions
    {
        internal static bool IsTypeName(this GeneratedNameKind kind)
        {
            switch (kind)
            {
                case GeneratedNameKind.LambdaDisplayClass:
                case GeneratedNameKind.StateMachineType:
                case GeneratedNameKind.DynamicCallSiteContainerType:
                    return true;
                default:
                    return false;
            }
        }
    }
}