using System;
using System.ComponentModel.DataAnnotations;

namespace fourthAPI.Models
{
	public class Issue
	{
        [Key]
        public int Id { get; set; }
        [Required]

        public string Title { get; set; }
		[Required]

		public string Description { get; set; }
		public PriorityEnum Priority { get; set; }
		public IssueTypeEnum IssueType { get; set; }
		public DateTime Created { get; set; }

		public DateTime? Completed { get; set; }

		public enum PriorityEnum
		{
			Low, Medium, High
		}

        public enum IssueTypeEnum
        {
            Feature, Bug, Documentation
        }


    }
}

