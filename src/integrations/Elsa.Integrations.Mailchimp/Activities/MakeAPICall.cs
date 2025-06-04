using Elsa.Integrations.Mailchimp.Activities;
using Elsa.Workflows;
using Elsa.Workflows.Attributes;
using Elsa.Workflows.Models;
using JetBrains.Annotations;
using System.Text.Json;

namespace Elsa.Integrations.Mailchimp.Activities;

/// <summary>
/// Performs an arbitrary authorized API call to Mailchimp.
/// </summary>
[Activity(
    "Elsa.Mailchimp.Core",
    "Mailchimp Core",
    "Performs an arbitrary authorized API call to Mailchimp.",
    DisplayName = "Make API Call")]
[UsedImplicitly]
public class MakeAPICall : MailchimpActivity
{
    /// <summary>
    /// The HTTP method to use (GET, POST, PUT, DELETE, PATCH).
    /// </summary>
    [Input(Description = "The HTTP method to use (GET, POST, PUT, DELETE, PATCH).")]
    public Input<string> Method { get; set; } = new("GET");

    /// <summary>
    /// The API endpoint path (e.g., 'lists', 'campaigns').
    /// </summary>
    [Input(Description = "The API endpoint path (e.g., 'lists', 'campaigns').")]
    public Input<string> Endpoint { get; set; } = null!;

    /// <summary>
    /// The request body as JSON (for POST, PUT, PATCH methods).
    /// </summary>
    [Input(Description = "The request body as JSON (for POST, PUT, PATCH methods).")]
    public Input<string?> RequestBody { get; set; } = default!;

    /// <summary>
    /// Query parameters as JSON object.
    /// </summary>
    [Input(Description = "Query parameters as JSON object.")]
    public Input<string?> QueryParameters { get; set; } = default!;

    /// <summary>
    /// The API response.
    /// </summary>
    [Output(Description = "The API response.")]
    public Output<object> Response { get; set; } = default!;

    /// <summary>
    /// The HTTP status code of the response.
    /// </summary>
    [Output(Description = "The HTTP status code of the response.")]
    public Output<int> StatusCode { get; set; } = default!;

    /// <summary>
    /// Executes the activity.
    /// </summary>
    protected override async ValueTask ExecuteAsync(ActivityExecutionContext context)
    {
        var method = context.Get(Method) ?? "GET";
        var endpoint = context.Get(Endpoint)!;
        var requestBody = context.Get(RequestBody);
        var queryParameters = context.Get(QueryParameters);

        var client = GetClient(context);

        try
        {
            // Note: This is a simplified implementation.
            // The actual MailChimp.Net library doesn't expose a generic HTTP client,
            // so this would need to be implemented using HttpClient directly
            // with the proper authentication headers.
            
            using var httpClient = new HttpClient();
            
            // Extract API key to get the data center
            var apiKey = context.Get(ApiKey)!;
            var dataCenterSuffix = apiKey.Split('-').LastOrDefault();
            var baseUrl = $"https://{dataCenterSuffix}.api.mailchimp.com/3.0/";
            
            httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Basic", 
                    Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"anystring:{apiKey}")));

            var url = baseUrl + endpoint.TrimStart('/');
            
            // Add query parameters if provided
            if (!string.IsNullOrEmpty(queryParameters))
            {
                try
                {
                    var queryParams = JsonSerializer.Deserialize<Dictionary<string, object>>(queryParameters);
                    if (queryParams?.Any() == true)
                    {
                        var queryString = string.Join("&", queryParams.Select(kv => $"{kv.Key}={Uri.EscapeDataString(kv.Value?.ToString() ?? "")}"));
                        url += "?" + queryString;
                    }
                }
                catch
                {
                    // Ignore JSON parsing errors for query parameters
                }
            }

            HttpResponseMessage response;
            
            switch (method.ToUpper())
            {
                case "GET":
                    response = await httpClient.GetAsync(url);
                    break;
                case "POST":
                    var postContent = new StringContent(requestBody ?? "{}", System.Text.Encoding.UTF8, "application/json");
                    response = await httpClient.PostAsync(url, postContent);
                    break;
                case "PUT":
                    var putContent = new StringContent(requestBody ?? "{}", System.Text.Encoding.UTF8, "application/json");
                    response = await httpClient.PutAsync(url, putContent);
                    break;
                case "DELETE":
                    response = await httpClient.DeleteAsync(url);
                    break;
                case "PATCH":
                    var patchContent = new StringContent(requestBody ?? "{}", System.Text.Encoding.UTF8, "application/json");
                    var patchRequest = new HttpRequestMessage(new HttpMethod("PATCH"), url)
                    {
                        Content = patchContent
                    };
                    response = await httpClient.SendAsync(patchRequest);
                    break;
                default:
                    throw new ArgumentException($"Unsupported HTTP method: {method}");
            }

            var responseContent = await response.Content.ReadAsStringAsync();
            var responseObject = JsonSerializer.Deserialize<object>(responseContent);

            context.Set(Response, responseObject);
            context.Set(StatusCode, (int)response.StatusCode);
        }
        catch (Exception ex)
        {
            context.Set(Response, new { error = ex.Message });
            context.Set(StatusCode, 500);
        }
    }
}