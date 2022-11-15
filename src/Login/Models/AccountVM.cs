using System.ComponentModel.DataAnnotations;

namespace Login.Models
{
    public class LoginVM
    {
        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get; set; }
    }


}
