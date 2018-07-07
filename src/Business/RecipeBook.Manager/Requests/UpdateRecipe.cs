using FluentValidation;
using MediatR;
using RecipeBook.Models;

namespace RecipeBook.Manager.Requests
{
    public class UpdateRecipe : IRequest<RecipeEntry>
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
    }

    public class UpdateRecipeValidator: AbstractValidator<UpdateRecipe>
    {
        public UpdateRecipeValidator()
        {
            RuleFor(c => c.Id).NotNull().NotEmpty();
            RuleFor(c => c.Title).MaximumLength(200).NotNull().NotEmpty();
            RuleFor(c => c.Description).MaximumLength(500);
            RuleFor(c => c.Description).MaximumLength(1000);
        }
    }
}
