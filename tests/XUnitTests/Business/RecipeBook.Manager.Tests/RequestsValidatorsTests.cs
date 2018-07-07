using FluentValidation.TestHelper;
using RecipeBook.Manager.Requests;
using Xunit;

namespace XUnitTests.Business.RecipeBook.Manager.Tests
{
    public class RequestsValidatorsTests
    {
        [Fact]
        public void AddRecipeShouldBeValid()
        {
            // Should be valid
            var test1 = new AddRecipe
            {
                Title = "test",
                Description = "test",
                Notes = "test"
            };

            var test1Validator = new AddRecipeValidator();
            Assert.True(test1Validator.Validate(test1).IsValid);

            // Should require title
            var test2 = new AddRecipe
            {
                Description = "test",
                Notes = "test"
            };

            var test2Validator = new AddRecipeValidator();
            test2Validator.ShouldHaveValidationErrorFor(c => c.Title, test2);

            var longTitle = "test";
            while(longTitle.Length <= 100)
            {
                longTitle += $"{longTitle}{longTitle}{longTitle}{longTitle}";
            }

            // title should be 100 in length
            var test3 = new AddRecipe
            {
                Description = longTitle,
                Notes = "test"
            };

            var test3Validator = new AddRecipeValidator();
            test3Validator.ShouldHaveValidationErrorFor(c => c.Title, test2);
        }

        [Fact]
        public void UpdateRecipeShouldBeValid()
        {
            // Should be valid
            var test1 = new UpdateRecipe
            {
                Id = "test",
                Title = "test",
                Description = "test",
                Notes = "test"
            };

            var test1Validator = new UpdateRecipeValidator();
            Assert.True(test1Validator.Validate(test1).IsValid);

            // Should require title
            var test2 = new UpdateRecipe
            {
                Id = "test",
                Description = "test",
                Notes = "test"
            };

            var test2Validator = new UpdateRecipeValidator();
            test2Validator.ShouldHaveValidationErrorFor(c => c.Title, test2);

            var longTitle = "test";
            while (longTitle.Length <= 100)
            {
                longTitle += $"{longTitle}{longTitle}{longTitle}{longTitle}";
            }

            // title should be 100 in length
            var test3 = new UpdateRecipe
            {
                Id = "test",
                Description = longTitle,
                Notes = "test"
            };

            var test3Validator = new UpdateRecipeValidator();
            test3Validator.ShouldHaveValidationErrorFor(c => c.Title, test2);
        }
    }
}
