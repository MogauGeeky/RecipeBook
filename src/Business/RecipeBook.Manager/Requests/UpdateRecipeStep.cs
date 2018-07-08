using MediatR;
using RecipeBook.Models;

namespace RecipeBook.Manager.Requests
{
    public class UpdateRecipeStep: IRequest<RecipeEntryStep>
    {
        public string Id { get; set; }
        public string RecipeId { get; set; }
        public string Notes { get; set; }
    }
}
