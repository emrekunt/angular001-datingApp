using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.API.Data.Interfaces;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace DatingApp.API.Helpers
{
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContent = await next();
            var userId = int.Parse(resultContent.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            var repo = resultContent.HttpContext.RequestServices.GetService<IUserRepository>();

            var user = await repo.GetById(userId);
            user.LastActive = DateTime.Now;
            await repo.SaveAll();

        }
    }
}