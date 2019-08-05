using System;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.API.DTO.User
{
    public class UserForRegisterDTO
    {
        [Required]
        public string Username { get; set; }
        [Required]
        [StringLength(8, MinimumLength = 4, ErrorMessage = " Şifre alanı 4 ile 8 karakter arasında olmalıdır.")]
        public string Password { get; set; }
        [Required]
        public string KnownAs { get; set; }
        [Required]
        public string Gender { get; set; }
        [Required]
        public DateTime DateBirth { get; set; }
        [Required]
        public string City { get; set; }
        [Required]
        public string Country { get; set; }
        [Required]
        public DateTime Created { get; set; }
        public DateTime LastActive { get; set; }

        public UserForRegisterDTO()
        {
            LastActive = DateTime.Now;
            Created = DateTime.Now;
        }
    }
}