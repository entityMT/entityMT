using Microsoft.Extensions.DependencyInjection;
using MTQueries.Abstractions;
using MTQueries.Abstractions.ClausuleManagers;
using MTQueries.Abstractions.ClausuleManagers.MemberAccessHandlers;
using MTQueries.SqlServer.MemberAccessHandlers;

namespace MTQueries.SqlServer.NetCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddSqlServerQueries(this IServiceCollection services)
        {
            services.AddScoped(typeof(IGroupByClausuleManager<>), typeof(DefaultGroupByClausuleManager<>));
            services.AddScoped(typeof(IOrderByClausuleManager<>), typeof(DefaultOrderByClausuleManager<>));
            services.AddScoped(typeof(IWhereClausuleManager<>), typeof(DefaultWhereClausuleManager<>));
            services.AddScoped<IQuery, DefaultQuery>();
            services.AddScoped(typeof(IQueryBuilder<>), typeof(DefaultQueryBuilder<>));
            services.AddScoped(typeof(IQueryHandler<>), typeof(DefaultQueryHandler<>));
            services.AddScoped(typeof(IQueryJoinsGenerator<>), typeof(DefaultQueryJoinsGenerator<>));
            services.AddScoped(typeof(IQuerySelectedColumnsProvider<>), typeof(DefaultQuerySelectedColumnsProvider<>));
            services.AddScoped<IMemberAccessHandler, PropertyInfoMemberAccessHandler>();

            return services;
        }
    }
}