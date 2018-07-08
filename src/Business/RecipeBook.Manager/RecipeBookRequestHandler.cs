using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using RecipeBook.Data.Manager;
using RecipeBook.Manager.Requests;
using RecipeBook.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RecipeBook.Manager
{
    public class RecipeBookRequestHandler :
        IRequestHandler<AddRecipe, RecipeEntry>,
        IRequestHandler<UpdateRecipe, RecipeEntry>,
        IRequestHandler<GetRecipes, IEnumerable<RecipeEntry>>,
        IRequestHandler<GetRecipe, RecipeEntry>
    {
        private readonly IRecipeBookDataManager _recipeBookDataManager;
        private readonly ILogger<RecipeBookRequestHandler> _logger;
        private readonly IMapper _mapper;

        public RecipeBookRequestHandler(
            IRecipeBookDataManager recipeBookDataManager, 
            ILogger<RecipeBookRequestHandler> logger,
            IMapper mapper)
        {
            _recipeBookDataManager = recipeBookDataManager;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<RecipeEntry> Handle(AddRecipe request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Adding a new recipe", request);
            var recipeEntry = _mapper.Map<RecipeEntry>(request);

            var newEntryId = await _recipeBookDataManager.Recipes.CreateItemAsync(recipeEntry);
            recipeEntry.Id = newEntryId;

            return recipeEntry;
        }

        public async Task<RecipeEntry> Handle(UpdateRecipe request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating recipe", request);
            var recipeEntry = _mapper.Map<RecipeEntry>(request);

            await _recipeBookDataManager.Recipes.UpdateItemAsync(recipeEntry.Id, recipeEntry);

            return recipeEntry;
        }

        public async Task<IEnumerable<RecipeEntry>> Handle(GetRecipes request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving recipes", request);

            return await _recipeBookDataManager.Recipes.GetItemsAsync(c => c.Id != null);
        }

        public async Task<RecipeEntry> Handle(GetRecipe request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Retrieving recipe", request);

            return await _recipeBookDataManager.Recipes.GetItemAsync(request.Id);
        }
    }
}
