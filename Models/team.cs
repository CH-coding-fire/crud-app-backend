using System;
using System.Collections.Generic;  // Import for List<>
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Policy;

namespace fourthAPI.Models
{
    public class Team
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string TeamName { get; set; }
        public DateTime Created { get; set; }
        // Navigation property
        public ICollection<User> Users { get; set; } = new List<User>();
        public ICollection<TodoGroup> TodoGroups { get; set; } = new List<TodoGroup>();
    }
}




