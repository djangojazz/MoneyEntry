using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MoneyEntry.DataAccess.EFCore;

namespace MoneyEntry.ExpensesAPI
{
    public class Startup
    {
        private IConfiguration _config;

        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors();
            
            var tokenSymetricKey = Convert.FromBase64String(_config["Security:Tokens:Key"]);
            var text = Encoding.UTF8.GetString(tokenSymetricKey);

            services.AddAuthentication()
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = _config["Security:Tokens:Issuer"],
                        ValidAudience = _config["Security:Tokens:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(_config["Security:Tokens:Key"]))
                    };
                });

            Boolean.TryParse(_config["UseAzure"], out bool useAzure);
            if (useAzure)
            {
                var sqlServer = _config["SQLServer"];
                var sqlUser = _config["SQLUser"];
                var sqlPassword = _config["SQLPassword"];

                if (sqlServer != null && sqlUser != null && sqlPassword != null)
                {
                    ExpensesRepository.SetConnectionFirstTime(new SqlConnectionStringBuilder
                    {
                        DataSource = sqlServer,
                        InitialCatalog = "Expenses",
                        UserID = sqlUser,
                        Password = sqlPassword
                    }.ConnectionString);
                }
            }
            else
            {
                ExpensesRepository.SetConnectionFirstTime(_config.GetConnectionString("Expenses"));
            }

            //ExpensesRepository.SetConnectionFirstTime(_config.GetConnectionString("Expenses"));
            ExpensesRepository.Instance.SeedDatabase();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());
            }
            else
            {
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
