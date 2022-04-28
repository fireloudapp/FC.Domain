using System.ComponentModel.DataAnnotations;
using System.Linq;
using FC.Common.Domain;
using FC.Extension.SQL.Mongo;
using FC.Extension.SQL.Mongo.Helper;
using FC.Extension.SQL.Mongo.Interface;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace FC.Edu.DataAccess.Organization.AccountAction;

public class AccountStatusHandler: MongoDataAccess<Account>, IQuery<Account>
{
    #region Variable
    private SQLConfig _config;
    #endregion
  
    #region Constructor

    /// <summary>
    /// Initializes the Account based configuration with MongoDB
    /// </summary>
    /// <param name="config">Account DB configuration AccountMaster DB.</param>
    public AccountStatusHandler(SQLConfig config) : base(config)
    {
        _config = config;
    }

    #endregion

    #region Action Handlers

    public IEnumerable<Account> GetHandler(Account model)
    {
        throw new NotImplementedException();
    }

    /// <summary>
    /// Gets the account status as is active or not active, if active returns the account. It matches with 'ServiceDomain' URL & 'IsActive' == 'true'.
    /// </summary>
    /// <param name="model">Account object with 'ServiceDomain' URL & 'IsActive'</param>
    /// <returns>Account object</returns>
    /// <exception cref="ValidationException">If not active this throws validation error on 'ServiceDomain' URL & 'IsActive'</exception>
    public async Task<IEnumerable<Account>> GetHandlerAsync(Account model)
    {
        IEnumerable<Account>? accounts = null;
        try
        {
            #region 1.Check if the account is Active or not
            INoSQLBaseAccess<Account> baseAccountAccess = new MongoDataAccess<Account>(_config);
            var query =
                baseAccountAccess.ModelCollection.AsQueryable()
                    .Where(e => e.ServiceDomain == model.ServiceDomain)
                    .Where(e => e.IsActive == true)
                    .Select(e => e);
            accounts = await query.ToListAsync();
            
            if (accounts == null)
            {
                throw new ValidationException("Account is not active or invalid domain");
            }
            #endregion
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }

        return accounts;
    }
    #endregion
    
    public string Message { get; set; }
}