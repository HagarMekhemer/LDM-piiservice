using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace LDM_PIIService.Helpers
{
    public static class HttpHelper
    {
        private static readonly HttpClient _client = new HttpClient();

        private static readonly JsonSerializerOptions _defaultJsonOptions = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            DictionaryKeyPolicy = JsonNamingPolicy.CamelCase,
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.All),
            PropertyNameCaseInsensitive = true
        };

        private static readonly FileLogger _logger = FileLogger.GetInstance("HttpHelper");

        public static async Task<T> PostJsonAsync<T>(string url, Dictionary<string, string> Data, string bearerToken)
        {
            if (string.IsNullOrWhiteSpace(bearerToken))
            {
                throw new ArgumentException("Bearer token is required.", nameof(bearerToken));
            }

            var json = JsonSerializer.Serialize(Data, _defaultJsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Sending POST (JSON) to {url}");
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"JSON Body: {json}");

            var response = await _client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Response Status: {response.StatusCode}");
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Response Body: {responseContent}");

            if ((int)response.StatusCode >= 200 && (int)response.StatusCode <= 399)
            {
                return JsonSerializer.Deserialize<T>(responseContent, _defaultJsonOptions);
            }
            else
            {
                _logger.WriteToLogFile(ActionTypeEnum.Error, $"Request to {url} failed with status code {response.StatusCode}. Response: {responseContent}");
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
            }
        }

        public static async Task<T> PostFormAsync<T>(string url, object FormData)
        {
            var keyValuePairs = FormData.GetType()
                .GetProperties()
                .Where(p => p.CanRead && p.GetValue(FormData) != null)
                .ToDictionary(p => p.Name, p => p.GetValue(FormData)!.ToString());

            var content = new FormUrlEncodedContent(keyValuePairs);

            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Sending POST (FormUrlEncoded) to {url}");
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Form Data: {string.Join(", ", keyValuePairs.Select(kv => $"{kv.Key}={kv.Value}"))}");

            var response = await _client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Response Status: {response.StatusCode}");
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Response Body: {responseContent}");

            if ((int)response.StatusCode >= 200 && (int)response.StatusCode <= 399)
            {
                return JsonSerializer.Deserialize<T>(responseContent, _defaultJsonOptions);
            }
            else
            {
                _logger.WriteToLogFile(ActionTypeEnum.Error, $"Request to {url} failed with status code {response.StatusCode}. Response: {responseContent}");
                throw new HttpRequestException($"Request failed with status code {response.StatusCode}");
            }
        }

    }

}

