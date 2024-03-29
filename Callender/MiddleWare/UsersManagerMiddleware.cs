﻿
using Callender.Date.Repo;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
namespace Callender.Middleware
{
    public class UsersManagerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public UsersManagerMiddleware(IServiceScopeFactory serviceScopeFactory, RequestDelegate next)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _next = next;
        }
        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                var email = httpContext.User.FindFirstValue(ClaimTypes.Email);
                var username = httpContext.User.FindFirstValue(ClaimTypes.Name);
                var userid = httpContext.User.FindFirstValue(ClaimTypes.SerialNumber);
                var roles = httpContext.User.FindAll(ClaimTypes.Role);
                using var scope = _serviceScopeFactory.CreateScope();
                var repo = scope.ServiceProvider.GetRequiredService<ICallenderRepo>();
                string token = httpContext.Request.Headers["Authorization"];
                var join = await repo.GetTokenByToken(token);
                if (join == false)
                {
                    httpContext.Response.StatusCode = 403;
                    return;
                }
                else
                {
                    if (roles.Any(x => x.Value == "Admin"))
                        if (!await repo.CheckIsAdmin(email))
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
