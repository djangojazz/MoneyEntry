using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MoneyEntry.ExpensesAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((context, config) =>
            {
                var builtConfig = config.Build();

                var keyVaultConfigBuilder = new ConfigurationBuilder();

                keyVaultConfigBuilder.AddAzureKeyVault(
                    $"https://{builtConfig["Azure:Vault"]}.vault.azure.net/",
                    builtConfig["Azure:ClientId"],
                    builtConfig["Azure:ClientSecret"]);

                var keyVaultConfig = keyVaultConfigBuilder.Build();

                config.AddConfiguration(keyVaultConfig);
            })
                .UseStartup<Startup>()
                .Build();
    }
}
