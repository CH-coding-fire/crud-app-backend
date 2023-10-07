using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace fourthAPI.Models
{
    public class TodoItem
    {
        [Key]
        public int Id { get; set; }  // Primary Key
        public string Name { get; set; }  // Name property added as per frontend
        public string Description { get; set; }
        public DateTime DueDate { get; set; }  // Date property for the due date

        [RegularExpression("NotStarted|InProgress|Completed",
            ErrorMessage = "Status can only be NotStarted, InProgress, Completed")]
        public string Status { get; set; }  // Enum for task status
        [RegularExpression("Work|Errand|Hobby",
            ErrorMessage = "Status can only be Work, Errand, Hobby")]
        public string Tag { get; set; }  // Enum for task tags
        public Priority Priority { get; set; }  // Enum for priority
        public DateTime Created { get; set; }

        // Foreign key for TodoGroup
        public int TodoGroupId { get; set; }
        [JsonIgnore]
        public TodoGroup TodoGroup { get; set; }  // Navigation property for TodoGroup
    }


    // For priority, using an enum makes sense here for strong typing and future extensibility.
    // The value of the enum can be directly saved to the database and cast back when retrieving.
    public enum Priority
    {
        One = 1,
        Two = 2,
        Three = 3,
        Four = 4,
        Five = 5
    }
}
