using MediatR;

namespace RecipeBook.Manager.Requests
{
    public class DeleteRecipeStep: IRequest
    {
        public DeleteRecipeStep(string id, string recipeId)
        {
            Id = id;
            RecipeId = recipeId;
        }

        public string Id { get; set; }
        public string RecipeId { get; set; }
    }
}
