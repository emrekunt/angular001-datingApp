using System.Security.Claims;
using DatingApp.API.Data.Interfaces;
using DatingApp.API.DTO;
using Microsoft.AspNetCore.Mvc;

namespace DatingApp.API.Controllers
{
    public class BaseApiController : ControllerBase
    {
        public bool IsAuthenticated(int userId)
        {
            if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
                return false;
            return true;
        }
    }
}