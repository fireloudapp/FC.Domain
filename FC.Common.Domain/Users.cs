using System.ComponentModel.DataAnnotations;
using System.Runtime.InteropServices;
using System.Text.Json.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace FC.Common.Domain;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    /// <summary>
    /// Parent Id from, "Account" Table.
    /// </summary>
    public string? AccountId { get; set; }
    
    [Required]
    [StringLength(50, ErrorMessage = "Name length can't be more than 50.")]
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [Required]
    [StringLength(50, ErrorMessage = "User Name length can't be more than 50.")]
    public string Username { get; set; }

    [Required]
    [StringLength(15, ErrorMessage = "Password length can't be more than 15.")]
    public string PasswordHash { get; set; }
    
    /// <summary>
    /// Used only for login prupose.
    /// </summary>
    public string ClientDomain { get; set; }
    
    public UserCategory UserTypes { get; set; }

    [JsonIgnore] 
    public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
}

/// <summary>
/// 
/// </summary>
public enum UserCategory
{
    ClientUser,
    FcUser 
}

//[Owned]
public class RefreshToken
{
    //[Key]
    [JsonIgnore]
    public int Id { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public DateTime Created { get; set; }
    public string CreatedByIp { get; set; }
    public DateTime? Revoked { get; set; }
    public string RevokedByIp { get; set; }
    public string ReplacedByToken { get; set; }
    public string ReasonRevoked { get; set; }
    public bool IsExpired => DateTime.UtcNow >= Expires;
    public bool IsRevoked => Revoked != null;
    public bool IsActive => !IsRevoked && !IsExpired;
}