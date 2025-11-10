using System;
using System.Net.Http;

namespace ApiTests;

public class IntegrationTest : IDisposable
{
    private HttpClient? _httpClient;

    protected HttpClient HttpClient
    {
        get
        {
            if (_httpClient == default)
            {
                // Create HttpClientHandler that bypasses SSL validation for development/testing
                // DO NOT use this approach in production code
                // ^ These comments brought to you by CoPilot! This is not something I've run across before, so I had the robot help me out.
                var handler = new HttpClientHandler
                {
                    ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
                };

                _httpClient = new HttpClient(handler)
                {
                    BaseAddress = new Uri("https://localhost:7124")
                };
                _httpClient.DefaultRequestHeaders.Add("accept", "text/plain");
            }

            return _httpClient;
        }
    }

    public void Dispose()
    {
        HttpClient.Dispose();
    }
}

