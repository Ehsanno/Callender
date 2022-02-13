using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Callender.Date.Repo;

namespace Callender.Middleware
{
    public class UsersManagerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ICallenderRepo _callenderRepo;
        public UsersManagerMiddleware(IServiceScopeFactory serviceScopeFactory, RequestDelegate next, ICallenderRepo callenderRepo)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _next = next;
            _callenderRepo = callenderRepo;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                var username = httpContext.User.FindFirstValue(ClaimTypes.Name);
                var userid = httpContext.User.FindFirstValue(ClaimTypes.SerialNumber);
                var roles = httpContext.User.FindAll(ClaimTypes.Role);
                using var scope = _serviceScopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<ICallenderRepo>();
                string token = httpContext.Request.Headers["Authorization"];
                var check = await _callenderRepo.GetTokenByToken(token);
                if (check == false)
                {
                    httpContext.Response.StatusCode = 403;
                    return;
                }
                else
                {
                    if (roles.Any(x => x.Value == "Admin"))
                        if (!await repo.CheckIsAdmin(username))
                        {

                            httpContext.Response.StatusCode = 403;
                            return;
                        }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            await _next(httpContext);
        }

    }
    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder AuthMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<UsersManagerMiddleware>();
        }
    }
}
