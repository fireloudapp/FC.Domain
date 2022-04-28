using System.Linq.Expressions;
using FC.Extension.SQL.Mongo.Helper;
using FC.Extension.SQL.Mongo.Interface;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace FC.Extension.SQL.Mongo;

 public class MongoDataAccess<TModel> : INoSQLBaseAccess<TModel> where TModel : class
    {
        private readonly IMongoCollection<TModel> _modelCollection;
        private readonly IMongoCollection<BsonDocument> _genericCollection;
        
        #region Property
        /// <summary>
        /// Used for customized or generic way of handling model objects.
        /// </summary>
        public IMongoCollection<BsonDocument> GenericCollection
        {
            get;
        }

        /// <summary>
        /// Used to handle model collection for custom query
        /// </summary>
        public IMongoCollection<TModel> ModelCollection
        {
            get;
            set;
        }
    

        #endregion
        
        #region Constructor
        //Ref:https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-mongo-app?view=aspnetcore-6.0&tabs=visual-studio#add-an-entity-model
        public MongoDataAccess(SQLConfig sqlConfig)
        {
            var settings = MongoClientSettings.FromConnectionString(sqlConfig.ConnectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);
            var mongoClient = new MongoClient(settings);
            var mongoDatabase = mongoClient.GetDatabase(sqlConfig.DataBaseName);
            _modelCollection = mongoDatabase.GetCollection<TModel>(  typeof(TModel).Name);
            ModelCollection = _modelCollection;
            _genericCollection = mongoDatabase.GetCollection<BsonDocument>(typeof(TModel).Name);
            GenericCollection = _genericCollection;
            
        }
        #endregion

        #region IBaseAccess - Base Generic CRUD Operation
        public TModel Create(TModel model)
        {
            _modelCollection.InsertOne(model);
            return model;
        }
        public async Task<TModel> CreateAsync(TModel model)
        {
            await _modelCollection.InsertOneAsync(model);
            return model;
        }
        public async Task<TModel> UpdateAsync(Expression<Func<TModel, bool>> filter, TModel model)
        {
            await _modelCollection.ReplaceOneAsync(filter, model);
            return model;
        }

        public async Task<string> DeleteAsync(Expression<Func<TModel, bool>> filter, string id)
        {
            var deletedOperation = await _modelCollection.DeleteOneAsync(filter);
            return deletedOperation.ToJson();
        }

        #endregion

        #region Basic Get Operation

        /// <summary>
        /// Get by filter condition eg. (x => x.Id == id), (_ => true) etc..
        /// </summary>
        /// <param name="filter">filter condition </param>
        /// <returns>returns model data</returns>
        public async Task<IEnumerable<TModel>> GetAnyAsync(Expression<Func<TModel, bool>> filter)
        {
            return await _modelCollection.Find(filter).ToListAsync();
        }
        /// <summary>
        /// Get all data from the table so called as documents. In general use it for small tables and not for larger tables.
        /// </summary>
        /// <returns>list of model data, without any filter.</returns>
        public async Task<IEnumerable<TModel>> GetAllAsync()
        {
            return await _modelCollection.Find(_ => true).ToListAsync();
        }


        #endregion

        #region Basic Scalar Methods
        public async Task<long> GetRecordCountAsync()
        {
            return await _modelCollection.CountDocumentsAsync(new BsonDocument());
        }
        #endregion

        #region Pagination
        //Ref: https://mongodb.github.io/mongo-csharp-driver/2.14/reference/driver/expressions/
        public async Task<IEnumerable<TModel>> GetByPagingAsync<TField>
        (Func<TModel, TField> orderBy, int page = 0,
            int rowsPerBatch = 10) where TField : class
        {
            var query = _modelCollection.Find(_ => true);
            var result  = await query .Skip(page).Limit(rowsPerBatch).ToListAsync();
            result = result.OrderBy(orderBy).ToList();
            return result;
        }

        public async Task<IEnumerable<TModel>> GetByPagingAsync<TField>
        (Func<TModel, TField> orderBy, Expression<Func<TModel, bool>> filter ,  int page = 0,
            int rowsPerBatch = 10) where TField : class
        {
            var query = _modelCollection.Find(filter);
            var result  = await query .Skip(page).Limit(rowsPerBatch).ToListAsync();
            result = result.OrderBy(orderBy).ToList();
            return result;
        }
        
        /// <summary>
        /// Filter by regular expression
        /// </summary>
        /// <param name="rejexSearch">rejex filter value eg.Ref: https://www.thecodebuzz.com/mongodb-csharp-driver-like-query-examples/</param>
        /// <param name="page">which page you want to look</param>
        /// <param name="rowsPerBatch">How many records you need</param>
        /// <typeparam name="TField">Model type</typeparam>
        /// <returns>A List of Objects</returns>
        /// <example>
        /// var queryExpr = new BsonRegularExpression(new Regex(searchName, RegexOptions.IgnoreCase));
        /// var builder = Builders<BsonDocument>.Filter;
        /// FilterDefinition<BsonDocument> filter = builder.Regex("Name", queryExpr);
        /// var query = BaseAccess.GenericCollection.Find(filter);
        /// var result  = await query .Skip(page).Limit(rows).ToListAsync ();
        /// var countryList = result.Select(obj => BsonSerializer.Deserialize<TModel>(obj)).ToList();
        ///     return countryList;
        /// </example>
        public async Task<IEnumerable<TModel>> SearchByField<TField>
        (FilterDefinition<BsonDocument> rejexSearch ,  int page = 0,
            int rowsPerBatch = 10) where TField : class
        {
            var query = _genericCollection.Find(rejexSearch);
            var result  = await query .Skip(page).Limit(rowsPerBatch).ToListAsync();
            var modelList = result.Select(obj => BsonSerializer.Deserialize<TModel>(obj)).ToList();
            return modelList;
        }
        #endregion
    }