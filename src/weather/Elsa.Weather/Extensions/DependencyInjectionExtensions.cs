using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;

namespace Elsa.Weather.Extensions;

/// <summary>
/// Service collection extensions for Elsa.Weather.
/// </summary>
public static class DependencyInjectionExtensions
{
    /// <summary>
    /// Registers weather services.
    /// </summary>
    public static IServiceCollection AddWeather(this IServiceCollection services, Action<IHttpClientBuilder>? configure = null)
    {
        var builder = services.AddHttpClient(nameof(Activities.FetchWeatherData));
        configure?.Invoke(builder);
        return services;
    }
}
