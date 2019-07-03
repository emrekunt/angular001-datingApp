using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTO
{
    public class UserForRegisterDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(8, MinimumLength=4, ErrorMessage=" Şifre alanı 4 ile 8 karakter arasında olmalıdır.")]
        public string Password { get; set; }
    }
}