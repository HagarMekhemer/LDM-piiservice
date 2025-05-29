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

        public static async Task<T> PostJsonAsync<T>(string url, object data, string bearerToken = null)
        {
            var json = JsonSerializer.Serialize(data, _defaultJsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            if (!string.IsNullOrWhiteSpace(bearerToken))
            {
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);
            }

            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Sending POST (JSON) to {url}");
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Request Body: {json}");

            var response = await _client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Response Status: {response.StatusCode}");
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Response Body: {responseContent}");

            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<T>(responseContent, _defaultJsonOptions);
        }

        public static async Task<T> PostFormAsync<T>(string url, Dictionary<string, string> formData)
        {
            var content = new FormUrlEncodedContent(formData);

            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Sending POST (Form) to {url}");
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Form Data: {string.Join(", ", formData.Select(kv => $"{kv.Key}={kv.Value}"))}");

            var response = await _client.PostAsync(url, content);
            var responseContent = await response.Content.ReadAsStringAsync();

            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Response Status: {response.StatusCode}");
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Response Body: {responseContent}");

            response.EnsureSuccessStatusCode();
            return JsonSerializer.Deserialize<T>(responseContent, _defaultJsonOptions);
        }
    }
}
