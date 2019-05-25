using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesDAW.Models
{
    public class Ingredient
    {
        public Guid Id { get; set; }

        public string amount { get; set; }
        public string name { get; set; }

        public virtual Recipe Recipe { get; set; }
    }
}
