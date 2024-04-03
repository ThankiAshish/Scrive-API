using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using ScriveAPI.Data;

namespace ScriveAPI.Models
{
    public class User
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(50, ErrorMessage = "Username must not exceed 50 characters.")]
        [Column("username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Email is required.")]
        [StringLength(50, ErrorMessage = "Email must not exceed 50 characters.")]
        [Column("email")]
        [EmailAddress]
        public string Email { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [StringLength(100, ErrorMessage = "Password must not exceed 100 characters.")]
        [Column("password")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Profile picture is required.")]
        [Column("profilePicture")]
        public string ProfilePicture { get; set; }
    }
}