using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ToDo_API.Models;

public class ToDo
{
    [BsonId]
    [BindNever]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonIgnore]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString()!;
    
    [BsonElement("UserID")]
    [BindNever]
    [BsonRepresentation(BsonType.ObjectId)]
    [JsonIgnore]
    public string UserId { get; set; } = ObjectId.GenerateNewId().ToString()!;
    
    [Required]
    [StringLength(100, ErrorMessage = "Title length can't be more than 100.")]
    [BsonElement("Title")]
    public required string Title { get; set; }
    
    [BsonIgnoreIfNull]
    [StringLength(500, ErrorMessage = "Description length can't be more than 500.")]
    [BsonElement("Description")]
    public string? Description { get; set; }

    [BsonElement("CreatedOn")]
    [JsonIgnore]
    public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
    
    [BsonElement("IsCompleted")]
    public bool IsCompleted { get; set; } = false;

    /*
    [BsonElement("DueDate")]
    public DateTime? DueDate { get; set; }*/
    
    /*[BsonElement("Priority")]
    public string Priority { get; set; }
    
    [BsonElement("Tags")]
    public List<string> Tags { get; set; } = new List<string>();

    [BsonElement("Attachments")]
    public List<string> Attachments { get; set; } = new List<string>();*/
        
    [BsonIgnoreIfNull]
    [BsonElement("SubTasks")]
    public List<SubTask> SubTasks { get; set; }
    
}

public class SubTask
{
    [BsonId]
    [BindNever]
    [JsonIgnore]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString()!;
    
    [BsonElement("Title ")]
    [Required]
    public required string Title  { get; set; }

    [BsonElement("IsCompleted")]
    public bool IsCompleted { get; set; } = false;
}