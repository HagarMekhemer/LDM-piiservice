using LDM_PIIService.DAL;
using LDM_PIIService.Entities.RequestsDTOs;
using LDM_PIIService.Helpers;
using NT.Integration.SharedKernel.OracleManagedHelper;
using System;
using System.Text.Json;
using System.Threading.Tasks;

namespace LDM_PIIService.DSL
{
    public class Get_GH_Attachment_API_DSL
    {
        private readonly Get_GH_Attachment_API_DAL _dal;
        private readonly ConfigManager _configManager;
        private readonly FileLogger _logger;

        public Get_GH_Attachment_API_DSL(Get_GH_Attachment_API_DAL dal, ConfigManager configManager)
        {
            _dal = dal;
            _configManager = configManager;
            _logger = FileLogger.GetInstance("Get_GH_Attachment_API_DSL");
        }

        public async Task<PdfUpdateRequest> GetAttachmentAsync()
        {
            _logger.WriteToLogFile(ActionTypeEnum.Information, "Start Get_GH_Attachment_API.");

            try
            {
                using var oracleManager = new OracleManager(_configManager.ConnectionString);
                await oracleManager.OpenConnectionAsync();

                var (jsonResult, seqNum) = await _dal.ExecuteGetAttachmentAsync(oracleManager);

                _logger.WriteToLogFile(ActionTypeEnum.Information, $"Got seqNum: {seqNum}");
                _logger.WriteToLogFile(ActionTypeEnum.Information, $"JSON result: {jsonResult}");

                if (string.IsNullOrWhiteSpace(jsonResult))
                {
                    _logger.WriteToLogFile(ActionTypeEnum.Warning, "No JSON data returned.");
                    return null;
                }

                var pdfRequestDto = JsonSerializer.Deserialize<PdfUpdateRequest>(jsonResult)
                oracleManager.CloseConnection();

                return pdfRequestDto;
            }
            catch (Exception ex)
            {
                _logger.WriteToLogFile(ActionTypeEnum.Exception, $"Exception in GetAttachment: {ex}");
                return null;
            }
        }
    }
}
