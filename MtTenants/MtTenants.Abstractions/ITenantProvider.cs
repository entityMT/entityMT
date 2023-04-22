namespace MtTenants.Abstractions
{
    public interface ITenantProvider
    {
        ITenant GetTenant();
    }
}