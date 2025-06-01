using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LDM_PIIService.Entities.RequestsDTOs
{
    public class PdfUpdateRequest
    {
        [Required(ErrorMessage = "The Pdf field is required.")]
        [JsonPropertyName("pdf")]
        public string Pdf { get; set; }

        [Required(ErrorMessage = "The Data field is required.")]
        [JsonPropertyName("data")]
        public Dictionary<string, string> Data { get; set; }

        [Required(ErrorMessage = "The TemplateId field is required.")]
        [JsonPropertyName("templateId")]
        public string TemplateId { get; set; }
    }
}
