using System;
using System.Data;
using Dapper;
using Npgsql;
using swg.mining.core;

namespace algotest.core.services
{
    public interface ICommonService
    {
        IDbConnection DbConnection();
        string GetErrorMessage(string methodeName, Exception ex);
    }
    public class CommonService : ICommonService
    {
        public IDbConnection DbConnection()
        {
            SimpleCRUD.SetDialect(SimpleCRUD.Dialect.PostgreSQL);
            return new NpgsqlConnection(AppSettings.ConnectionString);

        }
        public string GetErrorMessage(string methodeName, Exception ex)
        {
            string errMessage = ex.Message;
            if (ex.InnerException != null)
            {
                errMessage = ex.InnerException.Message;
                if (ex.InnerException.InnerException != null)
                {
                    errMessage = ex.InnerException.InnerException.Message;
                    if (ex.InnerException.InnerException.InnerException != null)
                    {
                        errMessage = ex.InnerException.InnerException.InnerException.Message;
                    }
                }
            }
            var lineNumber = 0;
            const string lineSearch = ":line ";
            var index = ex.StackTrace.LastIndexOf(lineSearch);
            if (index != -1)
            {
                var lineNumberText = ex.StackTrace.Substring(index + lineSearch.Length);
                if (int.TryParse(lineNumberText, out lineNumber))
                {
                }
            }
            return methodeName + ", line: " + lineNumber + Environment.NewLine + "Error Message: " + errMessage;
        }
    }
}
