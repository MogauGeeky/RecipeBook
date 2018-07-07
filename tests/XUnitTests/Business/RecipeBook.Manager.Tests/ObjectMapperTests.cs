using AutoMapper;
using RecipeBook.Manager;
using Xunit;

namespace XUnitTests.Business.RecipeBook.Manager.Tests
{
    public class ObjectMapperTests
    {
        [Fact]
        public void CommandRequestAndModelsShouldMapAccordingly()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<RecipeBookObjectMapper>());

            config.AssertConfigurationIsValid();
        }
    }
}
