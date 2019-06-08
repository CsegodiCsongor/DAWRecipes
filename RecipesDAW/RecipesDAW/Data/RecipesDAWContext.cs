using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace RecipesDAW.Models
{
    public class RecipesDAWContext : IdentityDbContext
    {
        public RecipesDAWContext (DbContextOptions<RecipesDAWContext> options)
            : base(options)
        {
        }

        public DbSet<RecipesDAW.Models.Recipe> Recipes { get; set; }
        public DbSet<RecipesDAW.Models.Ingredient> Ingredients { get; set; }
        public DbSet<RecipesDAW.Models.Instruction> Instructions { get; set; }
    }
}
