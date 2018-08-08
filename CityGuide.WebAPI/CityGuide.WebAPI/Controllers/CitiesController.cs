using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CityGuide.WebAPI.Data;
using CityGuide.WebAPI.Dtos;
using CityGuide.WebAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityGuide.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CitiesController : ControllerBase
    {
        private readonly IAppRepository _appRepository;
        private IMapper _mapper;

        public CitiesController(IAppRepository appRepository, IMapper mapper)
        {
            _appRepository = appRepository;
            _mapper = mapper;
        }

        [HttpGet]
        public ActionResult GetCities()
        {
            #region beforeAutoMapper
            //var cities = _appRepository.GetCities().Select(c=> new CityForListDto
            //{
            //    Id = c.Id,
            //    Description = c.Description,
            //    Name = c.Name,
            //    PhotoUrl = c.Photos.SingleOrDefault(p=>p.IsMain == true).Url 
            //}).ToList();
            #endregion

            var cities = _appRepository.GetCities();
            var dtoCities = _mapper.Map<List<CityForListDto>>(cities);

            return Ok(cities);
        }

        [HttpPost]
        [Route("add")]
        public ActionResult AddCity([FromBody] City city)
        {
            _appRepository.Add<City>(city);
            _appRepository.SaveAll();
            return Ok(city);
        }


    }
}