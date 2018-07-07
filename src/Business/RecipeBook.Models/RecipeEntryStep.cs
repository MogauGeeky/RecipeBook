using System;
using System.Collections.Generic;
using System.Text;

namespace RecipeBook.Models
{
    public class RecipeEntryStep
    {
        public string Id { get; set; }
        public int Sequence { get; set; }
        public string RecipeEntryId { get; set; }
        public string Notes { get; set; }

        public virtual RecipeEntry RecipeEntry { get; set; }
    }
}
