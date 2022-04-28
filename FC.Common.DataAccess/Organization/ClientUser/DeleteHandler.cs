using System.ComponentModel.DataAnnotations;
using System.Linq;
using FC.Common.Domain;
using FC.Extension.SQL.Engine;
using FC.Extension.SQL.Mongo;
using FC.Extension.SQL.Mongo.Helper;
using FC.Extension.SQL.Mongo.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace FC.Edu.DataAccess.Organization.ClientUser;

public class DeleteHandler : MongoDataAccess<User>, ICommand<User>
{
    #region Variable
    private SQLConfig _config;
    #endregion

    #region Constructor
    public DeleteHandler(SQLConfig sqlConfig) : base(sqlConfig)
    {
        _config = sqlConfig;//Account configuration to be passed.
    }

    

    #endregion

    #region Command Handlers

    public async Task<User> CommandHandlerAsync(User model)
    {
        IEnumerable<User>? user = null;
        try
        {
            //1. using account id, get the account dbname & connection string.
            //2. Construct the connection string of client server & db.
            //3. Delete the user by using clients connection string.
            
            #region 1. Using account id, get the account dbname & connection string.
            
            INoSQLBaseAccess<Account> baseAccountAccess = new MongoDataAccess<Account>(_config);
            var query =
                baseAccountAccess.ModelCollection.AsQueryable()
                    .Where(e => e.Id == model.AccountId)
                    .Select(e => e);
            var accounts = await query.ToListAsync();
            var account = accounts.FirstOrDefault();
            if (account == null)
            {
                throw new ValidationException("Account is not available");
            }

            #endregion

            #region 2. Construct the connection string of client server & db.
            
            SQLConfig userConfig = new SQLConfig()
            {
                Compiler = SQLCompiler.MongoDB,
                DBType = DBType.NoSQL,
                ConnectionString = string.Format(account.ClientConnectionString, account.ClientDbName),
                DataBaseName = account.ClientDbName,
                CollectionName = nameof(User)
            };
            
            #endregion

            #region 3. Delete the user by using clients connection string.

            INoSQLBaseAccess<User> baseUserAccess = new MongoDataAccess<User>(userConfig);
            model.PasswordHash = BCrypt.Net.BCrypt.HashPassword(model.PasswordHash);
            var id = await baseUserAccess.DeleteAsync(filter:u => u.Id == model.Id, model.Id);
            #endregion
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return model;
    }

      public User CommandHandler(User model)
      {
          throw new NotImplementedException();
      }

      #endregion
  
}