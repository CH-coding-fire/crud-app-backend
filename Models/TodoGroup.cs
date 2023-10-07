using System;
using System.Collections.Generic;  // Import for List<>
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fourthAPI.Models
{
    public class TodoGroup
    {
        [Key]
        public int Id { get; set; }

        // Foreign key for Team
        [ForeignKey("Team")]
        public int TeamId { get; set; }

        // Navigation property for Team
        public Team Team { get; set; }

        public string Name {  get; set; }
        public DateTime Created { get; set; }


        // Navigation property for TodoItem
        public ICollection<TodoItem> TodoItems { get; set; } = new List<TodoItem>();
    }
}
