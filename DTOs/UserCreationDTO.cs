using System.ComponentModel.DataAnnotations;

namespace fourthAPI.DTOs
{
    public class UserCreationDTO
    {
        [Required]
        public string username { get; set; }

        [Required]
        public string password { get; set; }

        public int teamId { get; set; }

        [Required]
        [RegularExpression("Admin|Member", ErrorMessage = "Role can only be Admin or Member")]
        public string role { get; set; }
    }
}
