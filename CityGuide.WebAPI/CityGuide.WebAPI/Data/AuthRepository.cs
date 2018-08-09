using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityGuide.WebAPI.Models;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;

namespace CityGuide.WebAPI.Data
{
    public class AuthRepository : IAuthRepository
    {
        private CityGuideDbContext _dbContext;

        public AuthRepository(CityGuideDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<User> Register(User user, string password)
        {
            byte[] passwordHash, passwordSalt;
            CreatePassswordHash(password, out passwordHash, out passwordSalt);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = passwordSalt;
            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();
            return user;
        }

        private void CreatePassswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public async Task<User> Login(string userName, string password)
        {
            var usert = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username == userName);
            if (usert == null)
            {
                return null;
            }

            if (!VerifyPasswordHash(password,usert.PasswordHash,usert.PasswordSalt))
            {
                return null;
            }

            return usert;
        }

        private bool VerifyPasswordHash(string password, byte[] usertPasswordHash, byte[] usertPasswordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(usertPasswordSalt))// we sending salt value
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password)); // according to salt value 
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != usertPasswordHash[i])
                    {
                        return false;
                    } 
                }

                return true;
            }
        }

        public async Task<bool> UserIsExist(string userName)
        {
            if (await _dbContext.Users.AnyAsync(x => x.Username == userName))
                return true;
            return false;
        }
    }
}
