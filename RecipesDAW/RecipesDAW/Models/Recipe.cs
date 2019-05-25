using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesDAW.Models
{
    public class Recipe
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Serves { get; set; }
        public string ImageUrl { get; set; }

        public virtual List<Ingredient> Ingredients { get; set; }
        public virtual List<Instruction> Instructions { get; set; }
           
    }
}
