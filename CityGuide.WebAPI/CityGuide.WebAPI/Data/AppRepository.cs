using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityGuide.WebAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CityGuide.WebAPI.Data
{
    public class AppRepository : IAppRepository
    {
        private CityGuideDbContext _dbContext;

        public AppRepository(CityGuideDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        public void Add<T>(T entity) where T : class
        {
            _dbContext.Add(entity);
        }

        public void Delete<T>(T entity) where T : class
        {
            _dbContext.Remove(entity);
        }

        public void Update<T>(T entity) where T : class
        {
            _dbContext.Update(entity);
        }

        public bool SaveAll()
        {
            return _dbContext.SaveChanges() > 0;
        }

        public List<City> GetCities()
        {
            var cities = _dbContext.Cities.Include(p => p.Photos).ToList();
            return cities;
        }

        public List<Photo> GetPhotosByCity(int id)
        {
            var photos = _dbContext.Photos.Where(c => c.CityId == id).ToList();
            return photos;
        }

        public City GetCityById(int id)
        {
            var city = _dbContext.Cities.Include(p => p.Photos).FirstOrDefault(c => c.Id == id);
            return city;
        }

        public Photo GetPhoto(int id)
        {
            var photo = _dbContext.Photos.FirstOrDefault(p => p.Id == id);
            return photo;
        }
    }
}
