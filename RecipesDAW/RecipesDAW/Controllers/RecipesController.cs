using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using RecipesDAW.Models;

namespace RecipesDAW.Controllers
{
    [Route("api/[controller]/[action]")]
    public class RecipesController : Controller
    {
        private readonly RecipesDAWContext _context;

        public RecipesController(RecipesDAWContext context)
        {
            _context = context;
        }

        // GET: api/Recipes
        //[HttpGet]
        //public async Task<ActionResult<IEnumerable<Recipe>>> ListRecipes()
        //{
        //    return await _context.Recipe.ToListAsync();
        //}

        [HttpGet]
        public IEnumerable<Recipe> ListRecipes()
        {
            Recipe[] cRecipe=_context.Recipes.ToArray();
            return cRecipe;
        }

        // GET: api/Recipes/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Recipe>> DetailRecipe(Guid id)
        //{
        //    var recipe = await _context.Recipe.FindAsync(id);

        //    if (recipe == null)
        //    {
        //        return NotFound();
        //    }

        //    return recipe;
        //}

        [HttpGet("{recipeID?}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public SRecipe DetailRecipe([FromQuery]Guid? recipeID)
        {
            if(recipeID==null)
            {
                return new SRecipe();
            }

            Recipe cRecipe=_context.Recipes.FirstOrDefault(db => db.Id == recipeID);

            List<Ingredient> ing = _context.Ingredients.Where(i => i.Recipe.Id == cRecipe.Id).ToList();
            List<Instruction> inst = _context.instructions.Where(i => i.Recipe.Id == cRecipe.Id).ToList();


            SRecipe cR = new SRecipe()
            {
                Title = cRecipe.Title,
                Description = cRecipe.Description,
                Serves = cRecipe.Serves,
                ImageUrl = cRecipe.ImageUrl,
                Id = cRecipe.Id
            };

            List<SIngredient> SI = new List<SIngredient>();
            foreach(Ingredient ingr in ing)
            {
                SI.Add(new SIngredient()
                {
                    name = ingr.name,
                    amount = ingr.amount
                });
            }

            List<SInstruction> Sintsr = new List<SInstruction>();
            foreach(Instruction instr in inst)
            {
                Sintsr.Add(new SInstruction()
                {
                    instruction = instr.instruction
                });
            }
            cRecipe.Ingredients = ing;
            cRecipe.Instructions = inst;

           cR.Ingredients = SI;
           cR.Instructions = Sintsr;

            return cR;
        }

        // POST: api/Recipes
        //[HttpPost]
        //public async Task<ActionResult<Recipe>> SaveRecipe(Recipe recipe)
        //{
        //    _context.Recipe.Add(recipe);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetRecipe", new { id = recipe.Id }, recipe);
        //}

        public void SaveRecipe([FromBody]Recipe recipe)
        {
            if (recipe.Id == Guid.Empty)
            {
                recipe.Id = Guid.NewGuid();

                _context.Recipes.Add(recipe);
            }
            else
            {
                Recipe recipe1 = _context.Recipes.FirstOrDefault(Recipe => Recipe.Id == recipe.Id);
                recipe1.Title = recipe.Title;
                recipe1.Serves = recipe.Serves;
                recipe1.ImageUrl = recipe.ImageUrl;
                recipe1.Description = recipe.Description;

                List<Ingredient> ing = _context.Ingredients.Where(i => i.Recipe.Id == recipe.Id).ToList();
                List<Instruction> inst = _context.instructions.Where(i => i.Recipe.Id == recipe.Id).ToList();
                ing.Clear();
                inst.Clear();

                recipe1.Ingredients = recipe.Ingredients;
                recipe1.Instructions = recipe.Instructions;
            }

            _context.SaveChanges();
        }


        // DELETE: api/Recipes/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<Recipe>> DeleteRecipe(Guid id)
        //{
        //    var recipe = await _context.Recipes.FindAsync(id);
        //    if (recipe == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Recipes.Remove(recipe);
        //    await _context.SaveChangesAsync();

        //    return recipe;
        //}

        public void DeleteRecipe([FromQuery]Guid RecipeId)
        {
            Recipe dbRecipe = _context.Recipes.FirstOrDefault(db => db.Id == RecipeId);

            _context.Recipes.Remove(dbRecipe);

            List<Ingredient> auxing= _context.Ingredients.Where(ing => ing.Recipe.Id == RecipeId).ToList();
            foreach(Ingredient ing in auxing)
            {
                _context.Ingredients.Remove(ing);
            }

            List<Instruction> auxinstr = _context.instructions.Where(instr => instr.Recipe.Id == RecipeId).ToList();
            foreach (Instruction instr in auxinstr)
            {
                _context.instructions.Remove(instr);
            }

            _context.SaveChanges();
        }

        private bool RecipeExists(Guid id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
}
