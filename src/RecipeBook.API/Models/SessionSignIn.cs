using FluentValidation;

namespace RecipeBook.API.Models
{
    public class SessionSignIn
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }

    public class SessionSignInValidator: AbstractValidator<SessionSignIn>
    {
        public SessionSignInValidator()
        {
            RuleFor(c => c.Username).NotNull().NotEmpty();
            RuleFor(c => c.Password).NotNull().NotEmpty();
        }
    }
}
