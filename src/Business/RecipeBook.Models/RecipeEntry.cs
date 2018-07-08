using Newtonsoft.Json;
using System.Collections.Generic;

namespace RecipeBook.Models
{
    public class RecipeEntry
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }
        public List<RecipeEntryStep> RecipeEntrySteps { get; set; }
    }
}
