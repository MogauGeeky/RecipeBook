using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook.Models
{
    public class RecipeEntry
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Notes { get; set; }

        public virtual IEnumerable<RecipeEntryStep> RecipeEntrySteps { get; set; }
    }
}
