using ContaAPI.Application;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace ContaAPI.IntegrationTests.Drivers
{
    public static class ServerSetup
    {
        public static TestServer Setup()
        {
            var webHost = new WebHostBuilder().UseStartup<Startup>();

            var configurations = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables()
                .Build();

            webHost.UseConfiguration(configurations);

            return new TestServer(webHost);
        }
    }
}
