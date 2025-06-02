using Microsoft.Extensions.Configuration;
using NT.Integration.Shield;
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
            ConnectionString= GetKey<string>("ConnectionString");
            Grant_Type= GetKey<string>("Grant_Type");
            Client_Id = GetKey<string>("Client_Id");
            Client_Secret = GetKey<string>("Client_Secret");
            Client_Authentication_Method = GetKey<string>("Client_Authentication_Method");
            AuthUrl = GetKey<string>("AuthUrl");
            PdfServiceUrl = GetKey<string>("PdfService:Url");
            IntervalInMinutes = GetKey<int>("IntervalInMinutes");
        }

        public string LogPath { get; }
        public string ConnectionString { get; }
        public string Grant_Type { get; }
        public string Client_Id { get; }
        public string Client_Secret { get; }
        public string Client_Authentication_Method { get; }
        public string AuthUrl { get; }
        public string PdfServiceUrl { get; }
        public int IntervalInMinutes { get; }


        private T GetKey<T>(string key, bool isConnectionString = false)
        {
            if (isConnectionString)
                return Decrypt<T>(_configuration.GetConnectionString(key));

            return Decrypt<T>(_configuration.GetSection(key).Value);
        }
        private T Decrypt<T>(string encryptedValue)
        {
            try
            {
                return (T)Convert.ChangeType(EncryptionHandler.DecryptText(encryptedValue, true), typeof(T));
            }
            catch (Exception)
            {
                return default;
            }
        }

    }
}
