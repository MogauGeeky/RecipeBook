using System.Security.Claims;

namespace RecipeBook.Data.Manager
{
    public interface ICurrentUserContext
    {
        void SetClaims(ClaimsIdentity claims);
    }
}
