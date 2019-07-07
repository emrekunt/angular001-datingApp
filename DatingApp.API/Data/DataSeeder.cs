using System.Collections.Generic;
using DatingApp.API.Models;
using Newtonsoft.Json;

namespace DatingApp.API.Data
{
    public class DataSeeder
    {
        private readonly DataContext _context;
        public DataSeeder(DataContext context)
        {
            _context = context;
        }

        public void SeedUser()
        {
            var jsonFile = System.IO.File.ReadAllText("Data/UserSeedData.json");
            var users = JsonConvert.DeserializeObject<List<User>>(jsonFile);

            foreach (var user in users)
            {
                byte[] passwordSalt, passwordHash;
                GeneratePasswordHash("1234", out passwordHash, out passwordSalt);

                user.Password = passwordHash;
                user.PaswordSalt = passwordSalt;

                _context.Users.Add(user);
            }
            
            _context.SaveChanges();
        }

        private void GeneratePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

    }
}