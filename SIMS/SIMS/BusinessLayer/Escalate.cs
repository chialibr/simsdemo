using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

public class CloudFunctionClient
{
    private readonly HttpClient _httpClient;
    private readonly string _functionUrl;

    public CloudFunctionClient()
    {
        _functionUrl = "https://unaliveservice-89127647723.europe-west1.run.app/";
        _httpClient = new HttpClient();
    }

    public async Task StopInstanceAsync(string recipient, string instanceId)
    {
        var payload = new
        {
            project_id = "fhstp-460810",
            zone = "europe-west1-d",
            resource_id = instanceId,
            recipient = recipient
        };
        var json = JsonSerializer.Serialize(payload);
        using var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync(_functionUrl, content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Function returned {(int)response.StatusCode}: {error}");
        }

        var responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine("Function response: " + responseBody);
    }
}
