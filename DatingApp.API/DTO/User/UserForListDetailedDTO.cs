using System.Collections.Generic;
using DatingApp.API.DTO.Photo;
using DatingApp.API.Models;

namespace DatingApp.API.DTO.User
{
    public class UserForListDetailedDTO : UserForListDTO
    {
        public string LookingFor { get; set; }
        public string Introduction { get; set; }
        public string Interests { get; set; }

        public ICollection<PhotoForDetailedDTO> Photos { get; set; }
    }
}