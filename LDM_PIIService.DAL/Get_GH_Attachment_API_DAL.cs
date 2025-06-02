using System;
using System.Data;
using System.Threading.Tasks;
using NT.Integration.SharedKernel.OracleManagedHelper;
using Oracle.ManagedDataAccess.Client;

namespace LDM_PIIService.DAL
{
    public class Get_GH_Attachment_API_DAL
    {
        public async Task<(string pJson, long seqNum)> ExecuteGetAttachmentAsync(OracleManager oracleManager)
        {
            oracleManager.CommandParameters.Add(new OracleParameter("P_JSON", OracleDbType.Clob) { Direction = ParameterDirection.Output });
            oracleManager.CommandParameters.Add(new OracleParameter("P_SEQ_NUM", OracleDbType.Int64) { Direction = ParameterDirection.Output });

            await oracleManager.ExcuteNonQueryAsync("get_GH_ATTACHMENT_API", CommandType.StoredProcedure);

            string pJson = oracleManager.CommandParameters["P_JSON"].Value?.ToString();
            long seqNum = 0;

            if (oracleManager.CommandParameters["P_SEQ_NUM"].Value != null &&
                long.TryParse(oracleManager.CommandParameters["P_SEQ_NUM"].Value.ToString(), out long parsedSeqNum))
            {
                seqNum = parsedSeqNum;
            }

            return (pJson, seqNum);
        }
    }
}
