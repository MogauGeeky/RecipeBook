using System;

namespace RecipeBook.Manager.Exceptions
{
    public class RestrictedUpdateException: Exception
    {
        public RestrictedUpdateException(string message): base(message) { }
    }
}
