using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityGuide.WebAPI.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace CityGuide.WebAPI.Data
{
    public class CityGuideDbContext : DbContext
    {
        public CityGuideDbContext(DbContextOptions<CityGuideDbContext> options) : base(options)
        {
        }

        public DbSet<Value> Values { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Photo> Photos { get; set; }
    }
}
