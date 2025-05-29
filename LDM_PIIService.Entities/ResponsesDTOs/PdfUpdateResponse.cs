using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LDM_PIIService.Entities.ResponsesDTOs
{
    public class PdfUpdateResponse
    {
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [JsonPropertyName("modifiedPdf")]
        public string ModifiedPdf { get; set; }
    }
}
