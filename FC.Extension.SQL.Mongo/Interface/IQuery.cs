namespace FC.Extension.SQL.Mongo.Interface;

public interface IQuery<TModel>
{
    string Message { get; set; }
    IEnumerable<TModel> GetHandler(TModel model);
    Task<IEnumerable<TModel>> GetHandlerAsync(TModel model);
}
public interface IQueryPaging<Paging>
{
    Paging GetHandler(Paging model);
    Task<Paging> GetHandlerAsync(Paging model);
}
public interface IQueryById<TModel>
{
    TModel GetHandler(long id);
    Task<TModel> GetHandlerAsync(long id);
}
