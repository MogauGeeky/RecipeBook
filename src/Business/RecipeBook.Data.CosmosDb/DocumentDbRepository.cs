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
        private readonly DocumentClient _client;
        private readonly string _collectionId;

        public DocumentDbRepository(DocumentClient client, string databaseId)
        {
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _databaseId = databaseId ?? throw new ArgumentNullException(nameof(databaseId));
            _collectionId = typeof(T).FullName;
        }

        public async Task<string> CreateItemAsync(T item)
        {
            var document = await _client.CreateDocumentAsync(UriFactory.CreateDocumentCollectionUri(_databaseId, _collectionId), item);

            return document.Resource.Id;
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

        public async Task UpdateItemAsync(string id, T item)
        {
            await _client.ReplaceDocumentAsync(UriFactory.CreateDocumentUri(_databaseId, _collectionId, id), item);
        }
    }
}
