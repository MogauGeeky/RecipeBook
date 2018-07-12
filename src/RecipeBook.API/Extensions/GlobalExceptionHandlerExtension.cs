using GlobalExceptionHandler.WebApi;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using RecipeBook.Manager.Exceptions;

namespace RecipeBook.API.Extensions
{
    public static class GlobalExceptionHandlerExtension
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler("/error").WithConventions(x =>
            {
                x.ContentType = "application/json";
                x.MessageFormatter(s => JsonConvert.SerializeObject(new
                {
                    Message = "An error occurred whilst processing your request"
                }));

                x.ForException<MissingRecordException>().ReturnStatusCode(StatusCodes.Status404NotFound)
                    .UsingMessageFormatter((ex, context) => JsonConvert.SerializeObject(new
                    {
                        ex.Message
                    }));

                x.ForException<RestrictedUpdateException>().ReturnStatusCode(StatusCodes.Status403Forbidden)
                    .UsingMessageFormatter((ex, context) => JsonConvert.SerializeObject(new
                    {
                        error = ex.Message
                    }));
            });

            return app;
        }
    }
}
