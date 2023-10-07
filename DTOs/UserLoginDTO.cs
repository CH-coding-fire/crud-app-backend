using System.ComponentModel.DataAnnotations;

namespace fourthAPI.DTOs
{
    public class UserLoginDTO
    {
        [Required]
        public string username { get; set; }
        [Required]
        public string password { get; set; }
    }
}
