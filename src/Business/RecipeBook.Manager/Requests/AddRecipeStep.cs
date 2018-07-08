using MediatR;
using RecipeBook.Models;

namespace RecipeBook.Manager.Requests
{
    public class AddRecipeStep: IRequest<RecipeEntryStep>
    {
        public string RecipeStepId { get; set; }
        public string Notes { get; set; }
    }
}
