using AutoMapper;
using AutoMoqCore;
using Moq;
using RecipeBook.Data.Manager;
using RecipeBook.Manager;
using RecipeBook.Manager.Requests;
using RecipeBook.Models;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace XUnitTests.Business.RecipeBook.Manager.Tests
{
    public class RecipeBookRequestHandlerTests
    {
        AutoMoqer autoMoqer;
        RecipeBookRequestHandler recipeBookRequestHandler;
        CancellationToken cancellationToken;

        public RecipeBookRequestHandlerTests()
        {
            autoMoqer = new AutoMoqer();
            cancellationToken = new CancellationToken();
            var mapper = new MapperConfiguration(c => c.AddProfile<RecipeBookObjectMapper>()).CreateMapper();
            autoMoqer.SetInstance(mapper);

            recipeBookRequestHandler = autoMoqer.Create<RecipeBookRequestHandler>();
        }

        [Fact]
        public void AddRecipeShouldAddNewRecipe()
        {
            // Arrange

            var addNew = new AddRecipe
            {
                Title = "test"
            };

            autoMoqer.GetMock<IRecipeBookDataManager>().Setup(c => c.Recipes.CreateItemAsync(It.IsAny<RecipeEntry>())).Returns(Task.FromResult((object)new RecipeEntry
            {
                Id = "1",
                Title = "test"
            }));

            // Act
            var response = recipeBookRequestHandler.Handle(addNew, cancellationToken).Result;

            // Assert
            Assert.NotNull(response);
            Assert.IsType<RecipeEntry>(response);
            Assert.True(response.Title == addNew.Title);
            autoMoqer.GetMock<IRecipeBookDataManager>().Verify(c => c.Recipes.CreateItemAsync(It.IsAny<RecipeEntry>()), Times.Once);
        }

        [Fact]
        public void UpdateRecipeShouldUpdateTheRecipe()
        {
            // Arrange

            var updateRecipe = new UpdateRecipe
            {
                Id = "1",
                Title = "test"
            };

            autoMoqer.GetMock<IRecipeBookDataManager>().Setup(c => c.Recipes.UpdateItemAsync(It.IsAny<string>(), It.IsAny<RecipeEntry>())).Returns(Task.FromResult((object)new RecipeEntry
            {
                Id = "1",
                Title = "test"
            }));

            // Act
            var response = recipeBookRequestHandler.Handle(updateRecipe, cancellationToken).Result;

            // Assert
            Assert.NotNull(response);
            Assert.IsType<RecipeEntry>(response);
            Assert.True(response.Title == updateRecipe.Title);
            autoMoqer.GetMock<IRecipeBookDataManager>().Verify(c => c.Recipes.UpdateItemAsync(It.Is<string>(x => x == updateRecipe.Id), It.IsAny<RecipeEntry>()), Times.Once);
        }
    }
}
