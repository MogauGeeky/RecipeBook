using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RecipeBook.Data.CosmosDb
{
    public class DocumentDbRepository<T> : IRepository<T> where T : class
    {
        private readonly string _databaseId;
        private readonly string _collectionId = nameof(T).ToUpper();

        private DocumentClient _client;

        public DocumentDbRepository(DocumentDbOptions documentDbOptions)
        {
            _databaseId = documentDbOptions.DatabaseId;

            _client = new DocumentClient(new Uri(documentDbOptions.Endpoint), documentDbOptions.Key, new ConnectionPolicy { EnableEndpointDiscovery = false });
        }

        public async Task<object> CreateItemAsync(T item)
        {
            return await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId), item);
        }

        public async Task DeleteItemAsync(string id)
        {
            await _client.DeleteDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id));
        }

        public async Task<T> GetItemAsync(string id)
        {
            try
            {
                Document document = await _client.ReadDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id));
                return (T)(dynamic)document;
            }
            catch (DocumentClientException e)
            {
                if (e.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate)
        {
            IDocumentQuery<T> query = _client.CreateDocumentQuery<T>(
                UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId),
                new FeedOptions { MaxItemCount = -1 })
                .Where(predicate)
                .AsDocumentQuery();

            List<T> results = new List<T>();
            while (query.HasMoreResults)
            {
                results.AddRange(await query.ExecuteNextAsync<T>());
            }

            return results;
        }

        public async Task<object> UpdateItemAsync(string id, T item)
        {
            return await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id), item);
        }
    }
}
