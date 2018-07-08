using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using RecipeBook.Data.Manager;
using RecipeBook.Manager.Requests;
using RecipeBook.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace RecipeBook.Manager
{
    public class RecipeBookRequestHandler :
        IRequestHandler<AddRecipe, RecipeEntry>,
        IRequestHandler<UpdateRecipe, RecipeEntry>,
        IRequestHandler<DeleteRecipe>,
        IRequestHandler<GetRecipes, IEnumerable<RecipeEntry>>,
        IRequestHandler<GetRecipe, RecipeEntry>,
        IRequestHandler<AddRecipeStep, RecipeEntryStep>,
        IRequestHandler<UpdateRecipeStep, RecipeEntryStep>,
        IRequestHandler<DeleteRecipeStep>
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

        public async Task<Unit> Handle(DeleteRecipe request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting recipe", request);
            await _recipeBookDataManager.Recipes.DeleteItemAsync(request.Id);

            return new Unit();
        }

        public async Task<RecipeEntryStep> Handle(AddRecipeStep request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Adding a new step for recipe", request);

            var recipe = await _recipeBookDataManager.Recipes.GetItemAsync(request.RecipeId);

            if (recipe.RecipeEntrySteps == null)
                recipe.RecipeEntrySteps = new List<RecipeEntryStep>();

            var recipeStep = _mapper.Map<RecipeEntryStep>(request);
            recipe.RecipeEntrySteps.Add(recipeStep);

            await _recipeBookDataManager.Recipes.UpdateItemAsync(recipe.Id, recipe);

            return recipeStep;
        }

        public async Task<RecipeEntryStep> Handle(UpdateRecipeStep request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Updating a step for recipe", request);

            var recipe = await _recipeBookDataManager.Recipes.GetItemAsync(request.RecipeId);

            var update = _mapper.Map<RecipeEntryStep>(request);

            var record = recipe.RecipeEntrySteps.FindIndex(c => c.Id == request.Id);
            recipe.RecipeEntrySteps[record] = update;

            await _recipeBookDataManager.Recipes.UpdateItemAsync(recipe.Id, recipe);

            return update;
        }

        public async Task<Unit> Handle(DeleteRecipeStep request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Deleting a step for recipe", request);

            var recipe = await _recipeBookDataManager.Recipes.GetItemAsync(request.RecipeId);

            recipe.RecipeEntrySteps = recipe.RecipeEntrySteps.Where(c => c.Id != request.Id).ToList();

            await _recipeBookDataManager.Recipes.UpdateItemAsync(recipe.Id, recipe);

            return new Unit();
        }
    }
}
