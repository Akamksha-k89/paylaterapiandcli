
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

public class iPayLaterClient
{
    private readonly HttpClient _httpClient;
    private readonly ILogger _logger;

    public iPayLaterClient(HttpClient httpClient,  ILogger logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<HttpResponseMessage> GetAsync(Uri url)
    {

        HttpResponseMessage result;
        try
        {
           result = await _httpClient.GetAsync(url);
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return result;
    }

    public async Task<HttpResponseMessage> PostAsync(Uri url, HttpContent content)
    {
        HttpResponseMessage result;
        try
        {
                result = await _httpClient.PostAsync(url, content);
        }
        catch (Exception ex)
        {
            throw ex;
            
        }
        return result;
    }

    public async Task<HttpResponseMessage> PutAsync(Uri url, HttpContent content)
    {
        HttpResponseMessage response;
        try
        {
                response = await _httpClient.PutAsync(url, content);
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
        return response;
    }
}
