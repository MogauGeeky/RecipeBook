using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace RecipeBook.Data
{
    public interface IRepository<T> where T: class
    {
        Task<T> GetItemAsync(string id);

        Task<IEnumerable<T>> GetItemsAsync(Expression<Func<T, bool>> predicate);

        Task<object> CreateItemAsync(T item);

        Task<object> UpdateItemAsync(string id, T item);

        Task DeleteItemAsync(string id);
    }
}
