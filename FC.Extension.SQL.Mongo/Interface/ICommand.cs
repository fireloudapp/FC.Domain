namespace FC.Extension.SQL.Mongo.Interface;

public interface ICommand<TModel>
{
    TModel CommandHandler(TModel model);
    Task<TModel> CommandHandlerAsync(TModel model);
}