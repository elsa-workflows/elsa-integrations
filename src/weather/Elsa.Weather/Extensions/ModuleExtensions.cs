using Elsa.Extensions;
using Elsa.Features.Services;
using Elsa.Weather.Features;

namespace Elsa.Extensions;

public static class ModuleExtensions
{
    public static IModule UseWeather(this IModule module, Action<WeatherFeature>? configure = null)
    {
        module.Configure(configure);
        return module;
    }
}
