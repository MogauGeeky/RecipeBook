using System;
using System.Security.Claims;

namespace RecipeBook.Data.Manager
{
    public class CurrentUserProvider: ICurrentUser, ICurrentUserContext
    {
        private ClaimsIdentity Claims { get; set; }

        public string UserId => FindFirstValue(ClaimTypes.Sid);

        public string UserName => FindFirstValue(ClaimTypes.Name);

        public void SetClaims(ClaimsIdentity claims)
        {
            Claims = claims;
        }

        private string FindFirstValue(string claimType)
        {
            if (Claims == null)
                throw new Exception("Invalid security");

            var claim = Claims.FindFirst(claimType);
            if (claim == null)
                throw new Exception("Invalid security");

            return claim.Value;
        }
    }
}
