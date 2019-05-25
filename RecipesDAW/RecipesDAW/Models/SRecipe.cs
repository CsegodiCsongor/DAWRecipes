using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesDAW.Models
{
    public class SRecipe
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Serves { get; set; }
        public string ImageUrl { get; set; }

        public List<SIngredient> Ingredients { get; set; }
        public List<SInstruction> Instructions { get; set; }
    }
}
