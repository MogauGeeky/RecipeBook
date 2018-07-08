using Newtonsoft.Json;

namespace RecipeBook.Models
{
    public class RecipeEntryStep
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public int Sequence { get; set; }
        public string Notes { get; set; }
    }
}
