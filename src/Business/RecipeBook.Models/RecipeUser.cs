using Newtonsoft.Json;

namespace RecipeBook.Models
{
    public class RecipeUser
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Username { get; set; }
        public string FullName { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSecret { get; set; }
    }
}
