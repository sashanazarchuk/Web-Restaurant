using Logger;
using NLog;

namespace Restaurant.Common
{
    public static class ConfigurationHelper
    {
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            LogManager.Setup().LoadConfigurationFromFile(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            services.AddSingleton<ILoggerManager, LoggerManager>();
        }

    }
}
