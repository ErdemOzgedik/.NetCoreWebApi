using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CityGuide.WebAPI.Data;
using CityGuide.WebAPI.Dtos;
using CityGuide.WebAPI.Helpers;
using CityGuide.WebAPI.Models;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CityGuide.WebAPI.Controllers
{
    [Route("api/cities/{cityId}/photos")]
    [ApiController]
    public class PhotosController : ControllerBase
    {
        private IAppRepository _appRepository;
        private IMapper _mapper;
        private IOptions<CloudinarySettings> _cloudinaryOptions; // config
        private Cloudinary _cloudinary;
        public PhotosController(IMapper mapper, IAppRepository appRepository, IOptions<CloudinarySettings> cloudinaryOptions)
        {
            _mapper = mapper;
            _appRepository = appRepository;
            _cloudinaryOptions = cloudinaryOptions;
            Account account = new Account(
                _cloudinaryOptions.Value.CloudName,
                _cloudinaryOptions.Value.ApiKey,
                _cloudinaryOptions.Value.ApiSecret);
            _cloudinary = new Cloudinary(account);
        }

        [HttpPost]
        public ActionResult AddPhotoForCity(int cityId, [FromBody] PhotoForCreationDto creationDto)
        {
            var city = _appRepository.GetCityById(cityId);
            if (city == null)
            {
                return BadRequest("Could not find the city.");
            }

            var currentUserId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value); // get current user Id

            if (currentUserId != city.UserId)
            {
                return Unauthorized();
            }

            var file = creationDto.File;
            var uploadResult = new ImageUploadResult();
            if (file.Length > 0)
            {
                using (var stream = file.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(file.Name,stream)
                    };
                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            creationDto.Url = uploadResult.Uri.ToString();
            creationDto.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(creationDto);
            photo.City = city;
            if (!city.Photos.Any(p=>p.IsMain))
            {
                photo.IsMain = true;
            }
            city.Photos.Add(photo);
            if (_appRepository.SaveAll())
            {
                var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
                return CreatedAtRoute("GetPhoto", new {id = photo.Id}, photoToReturn);
            }

            return BadRequest("Could not add the photo");
        }

        [HttpGet("{id}")]
        public ActionResult GetPhoto(int id)
        {
            var photoFromDb = _appRepository.GetPhoto(id);
            var photo = _mapper.Map<PhotoForReturnDto>(photoFromDb);
            return Ok(photo);
        }

    }
}