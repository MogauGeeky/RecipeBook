using MediatR;
using RecipeBook.Models;

namespace RecipeBook.Manager.Requests
{
    public class GetRecipeStep: IRequest<RecipeEntryStep>
    {
        public string RecipeId { get; }
        public string RecipeStepId { get; }

        public GetRecipeStep(string recipeId, string recipeStepId)
        {
            RecipeId = recipeId;
            RecipeStepId = recipeStepId;
        }
    }
}
