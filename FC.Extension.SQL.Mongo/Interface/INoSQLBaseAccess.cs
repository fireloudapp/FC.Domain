using System.Linq.Expressions;
using MongoDB.Bson;
using MongoDB.Driver;

namespace FC.Extension.SQL.Mongo.Interface;

public interface INoSQLBaseAccess<TModel> where TModel : class
{
    #region Property
    /// <summary>
    /// Used for customized or generic way of handling model objects.
    /// </summary>
    IMongoCollection<BsonDocument> GenericCollection { get; }
        
    /// <summary>
    /// Used to handle model collection for custom query
    /// </summary>
    public IMongoCollection<TModel> ModelCollection
    {
        get;
        set;
    }
        

    #endregion
    #region Basic CRUD
    public TModel Create(TModel model);
    public Task<TModel> CreateAsync(TModel model);
    public Task<TModel> UpdateAsync(Expression<Func<TModel, bool>> filter, TModel model);
    public Task<string> DeleteAsync(Expression<Func<TModel, bool>> filter, string id);
    #endregion
        
    #region Basic Get Methods

    public Task<IEnumerable<TModel>> GetAnyAsync(Expression<Func<TModel, bool>> filter);
    public Task<IEnumerable<TModel>> GetAllAsync();

    #endregion
        
    #region Basic Scalar Methods
    public Task<long> GetRecordCountAsync();

    #endregion

    #region Pagination

    Task<IEnumerable<TModel>> GetByPagingAsync<TField>
    (Func<TModel, TField> orderBy, int page = 0,
        int rowsPerBatch = 10) where TField : class;
    public Task<IEnumerable<TModel>>  GetByPagingAsync<TField>
    (Func<TModel, TField> orderBy, Expression<Func<TModel, bool>> filter, int page = 0,
        int rowsPerBatch = 10) where TField : class;

    public Task<IEnumerable<TModel>> SearchByField<TField>
    (FilterDefinition<BsonDocument> rejexSearch ,  int page = 0,
        int rowsPerBatch = 10) where TField : class;

    #endregion

}