
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using FC.Extension.SQL.Mongo.Interface;
using MongoDB.Driver;

namespace FC.Extension.SQL.Engine
{
    public static class UpdateHandler
    {


        public static async Task<T> Update<T>
            (this T model, Expression<Func<T, bool>> filter) where T : class
        {
            T? entity = null;
            if (SQLExtension.SQLConfig == null) return model;


            INoSQLBaseAccess<T> baseAccess = SQLExtension.GetNoSQLCompiler<T>();
            entity = await baseAccess.UpdateAsync(filter, model);
            return entity;
        }
        
    }


}

