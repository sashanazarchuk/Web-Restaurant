using Entities.Data;
using Logger;
using Microsoft.EntityFrameworkCore;
using NLog;
using System;

namespace Restaurant.Common
{
    public static class ConfigurationHelper
    {
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }
        public static void ConfigureConnectionString(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ResDbContext>(options =>
            {
                options.UseSqlServer(configuration.GetConnectionString("sqlConnection"));
            });
        }

    }
}
