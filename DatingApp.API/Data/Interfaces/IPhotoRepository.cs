using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data.Interfaces
{
    public interface IPhotoRepository : IBaseRepository<Photo> { 

        Task<Photo> GetMainPhoto(int userId);
     }

}