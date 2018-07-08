using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook.Models
{
    public class RecipeEntry
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }

        public RecipeEntryStep[] RecipeEntrySteps { get; set; }
    }
}
