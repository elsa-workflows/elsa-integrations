using System.Net.Http;
using System.Threading.Tasks;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using JetBrains.Annotations;

namespace Elsa.Weather.Activities;

/// <summary>
/// Fetches current weather information from a public API.
/// </summary>
[Activity(
    "Elsa.Weather",
    "Weather",
    "Fetches current weather information for a location.",
    DisplayName = "Fetch Weather Data")]
[PublicAPI]
public class FetchWeatherData : Activity<string>
{
    private readonly IHttpClientFactory _httpClientFactory;

    /// <summary>
    /// Initializes a new instance of the <see cref="FetchWeatherData"/> class.
    /// </summary>
    public FetchWeatherData(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    /// <summary>
    /// The location to retrieve the weather for.
    /// </summary>
    [Input(Description = "The location to retrieve the weather for.")]
    public Input<string> Location { get; set; } = null!;

    /// <summary>
    /// The API key used to authenticate with the weather service.
    /// </summary>
    [Input(Description = "The API key used to authenticate with the weather service.")]
    public Input<string> ApiKey { get; set; } = null!;

    /// <summary>
    /// The JSON payload returned by the weather service.
    /// </summary>
    [Output(Description = "The JSON payload returned by the weather service.")]
    public Output<string?> WeatherJson { get; set; } = null!;

    /// <inheritdoc />
    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var location = context.Get(Location)!;
        var apiKey = context.Get(ApiKey)!;

        var client = _httpClientFactory.CreateClient(nameof(FetchWeatherData));
        var url = $"https://api.openweathermap.org/data/2.5/weather?q={Uri.EscapeDataString(location)}&appid={apiKey}";
        var response = await client.GetAsync(url, context.CancellationToken);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync(context.CancellationToken);
        context.Set(WeatherJson, json);
        context.SetResult(json);
    }
}
