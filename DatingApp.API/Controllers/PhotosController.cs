using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DatingApp.API.Data.Interfaces;
using DatingApp.API.DTO.Photo;
using DatingApp.API.Helpers;
using DatingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace DatingApp.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/users/{userId}/photos")]
    public class PhotosController : BaseApiController
    {
        private readonly IPhotoRepository _photoRepo;
        private readonly IMapper _mapper;
        private readonly IOptions<CloudinarySettings> _options;
        private Cloudinary _cloudinary;
        private readonly IUserRepository _userRepo;

        public PhotosController(IPhotoRepository photoRepo, IUserRepository userRepo,
                                IMapper mapper, IOptions<CloudinarySettings> options)
        {
            _userRepo = userRepo;
            _options = options;
            _mapper = mapper;
            _photoRepo = photoRepo;

            var account = new Account(
               _options.Value.CloudName,
               _options.Value.ApiKey,
               _options.Value.ApiSecret
            );
            _cloudinary = new Cloudinary(account);
        }

        [HttpGet("{id}", Name = "GetPhoto")]
        public async Task<IActionResult> GetPhoto(int id)
        {
            var photoFromRepo = await _photoRepo.GetById(id);
            var photo = _mapper.Map<PhotoForReturnDTO>(photoFromRepo);
            return Ok(photo);
        }

        [HttpPost]
        public async Task<IActionResult> AddPhotosForUser(int userId, [FromForm]PhotoForCreationDTO photoForCreationDTO)
        {
            if (!IsAuthenticated(userId))
                return Unauthorized();

            var uploadResult = new ImageUploadResult();
            if (photoForCreationDTO.File == null)
            {
                BadRequest("No attached files found!");
            }
            else if (photoForCreationDTO.File.Length > 0)
            {
                using (var stream = photoForCreationDTO.File.OpenReadStream())
                {
                    var uploadParams = new ImageUploadParams
                    {
                        File = new FileDescription(photoForCreationDTO.File.Name, stream),
                        Transformation = new Transformation().Width(500).Height(500)
                                                .Crop("fill").Gravity("face")
                    };

                    uploadResult = _cloudinary.Upload(uploadParams);
                }
            }

            photoForCreationDTO.Url = uploadResult.Uri.ToString();
            photoForCreationDTO.PublicId = uploadResult.PublicId;

            var photo = _mapper.Map<Photo>(photoForCreationDTO);
            var userFromRepo = await _userRepo.GetById(userId);

            if (userFromRepo == null)
                BadRequest("User can not vbe found!");

            photo.User = userFromRepo;
            photo.UserId = userFromRepo.Id;

            _photoRepo.Add(photo);

            if (await _photoRepo.SaveAll())
            {
                var photoForReturn = _mapper.Map<PhotoForReturnDTO>(photo);
                return CreatedAtRoute("GetPhoto", new { id = photo.Id }, photoForReturn);
            }

            return BadRequest("An error occurs when uploading the photo!");
        }

        [HttpPost("{id}/setMain")]
        public async Task<IActionResult> SetMainPhoto(int userId, int id)
        {
            if (!IsAuthenticated(userId))
                return Unauthorized();

            var hasPhoto = await UserHasPhoto(userId, id);
            if (!hasPhoto)
                return Unauthorized();

            var photoFromRepo = await _photoRepo.GetById(id);
            if (photoFromRepo.IsMain)
                return BadRequest("This photo is already main photo.");

            var currentMainPhoto = await _photoRepo.GetMainPhoto(userId);
            currentMainPhoto.IsMain = false;
            photoFromRepo.IsMain = true;

            if (await _photoRepo.SaveAll())
            {
                return NoContent();
            }

            return BadRequest("Cant change the main photo!");

        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePhoto(int userId, int id)
        {
            if (!IsAuthenticated(userId))
                return Unauthorized();

            var hasPhoto = await UserHasPhoto(userId, id);
            if (!hasPhoto)
                return Unauthorized();

            var photoFromRepo = await _photoRepo.GetById(id);
            if (photoFromRepo.IsMain)
                return BadRequest("You cant delete the main photo!");

            if (photoFromRepo.PublicId != null)
            {
                var deletionParams = new DeletionParams(photoFromRepo.PublicId);
                var result = _cloudinary.Destroy(deletionParams);

                if (result.Result == "ok")
                    _photoRepo.Delete(photoFromRepo);
            }
            else
                _photoRepo.Delete(photoFromRepo);
                
            if (await _photoRepo.SaveAll())
                return Ok();

            return BadRequest("Can not delete the photo.");

        }

        public async Task<bool> UserHasPhoto(int userId, int id)
        {
            var userFromRepo = await _userRepo.GetById(userId);
            if (!userFromRepo.Photos.Any(p => p.Id == id))
                return false;
            return true;
        }

    }
}