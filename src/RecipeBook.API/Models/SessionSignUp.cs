using FluentValidation;

namespace RecipeBook.API.Models
{
    public class SessionSignUp
    {
        public string Fullname { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }

    public class SessionSignUpValidator: AbstractValidator<SessionSignUp>
    {
        public SessionSignUpValidator()
        {
            RuleFor(c => c.Fullname).NotNull().NotEmpty().MaximumLength(100);
            RuleFor(c => c.Username).NotNull().NotEmpty().MaximumLength(60);
            RuleFor(c => c.Password).NotNull().NotEmpty().MaximumLength(60);
            RuleFor(c => c.ConfirmPassword).Equal(c => c.Password).WithMessage("Passwords do not match");
        }
    }
}
