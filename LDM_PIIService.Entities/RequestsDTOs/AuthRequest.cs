using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LDM_PIIService.Entities.RequestsDTOs
{
    public class AuthRequest
    {
        [Required(ErrorMessage = "The GrantType field is required.")]
        [JsonPropertyName("grant_type")]
        public string GrantType { get; set; }

        [Required(ErrorMessage = "The ClientAuthenticationMethod field is required.")]
        [JsonPropertyName("client_authentication_method")]
        public string ClientAuthenticationMethod { get; set; }

        [Required(ErrorMessage = "The ClientSecret field is required.")]
        [JsonPropertyName("client_secret")]
        public string ClientSecret { get; set; }

        [Required(ErrorMessage = "The ClientID field is required.")]
        [JsonPropertyName("client_id")]
        public string ClientId { get; set; }
    }
}

