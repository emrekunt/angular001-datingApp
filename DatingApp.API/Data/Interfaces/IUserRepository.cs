using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Models;

namespace DatingApp.API.Data.Interfaces
{
    public interface IUserRepository : IBaseRepository<User> {

        Task<User> GetUser(int id);
        Task<IEnumerable<User>> GetAllUser();

    }
}