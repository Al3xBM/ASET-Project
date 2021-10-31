using System.ComponentModel.DataAnnotations;

namespace UserService.DataTransferObjects
{
    public class UserAuthenticateDTO
    {
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
    }
}
