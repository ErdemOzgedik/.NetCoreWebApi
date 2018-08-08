using System.Collections.Generic;

namespace CityGuide.WebAPI.Models
{
    public class User
    {
        public User()
        {
            Cities = new List<City>();
        }
        public int Id { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public string Username { get; set; }

        public List<City> Cities { get; set; }
    }
}