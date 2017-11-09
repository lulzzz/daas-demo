﻿using Serilog;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.Logging;

namespace DaaSDemo.SqlExecutor
{
    /// <summary>
    ///     The Database-as-a-Service demo T-SQL execution API.
    /// </summary>
    /// <remarks>
    ///     This service acts as a proxy for executing T-SQL on tenant servers within the hosting Kubernetes cluster.
    /// </remarks>
    public static class Program
    {
        /// <summary>
        ///     The main program entry point.
        /// </summary>
        /// <param name="args">
        ///     Command-line arguments.
        /// </param>
        public static void Main(string[] args) => BuildWebHost(args).Run();

        /// <summary>
        ///     Create the application web host.
        /// </summary>
        /// <param name="args">
        ///     Command-line arguments.
        /// </param>
        /// <returns>
        ///     The <see cref="IWebHost"/>.
        /// </returns>
        public static IWebHost BuildWebHost(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .ConfigureLogging(logging =>
                {
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Information()
                        .WriteTo.LiterateConsole()
                        .WriteTo.Debug()
                        .Enrich.FromLogContext()
                        .CreateLogger();

                    logging.ClearProviders();
                    logging.AddSerilog(Log.Logger);
                })
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddUserSecrets<Startup>();
                })
                .Build();
    }
}