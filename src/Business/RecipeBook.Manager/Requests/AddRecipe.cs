using FluentValidation;
using MediatR;
using RecipeBook.Models;

namespace RecipeBook.Manager.Requests
{
    public class AddRecipe: IRequest<RecipeEntry>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
    }

    public class AddRecipeValidator: AbstractValidator<AddRecipe>
    {
        public AddRecipeValidator()
        {
            RuleFor(c => c.Title).MaximumLength(200).NotNull().NotEmpty();
            RuleFor(c => c.Description).MaximumLength(500);
            RuleFor(c => c.Description).MaximumLength(1000);
        }
    }
}