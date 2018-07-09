using RecipeBook.Models;

namespace RecipeBook.Data.Manager
{
    public interface IRecipeBookDataManager
    {
        IRepository<RecipeEntry> Recipes { get; }
        IRepository<RecipeUser> Users { get; }
    }
}
