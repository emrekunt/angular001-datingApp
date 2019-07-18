using System;
using System.Threading.Tasks;
using DatingApp.API.Data.Interfaces;
using DatingApp.API.Models;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Data.Concrete
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext _context;
        public AuthRepository(DataContext context)
        {
            _context = context;
        }

#region Methods
        public async Task<User> Login(string username, string password)
        {
            var user = await _context.Users.Include(p => p.Photos).FirstOrDefaultAsync(x=> x.Username == username);
            if(user == null)
                return null;

            if(!VerifyPassword(password, user.Password, user.PaswordSalt))
                return null;

            return user;
        }
        
        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            GeneratePasswordHash(password, out passwordHash, out passwordSalt);

            user.Password = passwordHash;
            user.PaswordSalt = passwordSalt;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }
#endregion Methods
#region Utility
        private void GeneratePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPassword(string password, byte[] passwordHash, byte[] paswordSalt)
        {
             using(var hmac = new System.Security.Cryptography.HMACSHA512(paswordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i = 0; i < computedHash.Length; i++){
                    if(computedHash[i] != passwordHash[i])
                        return false;
                }
                
                return true;
            }
        }

        public async Task<bool> UserExist(string username)
        {
            if(await _context.Users.AnyAsync(x=> x.Username == username))
                return true;

            return false;
        }
#endregion Utility
    }

}