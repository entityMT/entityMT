namespace MtTenants.Abstractions
{
    public interface IConnectionStringProvider
    {
        string GetConnectionString(ITenant tenant);
    }
}