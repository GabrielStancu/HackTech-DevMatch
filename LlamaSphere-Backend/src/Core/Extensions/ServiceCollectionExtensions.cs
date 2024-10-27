using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Core.Extensions;
public static class ServiceCollectionExtensions
{
    public static OptionsBuilder<T> AddConfiguration<T>(this IServiceCollection services,
        string sectionName, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton) where T : class
    {
        services.Add(new ServiceDescriptor(typeof(T), provider =>
        {
            var options = provider.GetRequiredService<IOptions<T>>();
            return options.Value;
        }, serviceLifetime));

        return services.AddOptions<T>().Configure<IConfiguration>((customSetting, configuration) =>
        {
            configuration.GetSection(sectionName).Bind(customSetting);
        });
    }
}
