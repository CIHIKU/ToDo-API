using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ToDo_API.Models;

public class User
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } =  ObjectId.GenerateNewId().ToString()!;
    
    [BsonElement("Email")]
    public required string Email { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("PasswordHash")]
    public byte[]? PasswordHash { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("PasswordSalt")]
    public byte[]? PasswordSalt { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("PasswordResetToken")]
    public string? PasswordResetToken { get; set; }

    [BsonIgnoreIfNull]
    [BsonElement("PasswordResetTokenExpiration")]
    public DateTime? PasswordResetTokenExpiration { get; set; }

    [BsonElement("Roles")]
    public List<string> Roles { get; set; } = new List<string>();

    [BsonElement("JwtToken")]
    public required string JwtToken { get; set; }

    [BsonElement("TokenExpiration")] 
    public required DateTime JwtTokenExpiration { get; set; }
    
    [BsonElement("RefreshToken")]
    public required string RefreshToken { get; set; }
    
    [BsonElement("OAuthIdentities")]
    public List<OAuthIdentity> OAuthIdentities { get; set; } = new List<OAuthIdentity>();
    
    [BsonElement("CreatedOn")]
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

    [BsonElement("UpdatedOn")] 
    public DateTime UpdatedOn { get; set; } = DateTime.UtcNow;
}

public class OAuthIdentity
{
    [BsonElement("Provider")]
    public required string Provider { get; set; }

    [BsonElement("ProviderId")]
    public required string ProviderId { get; set; }

    [BsonElement("ProviderKey")]
    public required string ProviderKey { get; set; }
}