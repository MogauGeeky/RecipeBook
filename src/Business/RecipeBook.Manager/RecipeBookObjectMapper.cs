using AutoMapper;
using RecipeBook.Manager.Requests;
using RecipeBook.Models;
using System;

namespace RecipeBook.Manager
{
    public class RecipeBookObjectMapper: Profile
    {
        public RecipeBookObjectMapper()
        {
            CreateMap<AddRecipe, RecipeEntry>()
                .ForMember(c => c.Id, x => x.Ignore())
                .ForMember(c => c.RecipeEntrySteps, x => x.Ignore());

            CreateMap<UpdateRecipe, RecipeEntry>()
                .ForMember(c => c.RecipeEntrySteps, x => x.Ignore());

            CreateMap<AddRecipeStep, RecipeEntryStep>()
                .ForMember(c => c.Id, x => x.MapFrom(c => Guid.NewGuid().ToString()));
        }
    }
}
