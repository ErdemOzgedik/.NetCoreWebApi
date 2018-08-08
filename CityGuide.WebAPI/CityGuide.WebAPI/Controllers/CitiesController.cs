using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CityGuide.WebAPI.Data;
using CityGuide.WebAPI.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityGuide.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly IAppRepository _appRepository;

        public CitiesController(IAppRepository appRepository)
        {
            _appRepository = appRepository;
        }

        [HttpGet]
        public ActionResult GetCities()
        {
            var cities = _appRepository.GetCities().Select(c=> new CityForListDto
            {
                Id = c.Id,
                Description = c.Description,
                Name = c.Name,
                PhotoUrl = c.Photos.SingleOrDefault(p=>p.IsMain == true).Url 
            }).ToList();
            return Ok(cities);
        }


    }
}