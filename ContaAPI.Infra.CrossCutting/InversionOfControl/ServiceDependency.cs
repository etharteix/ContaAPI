using ContaAPI.Domain.Interfaces;
using ContaAPI.Service.Services;
using Microsoft.Extensions.DependencyInjection;

namespace ContaAPI.Infra.CrossCutting.InversionOfControl
{
    public static class ServiceDependency
    {
        public static void AddServiceDependency(this IServiceCollection services)
        {
            services.AddScoped<IServiceUser, UserService>();
            services.AddScoped<IServiceAccount, AccountService>();
            services.AddScoped<IServiceHistoric, HistoricService>();
        }
    }
}
