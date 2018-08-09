using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MoneyEntry.DataAccess.EFCore;

namespace MoneyEntry.ExpensesAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddCors();
            
            if(Configuration["SQLServer"] != null && Configuration["SQLUser"] != null && Configuration["SQLPassword"] != null)
            {
                var connectionBuilder = new SqlConnectionStringBuilder
                {
                    DataSource = Configuration["SQLServer"],
                    InitialCatalog = "Expenses",
                    UserID = Configuration["SQLUser"],
                    Password = Configuration["SQLPassword"]
                };
            }



            ExpensesRepository.SetConnectionFirstTime(
                //connectionBuilder.ConnectionString);
                Configuration.GetConnectionString("Expenses"));
            ExpensesRepository.Instance.SeedDatabase();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                //Enables CORS for anything for the time being
                app.UseCors(builder =>
                    builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                   );
            }

            app.UseMvc();
        }
    }
}
