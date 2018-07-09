using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RecipeBook.Data.Manager;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace RecipeBook.API.Middlewares
{
    public class CurrentUserMiddleware
    {
        private readonly RequestDelegate _next;

        public CurrentUserMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (context.User != null && context.User.Identity.IsAuthenticated)
            {
                // get current claim identity
                var claimsIdentity = context.User.Identity as ClaimsIdentity;

                var securityContextInstance = context.RequestServices.GetService<ICurrentUser>();
                if (!(securityContextInstance is ICurrentUserContext securityContextPrincipal))
                    throw new Exception("Invalid authentication, building security context failed");

                securityContextPrincipal.SetClaims(claimsIdentity);
            }

            // Call the next delegate/middleware in the pipeline
            await _next.Invoke(context);
        }
    }
}
