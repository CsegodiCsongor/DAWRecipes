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

        [HttpGet]
        public IEnumerable<Recipe> ListRecipes()
        {
            Recipe[] cRecipe=_context.Recipes.ToArray();
            return cRecipe;
        }

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
            List<Instruction> inst = _context.Instructions.Where(i => i.Recipe.Id == cRecipe.Id).ToList();

            cRecipe.Ingredients = ing;
            cRecipe.Instructions = inst;

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
                    Id = ingr.Id,
                    name = ingr.name,
                    amount = ingr.amount
                });
            }

            List<SInstruction> Sintsr = new List<SInstruction>();
            foreach(Instruction instr in inst)
            {
                Sintsr.Add(new SInstruction()
                {
                    Id=instr.Id,
                    instruction = instr.instruction
                });
            }
            cRecipe.Ingredients = ing;
            cRecipe.Instructions = inst;

           cR.Ingredients = SI;
           cR.Instructions = Sintsr;

            return cR;
        }


        public void SaveRecipe([FromBody]SRecipe recipe)
        {
            if (recipe.Id == Guid.Empty)
            {
                recipe.Id = Guid.NewGuid();
                Recipe r = new Recipe
                {
                    Id = recipe.Id,
                    Title = recipe.Title,
                    Description = recipe.Description,
                    Serves = recipe.Serves,
                    ImageUrl = recipe.ImageUrl
                };

                List<Ingredient> ing = new List<Ingredient>();
                for(int i=0;i<recipe.Ingredients.Count;i++)
                {
                    ing.Add(new Ingredient()
                    {
                        name = recipe.Ingredients[i].name,
                        amount = recipe.Ingredients[i].amount
                    });
                }

                List<Instruction> instr = new List<Instruction>();
                for (int i = 0; i < recipe.Instructions.Count; i++)
                {
                    instr.Add(new Instruction()
                    {
                        instruction = recipe.Instructions[i].instruction
                    });
                }

                r.Ingredients = ing;
                r.Instructions = instr;
                _context.Recipes.Add(r);
            }
            else
            {
                Recipe recipe1 = _context.Recipes.FirstOrDefault(Recipe => Recipe.Id == recipe.Id);
                recipe1.Title = recipe.Title;
                recipe1.Serves = recipe.Serves;
                recipe1.ImageUrl = recipe.ImageUrl;
                recipe1.Description = recipe.Description;

               UpdateIng(recipe.Ingredients, recipe.Id);
               UpdateInstr(recipe.Instructions, recipe.Id);        
            }

            _context.SaveChanges();
        }


        public void UpdateIng(List<SIngredient> ingredients, Guid id)
        {
            List<Ingredient> ing = _context.Ingredients.Where(i => i.Recipe.Id == id).ToList();

            for (int i = 0; i < ing.Count; i++)
            {
                bool found = false;

                for (int j = 0; j < ingredients.Count; j++)
                {
                    if (ing[i].Id == ingredients[j].Id && ing[i].Id!=null)
                    {
                        found = true;
                        ing[i].name = ingredients[j].name;
                        ing[i].amount = ingredients[j].amount;
                        ingredients.RemoveAt(j);
                        j--;
                        break;
                    }
                }

                if (!found)
                {
                    _context.Ingredients.Remove(ing[i]);
                    ing.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < ingredients.Count; i++)
            {
                Ingredient ingr = new Ingredient()
                {
                    Id = new Guid(),
                    name =ingredients[i].name,
                    amount=ingredients[i].amount,
                };
                ingr.Recipe = new Recipe()
                {
                    Id = id
                };

                _context.Ingredients.Add(ingr);
            }
        }


        public void UpdateInstr(List<SInstruction> instructions, Guid id)
        {
            List<Instruction> instr = _context.Instructions.Where(i => i.Recipe.Id == id).ToList();

            for (int i = 0; i < instr.Count; i++)
            {
                bool found = false;

                for (int j = 0; j < instructions.Count; j++)
                {
                    if (instr[i].Id == instructions[j].Id && instr[i].Id != null)
                    {
                        found = true;
                        instr[i].instruction = instructions[j].instruction;
                        instructions.RemoveAt(j);
                        j--;
                        break;
                    }
                }

                if (!found)
                {
                    _context.Instructions.Remove(instr[i]);
                    instr.RemoveAt(i);
                    i--;
                }
            }

            for (int i = 0; i < instructions.Count; i++)
            {
                Instruction inst = new Instruction()
                {
                    Id = new Guid(),
                    instruction = instructions[i].instruction,
                };
                inst.Recipe = new Recipe()
                {
                    Id = id
                };

                _context.Instructions.Add(inst);
            }
        }

        public void DeleteRecipe([FromQuery]Guid RecipeId)
        {
            Recipe dbRecipe = _context.Recipes.FirstOrDefault(db => db.Id == RecipeId);

            List<Ingredient> auxing= _context.Ingredients.Where(ing => ing.Recipe.Id == RecipeId).ToList();
            foreach(Ingredient ing in auxing)
            {
                _context.Ingredients.Remove(ing);
            }

            List<Instruction> auxinstr = _context.Instructions.Where(instr => instr.Recipe.Id == RecipeId).ToList();
            foreach (Instruction instr in auxinstr)
            {
                _context.Instructions.Remove(instr);
            }

            _context.Recipes.Remove(dbRecipe);

            _context.SaveChanges();
        }

        private bool RecipeExists(Guid id)
        {
            return _context.Recipes.Any(e => e.Id == id);
        }
    }
}
