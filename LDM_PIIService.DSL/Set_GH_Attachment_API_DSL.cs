using LDM_PIIService.DAL;
using LDM_PIIService.Entities.RequestsDTOs;
using LDM_PIIService.Entities.ResponsesDTOs;
using LDM_PIIService.Helpers;
using NT.Integration.SharedKernel.OracleManagedHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace LDM_PIIService.DSL
{
    public class Set_GH_Attachment_API_DSL
    {
        private readonly ConfigManager _configManager;
        private readonly Set_GH_Attachment_API_DAL _setDal;
        private readonly Get_GH_Attachment_API_DAL _getDal;
        private readonly FileLogger _logger;

        public Set_GH_Attachment_API_DSL(Set_GH_Attachment_API_DAL setDal, Get_GH_Attachment_API_DAL getDal, ConfigManager configManager)
        {
            _setDal = setDal;
            _getDal = getDal;
            _configManager = configManager;
            _logger = FileLogger.GetInstance("Set_GH_Attachment_API_DSL");
        }

        public async Task<PdfUpdateResponse> UpdatePdfStatusAsync(PdfUpdateRequest request)
        {
            _logger.WriteToLogFile(ActionTypeEnum.Information, "Starting UpdatePdfStatus...");

            int status = 2;

            try
            {
                using var oracleManager = new OracleManager(_configManager.ConnectionString);
                oracleManager.OpenConnectionAsync();

                var (jsonResult, seqNum) = await _getDal.ExecuteGetAttachmentAsync(oracleManager);

                _logger.WriteToLogFile(ActionTypeEnum.Information, $"Got seqNum: {seqNum} from Get_GH_Attachment_API");

                byte[] fileContent = Convert.FromBase64String(request.Pdf);

                int result = await _setDal.ExecuteSetGhAttachmentApi(oracleManager, fileContent, (int)seqNum, status);

                _logger.WriteToLogFile(ActionTypeEnum.Information, $"Procedure executed. Result: {result}");

                if (result > 0)
                {
                    status = 1;
                    return new PdfUpdateResponse
                    {
                        Status = "Success",
                        ModifiedPdf = request.Pdf
                    };
                }
                else
                {
                    status = 2;
                    return new PdfUpdateResponse
                    {
                        Status = "Failed",
                        ModifiedPdf = ""
                    };
                }
            }
            catch (Exception ex)
            {
                status = 2;
                _logger.WriteToLogFile(ActionTypeEnum.Exception, $"Exception in UpdatePdfStatus: {ex}");

                return new PdfUpdateResponse
                {
                    Status = "Error",
                    ModifiedPdf = ""
                };
            }
        }
    }
}

