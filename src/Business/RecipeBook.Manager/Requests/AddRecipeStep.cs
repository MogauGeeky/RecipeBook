﻿using MediatR;
using RecipeBook.Models;

namespace RecipeBook.Manager.Requests
{
    public class AddRecipeStep: IRequest<RecipeEntryStep>
    {
        public string RecipeId { get; set; }
        public string Notes { get; set; }
    }
}
