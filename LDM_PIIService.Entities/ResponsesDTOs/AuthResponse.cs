using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LDM_PIIService.Entities.ResponsesDTOs
{
    public class AuthResponse
    {
        [Required(ErrorMessage = "The AccessToken field is required.")]
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [Required(ErrorMessage = "The Expiresin field is required.")]
        [JsonPropertyName("expires_in")]
        public int ExpiresIn { get; set; }

        [Required(ErrorMessage = "The Refresh_Expiresin field is required.")]
        [JsonPropertyName("refresh_expires_in")]
        public int RefreshExpiresIn { get; set; }

        [Required(ErrorMessage = "The Tokentype field is required.")]
        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [Required(ErrorMessage = "The NotBeforePolicy field is required.")]
        [JsonPropertyName("not-before-policy")]
        public int NotBeforePolicy { get; set; }

        [Required(ErrorMessage = "The Scope field is required.")]
        [JsonPropertyName("scope")]
        public string Scope { get; set; }
    }
}
