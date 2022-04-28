
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using FC.Extension.SQL.Mongo.Interface;

namespace FC.Extension.SQL.Engine
{
    /// <summary>
    /// A Class that handles Get Object
    /// </summary>
    public static class GetByIdHandler
    {
        /// <summary>
        /// A Get method returns the model data by Id
        /// </summary>
        /// <typeparam name="T">Entity/Model Type</typeparam>
        /// <param name="model">Entity model object</param>
        /// <param name="id">An Unique id that will be retrieve model</param>
        /// <returns>returns the model with the given id.</returns>
        // public static async Task<T> Get<T>(this T model, object id) where T : class
        // {
        //     T? entity = null;
        //     if (SQLExtension.SQLConfig == null) return model;
        //
        //     INoSQLBaseAccess<T> baseAccess = SQLExtension.GetNoSQLCompiler<T>();
        //     entity = await baseAccess .GetByIdAsync(id);
        //
        //     return entity;
        // }
    }
}
