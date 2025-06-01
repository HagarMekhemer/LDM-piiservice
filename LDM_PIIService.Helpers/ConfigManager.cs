using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDM_PIIService.Helpers
{
    public class ConfigManager
    {
        private readonly IConfiguration _configuration;

        public ConfigManager(IConfiguration configuration)
        {
            _configuration = configuration;
            LogPath = GetKey<string>("LogPath");
            ClientId = GetKey<string>("AuthSettings:ClientId");
            ClientSecret = GetKey<string>("AuthSettings:ClientSecret");
            AuthUrl = GetKey<string>("AuthSettings:AuthUrl");
            PdfServiceUrl = GetKey<string>("PdfService:Url");
            IntervalInMinutes = GetKey<int>("TimeLogger:IntervalInMinutes");
        }

        public string LogPath { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }
        public string AuthUrl { get; }
        public string PdfServiceUrl { get; }
        public int IntervalInMinutes { get; }


        //will be updated later>after connection with db
        private T GetKey<T>(string key)
        {
            try
            {
                var value = _configuration[key];
                return (T)Convert.ChangeType(value, typeof(T));
            }

            catch
            {
                return default;
            }
        }

    }
}
