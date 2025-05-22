# Elsa.Weather

This package extends [Elsa Workflows](https://github.com/elsa-workflows/elsa-core) with a simple activity for retrieving weather data from a public API.

## Activity

### FetchWeatherData

Fetches current weather information from **OpenWeather**.

| Property | Type | Description | Direction |
|----------|------|-------------|-----------|
| Location | `string` | The city or location to query. | Input |
| ApiKey   | `string` | Your OpenWeather API key. | Input |
| WeatherJson | `string` | The raw JSON payload returned by the API. | Output |

### Usage

Register the extension in your `Program.cs`:

```csharp
services.AddElsa()
    .AddWeather();
```

Then use the **Fetch Weather Data** activity in your workflow to obtain the latest conditions for a given location.
