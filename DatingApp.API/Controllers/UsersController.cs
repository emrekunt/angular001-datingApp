using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using DatingApp.API.Data;
using DatingApp.API.Data.Interfaces;
using DatingApp.API.DTO.User;
using DatingApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DatingApp.API.Controllers
{
    [ServiceFilter(typeof(LogUserActivity))]
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : BaseApiController
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UsersController(IUserRepository repo, IMapper mapper)
        {
            _mapper = mapper;
            _repo = repo;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _repo.GetAll();
            var retVal = _mapper.Map<IEnumerable<UserForListDTO>>(users); 
            return Ok(retVal);
        }

        [HttpGet("{id}", Name= "GetUser")]
        public async Task<IActionResult> GetUser(int id)
        {
            var user = await _repo.GetById(id);
            var retVal = _mapper.Map<UserForListDetailedDTO>(user);
            return Ok(retVal);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, UserForUpdateDTO userForUpdateDTO)
        {
            if (IsAuthenticated(id))
                return Unauthorized();

            var userFromRepo = await _repo.GetById(id);
            _mapper.Map(userForUpdateDTO, userFromRepo);

            if(await _repo.SaveAll())
                return NoContent();
            
            throw new Exception($"Updatind user {id} failed");
        }

    }
}
