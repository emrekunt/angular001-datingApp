using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DatingApp.API.Data.Interfaces;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data.Concrete
{
    public class PhotoRepository : BaseRepository<Photo>, IPhotoRepository
    {
        public PhotoRepository(DataContext context) : base(context) { }

        public async Task<Photo> GetMainPhoto(int userId)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x=> x.Id == userId);
            return  user.Photos.FirstOrDefault(x=> x.IsMain);
        }
    }
}