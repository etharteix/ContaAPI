using ContaAPI.Domain.Interfaces;
using ContaAPI.Infra.Data.Repository;
using Microsoft.Extensions.DependencyInjection;

namespace ContaAPI.Infra.CrossCutting.InversionOfControl
{
    public static class MySqlRepositoryDependency
    {
        public static void AddMySqlRepositoryDependency(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryUser, UserRepository>();
            services.AddScoped<IRepositoryAccount, AccountRepository>();
            services.AddScoped<IRepositoryHistoric, HistoricRepository>();
        }
    }
}
