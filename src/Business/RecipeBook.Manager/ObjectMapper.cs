using AutoMapper;
using RecipeBook.Manager.Requests;
using RecipeBook.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook.Manager
{
    public class ObjectMapper: Profile
    {
        public ObjectMapper()
        {
            CreateMap<AddRecipe, RecipeEntry>();
        }
    }
}
