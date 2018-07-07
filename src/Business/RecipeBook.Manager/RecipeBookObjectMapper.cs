using AutoMapper;
using RecipeBook.Manager.Requests;
using RecipeBook.Models;

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
        }
    }
}
