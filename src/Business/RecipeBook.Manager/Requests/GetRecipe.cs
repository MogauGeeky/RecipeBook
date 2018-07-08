using MediatR;
using RecipeBook.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook.Manager.Requests
{
    public class GetRecipe: IRequest<RecipeEntry>
    {
        public string Id { get; set; }
    }
}
