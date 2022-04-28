
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FC.Extension.SQL.Mongo;
using FC.Extension.SQL.Mongo.Helper;
using FC.Extension.SQL.Mongo.Interface;

namespace FC.Extension.SQL.Engine
{
    /// <summary>
    /// A Class that provies basic property in order to execute our query
    /// </summary>
    public static class SQLExtension
    {
        /// <summary>
        /// SQL Conifguation
        /// </summary>
        public static SQLConfig SQLConfig { get; set;  }
        
        /// <summary>
        /// Gets the compiler base object for executing the model object, specific to NoSQL.
        /// </summary>
        /// <typeparam name="TModel">Model type</typeparam>
        /// <returns>returns the connection object to execute model in the database</returns>
        public static INoSQLBaseAccess<TModel> GetNoSQLCompiler<TModel>() where TModel : class
        {
            INoSQLBaseAccess<TModel> baseAccess = null;
            switch (SQLConfig.Compiler)
            {
                case SQLCompiler.MongoDB:
                    baseAccess = new MongoDataAccess<TModel>(sqlConfig: SQLConfig);
                    break;
                default:
                    break;
            }
            return baseAccess;
        }
    }
}
