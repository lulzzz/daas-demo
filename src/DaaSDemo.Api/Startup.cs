﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Converters;

namespace DaaSDemo.Api
{
    using Data;
    using Provisioning;

    /// <summary>
    ///     Startup logic for the Database-as-a-Service demo API.
    /// </summary>
    public class Startup
    {
        /// <summary>
        ///     Create a new <see cref="Startup"/>.
        /// </summary>
        /// <param name="configuration">
        ///     The application configuration.
        /// </param>
        public Startup(IConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            Configuration = configuration;
        }

        /// <summary>
        ///     The application configuration.
        /// </summary>
        public IConfiguration Configuration { get; }

        /// <summary>
        ///     Configure application services.
        /// </summary>
        /// <param name="services">
        ///     The application service collection.
        /// </param>
        public void ConfigureServices(IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddEntityFrameworkSqlServer()
                .AddDbContext<Entities>(entities =>
                {
                    string connectionString = Configuration.GetValue<string>("Database:ConnectionString");

                    entities.UseSqlServer(connectionString, sqlServer =>
                    {
                        sqlServer.MigrationsAssembly("DaaSDemo.Api");
                    });
                });

            services.AddMvc()
                .AddJsonOptions(json =>
                {
                    json.SerializerSettings.Converters.Add(
                        new StringEnumConverter()
                    );
                });
            services.AddDataProtection(dataProtection =>
            {
                dataProtection.ApplicationDiscriminator = "DaaS.Demo";
            });

            services.AddSingleton<ProvisioningEngine>();
        }

        /// <summary>
        ///     Configure the application pipeline.
        /// </summary>
        /// <param name="app">
        ///     The application pipeline builder.
        /// </param>
        public void Configure(IApplicationBuilder app, ProvisioningEngine provisioningEngine, IApplicationLifetime appLifetime)
        {
            provisioningEngine.Start();

            app.UseDeveloperExceptionPage();
            app.UseMvc();

            appLifetime.ApplicationStopping.Register(() =>
            {
                provisioningEngine.Stop().Wait();
            });
            appLifetime.ApplicationStopped.Register(Serilog.Log.CloseAndFlush);
        }
    }
}
