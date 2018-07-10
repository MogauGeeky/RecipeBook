using System;

namespace RecipeBook.Manager.Exceptions
{
    public class MissingRecordException: Exception
    {
        public MissingRecordException(string message) : base(message) { }
    }
}
