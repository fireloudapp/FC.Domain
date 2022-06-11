using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FC.Common.Domain
{

    /// <summary>
    /// Account is also called as Organization
    /// </summary>
    public class Account
    {
        /// <summary>
        /// A Unique Id to get account details.
        /// </summary>
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; } = string.Empty;

        [Required]
        [StringLength(150, ErrorMessage = "Name length can't be more than 150.")]
        public string BusinessName { get; set; }

        public bool IsActive { get; set; }

        public string Email { get; set; }

        /// <summary>
        /// The exact full domain for the application it can be custom domain url or our own domain from netlify.
        /// </summary>
        [Required]
        public string? ServiceDomain { get; set; }

        /// <summary>
        /// Full database name eg. "AVS-DB"
        /// </summary>
        [Required]
        public string? ClientDbName { get; set; }

        /// <summary>
        /// Full connection string value with the formated one
        /// eg. mongodb+srv://fc_client_admin:fc.clients.mongo@cluster0.acxm4.mongodb.net/{0}?retryWrites=true&w=majority&connect=replicaSet
        /// </summary>
        [Required]
        public string? ClientConnectionString { get; set; }

        #region Can be made in future

        //public string Client_Server { get; set; }
        // [StringLength(15, ErrorMessage = "Phone length can't be more than 15.")]
        // public string Phone { get; set; }
        // [StringLength(20, ErrorMessage = "GSTIN length can't be more than 20.")]
        // public string GSTIN { get; set; }
        // public BusinessType BusinessType { get; set; }
        //public string Description { get; set; }
        //public string BusinessCategory { get; set; }
        //public int AddressId { get; set; } // Moved to Address Table
        //public string Logo { get; set; }
        // [Required]
        // public int ActivateNoOfDays { get; set; }
        /// <summary>
        /// Should be updated or assigned the date based on 'IsActive' state;
        /// </summary>
        // public DateTime ActivationDate
        // {
        //     get;
        //     set;
        // } = DateTime.Now;
        
        #endregion
        
        /// <summary>
        /// Subscription Plan
        /// </summary>
        public SubscriptionPlan Subscription { get; set; }

        

    }
    
    /// <summary>
    /// Customer Subscribed Service
    /// </summary>
    public class SubscriptionPlan
    {
        public string PlanName { get; set; } = "Pay-as-you-use";
        public IList<SubscribedService> Services { get; set; }
    }

    public class SubscribedService
    {
        public string ServiceName { get; set; } = string.Empty;
        public long QuantityLimit { get; set; } = long.MaxValue;
        public double CostPerQuantity { get; set; } = 0.0d;
        public string CostSuffix { get; set; } = "data";

    }

    /// <summary>
    /// Client Account Users
    /// </summary>
// public class AccountUser
// {
//     public int Id { get; set; }
//     public string FirstName { get; set; }
//     public string LastName { get; set; }
//     public string Username { get; set; }
//
//     [JsonIgnore]
//     public string PasswordHash { get; set; }
//
//     [JsonIgnore]
//     public List<RefreshToken> RefreshTokens { get; set; }
// }

    public class BusinessType
    {
        public string ID { get; set; }
        public string Text { get; set; }
    }

}