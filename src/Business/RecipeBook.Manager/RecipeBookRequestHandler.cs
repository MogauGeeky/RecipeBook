using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using RecipeBook.Data.Manager;
using RecipeBook.Manager.Requests;
using RecipeBook.Models;
using System.Threading;
using System.Threading.Tasks;

namespace RecipeBook.Manager
{
    public class RecipeBookRequestHandler : IRequestHandler<AddRecipe, RecipeEntry>
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

            return (RecipeEntry)await _recipeBookDataManager.Recipes.CreateItemAsync(recipeEntry);
        }
    }
}
