using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace LDM_PIIService.Entities.ResponsesDTOs
{
    public class PdfUpdateResponse
    {
        [Required(ErrorMessage = "The Status field is required.")]
        [JsonPropertyName("status")]
        public string Status { get; set; }

        [Required(ErrorMessage = "The ModifiedPdf field is required.")]
        [JsonPropertyName("modifiedPdf")]
        public string ModifiedPdf { get; set; }
    }
}
