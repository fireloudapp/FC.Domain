using System.Linq.Expressions;

namespace FC.Extension.SQL.Mongo.Interface
{
    public interface IBaseAccess<TModel> where TModel : class
    {
        #region Basic CRUD

        public TModel Create(TModel model);
        public Task<TModel> CreateAsync(TModel model);
        public TModel Update(TModel model);
        public Task<TModel> UpdateAsync(TModel model);
        public int Delete(object id);
        public Task<int> DeleteAsync(object id);

        #endregion

        #region Basic Get Methods

        public TModel GetById(object id);
        public Task<TModel> GetByIdAsync(object id);
        public IEnumerable<TModel> GetAll();
        public Task<IEnumerable<TModel>> GetAllAsync();

        #endregion

        #region Condition Executions

        public IEnumerable<TModel> GetByCondition(Expression condition);
        public Task<IEnumerable<TModel>> GetByConditionAsync(Expression condition);

        #endregion

        #region Basic Scalar Methods

        public long GetRecordCount();
        public Task<long> GetRecordCountAsync();

        #endregion

        #region Dangerous Methods

        public int Truncate();
        public Task<long> TruncateAsync();
        public int DeleteAll();

        public bool CreteTable(string sqlQuery);
        public bool DeleteTable(string sqlQuery);


        #endregion

    }
}