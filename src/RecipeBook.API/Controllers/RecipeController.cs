using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RecipeBook.Manager.Requests;
using RecipeBook.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace RecipeBook.API.Controllers
{
    [Authorize]
    [Route("api/recipes")]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private readonly IMediator _mediator;

        public RecipeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Recipe
        [AllowAnonymous]
        [HttpGet]
        public async Task<IEnumerable<RecipeEntry>> Get()
        {
            return await _mediator.Send(new GetRecipes());
        }

        // GET: api/Recipe/5
        [AllowAnonymous]
        [HttpGet("{id}", Name = "Get")]
        public async Task<RecipeEntry> Get(string id)
        {
            return await _mediator.Send(new GetRecipe { Id = id });
        }

        // POST: api/Recipe
        [HttpPost]
        public async Task<RecipeEntry> Post([FromBody]AddRecipe value)
        {
            return await _mediator.Send(value);
        }

        // PUT: api/Recipe/5
        [HttpPut("{id}")]
        public async Task<RecipeEntry> Put(string id, [FromBody] UpdateRecipe value)
        {
            return await _mediator.Send(value);
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task Delete(string id)
        {
            await _mediator.Send(new DeleteRecipe(id));
        }

        [HttpPost("{id}/steps")]
        public async Task<RecipeEntryStep> AddStep(string id, [FromBody]AddRecipeStep value)
        {
            value.RecipeId = id;

            return await _mediator.Send(value);
        }

        [HttpPut("{id}/steps/{stepId}")]
        public async Task<RecipeEntryStep> UpdateStep(string id, string stepId, [FromBody]UpdateRecipeStep value)
        {
            value.Id = stepId;
            value.RecipeId = id;

            return await _mediator.Send(value);
        }

        [HttpDelete("{id}/steps/{stepId}")]
        public async Task DeleteStep(string id, string stepId)
        {
            await _mediator.Send(new DeleteRecipeStep(stepId, id));
        }
    }
}
