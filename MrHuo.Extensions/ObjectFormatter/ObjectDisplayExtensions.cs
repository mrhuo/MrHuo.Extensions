namespace MrHuo.Extensions
{
    internal static class ObjectDisplayExtensions
    {
        internal static bool IncludesOption(
          this ObjectDisplayOptions options,
          ObjectDisplayOptions flag)
        {
            return (options & flag) == flag;
        }
    }
}
