namespace RecipeBook.Data.Manager
{
    public interface ICurrentUser
    {
        string UserId { get; }
        string UserName { get; }
    }
}
