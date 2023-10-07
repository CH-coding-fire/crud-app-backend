using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fourthAPI.Models
{
    public class User 
    {

        [Key] public int Id { get; set; }
        [ForeignKey("Team")]
        public int TeamId { get; set; } 
        public string Username { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;

        public Team Team { get; set; }  // Navigation property

        [RegularExpression("Admin|Member", ErrorMessage = "Role can only be Admin or Member")]
        public string Role { get; set; }
    }
}
