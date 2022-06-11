namespace FC.Extension.SQL.Mongo.Interface;

/// <summary>
/// Combines both IError handler with ICommand to handle errors easily.
/// </summary>
public interface IActionCommand<TModel> : ICommand<TModel>, IError
{
    
}

public interface IError
{
    bool IsError { get; set; }
    string ErrorMessage { get; set; }
}