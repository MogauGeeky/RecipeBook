using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Polly;
using RecipeBook.Data.CosmosDb;
using RecipeBook.Data.Manager;
using System;

namespace RecipeBook.API
{
    public static class DatabaseMigrator
    {
        public static IWebHost MigrateCosmosDb(this IWebHost webHost)
        {
            using (var scope = webHost.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var logger = services.GetRequiredService<ILogger<RecipeBookDataManager>>();
                var options = services.GetService<IOptions<DocumentDbOptions>>();

                try
                {
                    logger.LogInformation($"Migrating cosmos database");

                    var retry = Policy.Handle<DocumentClientException>()
                        .WaitAndRetry(new TimeSpan[]
                        {
                            TimeSpan.FromSeconds(5),
                            TimeSpan.FromSeconds(10),
                            TimeSpan.FromSeconds(15)
                        }, onRetry: (exception, retryCount, contextData) =>
                        {
                            logger.LogInformation($"Retry Migrating cosmos database. {retryCount}");
                        });

                    retry.Execute(() =>
                    {
                        RecipeBookDataManager.Initialize(options.Value);
                    });

                    logger.LogInformation($"Migrated database");
                }
                catch (Exception ex)
                {
                    logger.LogError(ex, $"An error occurred while migrating the database");
                }
            }

            return webHost;
        }
    }
}
