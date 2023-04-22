using MtPersistency.Providers;

namespace MtPersistency.SqlServer.Providers
{
    internal sealed class DefaultParameterPrefixProvider : IParameterPrefixProvider
    {
        public string GetPrefix()
        {
            return "@";
        }
    }
}