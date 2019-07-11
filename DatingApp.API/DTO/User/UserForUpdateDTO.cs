using System;

namespace DatingApp.API.DTO.User
{
    public class UserForUpdateDTO
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string Interests { get; set; }
        public string Introduction { get; set; }
        public string LookingFor { get; set; }

    }
}