using MediatR;

namespace RecipeBook.Manager.Requests
{
    public class DeleteRecipe: IRequest
    {
        public DeleteRecipe(string id)
        {
            Id = id;
        }

        public string Id { get; set; }
    }
}
