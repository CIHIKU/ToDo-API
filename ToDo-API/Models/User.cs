using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ToDo_API.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; }
    
    [BsonElement("Email")]
    public string Email { get; set; }

    [BsonElement("PasswordHash")]
    public string PasswordHash { get; set; }

    [BsonElement("PasswordSalt")]
    public string PasswordSalt { get; set; }

    [BsonElement("IsEmailVerified")]
    public bool IsEmailVerified { get; set; } = false;

    [BsonElement("EmailVerificationToken")]
    public string EmailVerificationToken { get; set; }

    [BsonElement("PasswordResetToken")]
    public string PasswordResetToken { get; set; }

    [BsonElement("PasswordResetTokenExpiration")]
    public DateTime? PasswordResetTokenExpiration { get; set; }

    [BsonElement("Roles")]
    public List<string> Roles { get; set; } = new List<string>();

    [BsonElement("OAuthIdentities")]
    public List<OAuthIdentity> OAuthIdentities { get; set; } = new List<OAuthIdentity>();

    [BsonElement("CreatedOn")]
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
}

public class OAuthIdentity
{
    [BsonElement("Provider")]
    public string Provider { get; set; }

    [BsonElement("ProviderId")]
    public string ProviderId { get; set; }

    [BsonElement("ProviderKey")]
    public string ProviderKey { get; set; }

    [BsonElement("AccessToken")]
    public string AccessToken { get; set; }

    [BsonElement("RefreshToken")]
    public string RefreshToken { get; set; }
}