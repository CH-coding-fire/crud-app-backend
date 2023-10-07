using fourthAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace fourthAPI.DTOs
{
    public class TodoItemCreationDTO
    {
        public int todoGroupId { get; set; }
        public string name { get; set; }  // Name property added as per frontend
        public string description { get; set; }
        public DateTime dueDate { get; set; }  // Date property for the due date

        public string status { get; set; }  // Enum for task status
        public string tag { get; set; }  // Enum for task tags
        public Priority priority { get; set; }  // Enum for priority
    }

    public class TodoItemEditDTO : TodoItemCreationDTO
    {
        public int id { get; set; }
        public int? todoGroupId { get; set; }

    }
}
