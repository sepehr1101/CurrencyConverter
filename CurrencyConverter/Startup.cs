using CurrencyConverter.Logic;
using Microsoft.Extensions.DependencyInjection;

namespace CurrencyConverter
{
    public static class Startup
    {
        public static IServiceCollection ConfigureServices()
        {
            var services = new ServiceCollection();
            services.AddTransient<EntryPoint>();
            services.AddSingleton<ICurrencyConverter, Converter>();
            return services;
        }
    }
}
