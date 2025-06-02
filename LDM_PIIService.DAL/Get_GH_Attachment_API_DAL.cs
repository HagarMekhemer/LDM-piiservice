using System;
using System.Collections.Generic;
using System.Data;
using NT.Integration.SharedKernel.OracleManagedHelper;
using Oracle.ManagedDataAccess.Client;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDM_PIIService.DAL
{
    public class Get_GH_Attachment_API_DAL
    {
        public void ExecuteGetAttachment(OracleManager oracleManager, out string pJson, out long seqNum)
        {
            oracleManager.CommandParameters.Add(new OracleParameter("P_JSON", OracleDbType.Clob, ParameterDirection.Output));
            oracleManager.CommandParameters.Add(new OracleParameter("P_SEQ_NUM", OracleDbType.Int64, ParameterDirection.Output));
            oracleManager.ExcuteNonQueryAsync("get_GH_ATTACHMENT_API", CommandType.StoredProcedure);

            pJson = oracleManager.CommandParameters["P_JSON"].Value?.ToString();
            seqNum = oracleManager.CommandParameters["P_SEQ_NUM"].Value != null
                ? Convert.ToInt64(oracleManager.CommandParameters["P_SEQ_NUM"].Value)
                : 0;
        }
    }
}

