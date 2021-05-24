using ContaAPI.Infra.CrossCutting.Filter;
using ContaAPI.Infra.CrossCutting.InversionOfControl;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;

namespace ContaAPI.Application
{
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddMvc(config =>
            {
                config.EnableEndpointRouting = false;
                config.Filters.Add<NotificationFilter>();
            });
            services.AddMySqlDependency(Configuration);
            services.AddMySqlRepositoryDependency();
            services.AddServiceDependency();
            services.AddSwaggerDependency();
            services.AddNotificationDependency();

            Environment.SetEnvironmentVariable("CDI", Configuration["defaultValues:cdi"]);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseMvc();
            app.UseSwaggerDependency();
        }
    }
}
