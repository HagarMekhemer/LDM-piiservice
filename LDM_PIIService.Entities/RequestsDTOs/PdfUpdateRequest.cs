using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LDM_PIIService.Entities.RequestsDTOs
{
    public class PdfUpdateRequest
    {
        [JsonPropertyName("pdf")]
        public string Pdf { get; set; }

        [JsonPropertyName("data")]
        public Dictionary<string, string> Data { get; set; }

        [JsonPropertyName("templateId")]
        public string TemplateId { get; set; }
    }
}
