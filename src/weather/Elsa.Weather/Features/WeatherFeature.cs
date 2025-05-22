using Elsa.Features.Abstractions;
using Elsa.Features.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Elsa.Weather.Features;

/// <summary>
/// Registers weather related services.
/// </summary>
public class WeatherFeature(IModule module) : FeatureBase(module)
{
    /// <summary>
    /// Optional configuration of the underlying <see cref="IHttpClientBuilder"/>.
    /// </summary>
    public Action<IHttpClientBuilder>? ConfigureHttpClient { get; set; }

    /// <inheritdoc />
    public override void Apply()
    {
        var builder = Services.AddWeather(ConfigureHttpClient);
    }
}
