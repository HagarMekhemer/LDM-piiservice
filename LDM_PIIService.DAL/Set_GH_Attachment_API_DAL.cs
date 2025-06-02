using NT.Integration.SharedKernel.OracleManagedHelper;
using Oracle.ManagedDataAccess.Client;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LDM_PIIService.DAL
{
    public class Set_GH_Attachment_API_DAL
    {
        public async Task<int> ExecuteSetGhAttachmentApi(OracleManager oracleManager, byte[] fileContent, int seqNum, int status)
        {
            oracleManager.CommandParameters.Add(new OracleParameter("P_file", OracleDbType.Blob, ParameterDirection.Input)
            {
                Value = fileContent
            });

            oracleManager.CommandParameters.Add(new OracleParameter("P_seq_num", OracleDbType.Int32, ParameterDirection.Input)
            {
                Value = seqNum
            });

            oracleManager.CommandParameters.Add(new OracleParameter("P_status", OracleDbType.Int32, ParameterDirection.Input)
            {
                Value = status
            });

            int result = await oracleManager.ExcuteNonQueryAsync("Set_GH_Attachment_API", CommandType.StoredProcedure);
            return result;
        }
    }
}