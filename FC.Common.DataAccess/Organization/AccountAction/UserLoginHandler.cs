using System.ComponentModel.DataAnnotations;
using System.Linq;
using FC.Extension.SQL.Mongo;
using MongoDB.Driver;
using BCrypt.Net;
using FC.Common.Domain;
using FC.Edu.DataAccess.Utility;
using FC.Extension.SQL.Mongo.Helper;
using FC.Extension.SQL.Mongo.Interface;
using MongoDB.Bson;
using MongoDB.Driver.Linq;

namespace FC.Edu.DataAccess.Organization.AccountAction;

public class UserLoginHandler : MongoDataAccess<User>, ICommand<User>
{
    #region Variable
    private SQLConfig _accountConfig;
    //private MongoSettings _mongoSettings;
    #endregion
  
    #region Constructor

    public UserLoginHandler(SQLConfig accountConfig) : base(accountConfig)
    {
        //_mongoSettings = mongoSettings;
        _accountConfig = accountConfig;
    }

    #endregion

    #region Action Handlers
    public User CommandHandler(User model)
    {
        throw new NotImplementedException();
    }

    public async Task<User> CommandHandlerAsync(User model)
    {
        User user = null;
        try
        {
            #region 1.Check if the account is Active or not

            IQuery<Account> accountQuery = new AccountStatusHandler(_accountConfig);
            var accounts = await accountQuery.GetHandlerAsync(new Account()
            {
                ServiceDomain = model.ClientDomain
            });
            var activeAccount = accounts.FirstOrDefault();
            if (activeAccount == null)
            {
                throw new AppException("Account Verification Failed.Account is not Active or 'ServiceDomain' does not matched with' ClientDomain'");
            }

#if DEBUG
            Console.WriteLine($"Client Id: {activeAccount.Id}");
            Console.WriteLine($"Client Account: {activeAccount.ToJson()}");
#endif

            #endregion

            #region 2.If account active check the user name and password matches or not.

#if DEBUG
            Console.WriteLine($"Login User: {model.ToJson()}");
            Console.WriteLine(string.Empty);
#endif

            SQLConfig userConfig = new SQLConfig()
            {
                Compiler = SQLCompiler.MongoDB,
                DBType = DBType.NoSQL,
                ConnectionString = string.Format(activeAccount.ClientConnectionString, activeAccount.ClientDbName),
                DataBaseName = activeAccount.ClientDbName,
                CollectionName = nameof(User)
            };

            INoSQLBaseAccess<User> baseUserAccess = new MongoDataAccess<User>(userConfig);
            var userExists = baseUserAccess.ModelCollection.AsQueryable<User>()
                .Where(u => u.Username == model.Username)
                .Select(u => u);
            user = await userExists.FirstOrDefaultAsync(); //User Exists.
            if (user == null)
            {
                
                throw new AppException("Username is invalid and Case sensitive.");
            }
            if (!BCrypt.Net.BCrypt.Verify(model.PasswordHash, user.PasswordHash))
                throw new AppException("Password is incorrect.");

#if DEBUG
            Console.WriteLine($"User Account: {user.ToJson()}");
#endif
            


            #endregion
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return user;
    }

    #endregion

    #region IError Handler

    public bool IsError { get; set; }
    public string ErrorMessage { get; set; }

    #endregion
    
}

