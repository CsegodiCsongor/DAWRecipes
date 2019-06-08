using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RecipesDAW.Models
{
    public class SIngredient
    {
        public Guid Id { get; set; }
        public string amount { get; set; }
        public string name { get; set; }
    }
}
