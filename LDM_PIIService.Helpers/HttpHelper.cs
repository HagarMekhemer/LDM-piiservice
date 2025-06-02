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

        public static T PostJson<T>(string url, object data, string bearerToken)
        {
            if (string.IsNullOrWhiteSpace(bearerToken))
                throw new ArgumentException("Bearer token is required.", nameof(bearerToken));

            var json = JsonSerializer.Serialize(data, _defaultJsonOptions);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var request = new HttpRequestMessage(HttpMethod.Post, url)
            {
                Content = content
            };

            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", bearerToken);

            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Sending POST (JSON) to {url}");
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Request Body: {json}");

            var response = _client.SendAsync(request).GetAwaiter().GetResult();
            var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

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

        public static T PostForm<T>(string url, Dictionary<string, string> formData)
        {
            var content = new FormUrlEncodedContent(formData);

            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Sending POST (Form) to {url}");
            _logger.WriteToLogFile(ActionTypeEnum.Information, $"Form Data: {string.Join(", ", formData.Select(kv => $"{kv.Key}={kv.Value}"))}");

            var response = _client.PostAsync(url, content).GetAwaiter().GetResult();
            var responseContent = response.Content.ReadAsStringAsync().GetAwaiter().GetResult();

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
