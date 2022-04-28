
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FC.Extension.SQL.Mongo.Interface;

namespace FC.Extension.SQL.Engine
{
    
    /// <summary>
    /// Class that handles Delete operation
    /// </summary>
    public static class DeleteHandler
    {
        public static async Task<string> Delete<T>
            (this T model, Expression<Func<T, bool>> filter, string id) where T : class
        {
            INoSQLBaseAccess<T> baseAccess = SQLExtension.GetNoSQLCompiler<T>();
            string jsonResult = await baseAccess.DeleteAsync(filter, id);

            return jsonResult;
        }


    }
}
