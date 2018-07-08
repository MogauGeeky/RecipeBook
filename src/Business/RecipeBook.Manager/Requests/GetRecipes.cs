using MediatR;
using RecipeBook.Models;
using System.Collections.Generic;

namespace RecipeBook.Manager.Requests
{
    public class GetRecipes: IRequest<IEnumerable<RecipeEntry>>
    {

    }
}
