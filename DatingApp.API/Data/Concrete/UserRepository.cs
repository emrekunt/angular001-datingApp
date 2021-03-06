using System.Collections.Generic;
using System.Threading.Tasks;
using DatingApp.API.Data.Interfaces;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data.Concrete
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context) { }

        public override async Task<IEnumerable<User>> GetAll()
        {
            return await _context.Users.Include(p=> p.Photos).ToListAsync();
        }

        public override async Task<User> GetById(int id){
            return await _context.Users.Include(p=> p.Photos).FirstOrDefaultAsync(x=> x.Id == id);
        }
    }
}