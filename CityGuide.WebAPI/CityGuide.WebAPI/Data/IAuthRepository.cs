using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityGuide.WebAPI.Models;

namespace CityGuide.WebAPI.Data
{
    public interface IAuthRepository
    {
        Task<User> Register(User user,string password);
        Task<User> Login(string userName, string password);
        Task<bool> UserIsExist(string userName);
    }
}
