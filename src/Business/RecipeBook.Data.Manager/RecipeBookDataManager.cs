using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Options;
using RecipeBook.Data.CosmosDb;
using RecipeBook.Models;

namespace RecipeBook.Data.Manager
{
    public class RecipeBookDataManager : IRecipeBookDataManager
    {
        private readonly DocumentClient _documentClient;
        private readonly DocumentDbOptions _documentDbOptions;

        public RecipeBookDataManager(IOptions<DocumentDbOptions> documentDbOptions)
        {
            _documentDbOptions = documentDbOptions.Value ?? throw new ArgumentNullException(nameof(documentDbOptions));
            _documentClient = new DocumentClient(new Uri(_documentDbOptions.Endpoint), _documentDbOptions.Key, new ConnectionPolicy { EnableEndpointDiscovery = false });
        }

        public IRepository<RecipeEntry> Recipes => new DocumentDbRepository<RecipeEntry>(_documentClient, _documentDbOptions.DatabaseId);

        public IRepository<RecipeEntryStep> RecipeSteps => new DocumentDbRepository<RecipeEntryStep>(_documentClient, _documentDbOptions.DatabaseId);

        public static void Initialize(DocumentDbOptions documentDbOptions)
        {
            // Connect the client
            DocumentClient client = new DocumentClient(new Uri(documentDbOptions.Endpoint), documentDbOptions.Key, new ConnectionPolicy { EnableEndpointDiscovery = false });

            // Initialize the database
            CreateDatabaseIfNotExistsAsync(client, documentDbOptions.DatabaseId).Wait();

            // create database collections
            CreateCollectionIfNotExistsAsync<RecipeEntry>(client, documentDbOptions.DatabaseId).Wait();
        }

        private static async Task CreateDatabaseIfNotExistsAsync(DocumentClient client, string databaseId)
        {
            try
            {
                await client.ReadDatabaseAsync(UriFactory.CreateDatabaseUri(databaseId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDatabaseAsync(new Database { Id = databaseId });
                }
                else
                {
                    throw;
                }
            }
        }

        private static async Task CreateCollectionIfNotExistsAsync<T>(DocumentClient client, string databaseId)
        {
            var collectionId = typeof(T).FullName;

            try
            {
                await client.ReadDocumentCollectionAsync(UriFactory.CreateDocumentCollectionUri(databaseId, collectionId));
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    await client.CreateDocumentCollectionAsync(
                        UriFactory.CreateDatabaseUri(databaseId),
                        new DocumentCollection { Id = collectionId },
                        new RequestOptions { OfferThroughput = 1000 });
                }
                else
                {
                    throw;
                }
            }
        }
    }
}
