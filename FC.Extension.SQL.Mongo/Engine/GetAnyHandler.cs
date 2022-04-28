
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FC.Extension.SQL.Mongo.Interface;

namespace FC.Extension.SQL.Engine
{
    /// <summary>
    /// A Class that handles and executes any query and retrieves data
    /// </summary>
    public static class GetAnyHandler
    {

        public static async Task<IEnumerable<T>> GetAny<T>(this T model, Expression<Func<T, bool>> filter)
            where T : class
        {
            IEnumerable<T> modelList = null;
            if (SQLExtension.SQLConfig == null) return null;



            INoSQLBaseAccess<T> baseAccess = SQLExtension.GetNoSQLCompiler<T>();
            modelList = await baseAccess.GetAnyAsync(filter);


            return modelList;
        }

    }
}
