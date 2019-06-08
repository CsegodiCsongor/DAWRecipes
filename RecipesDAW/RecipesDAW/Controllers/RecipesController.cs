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
        private readonly RecipesDAWContext recipesDAWContext;

        public RecipesController(RecipesDAWContext context)
        {
            recipesDAWContext = context;
        }

        [HttpGet]
        public IEnumerable<Recipe> ListRecipes()
        {
            Recipe[] cRecipe = recipesDAWContext.Recipes.ToArray();
            return cRecipe;
        }

        [HttpGet("{recipeID?}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public SRecipe DetailRecipe([FromQuery]Guid? recipeID)
        {
            if (recipeID == null)
            {
                return new SRecipe();
            }

            Recipe auxRecipe = recipesDAWContext.Recipes.FirstOrDefault(db => db.Id == recipeID);

            List<Ingredient> ingredients = recipesDAWContext.Ingredients.Where(i => i.Recipe.Id == auxRecipe.Id).ToList();
            List<Instruction> instructions = recipesDAWContext.Instructions.Where(i => i.Recipe.Id == auxRecipe.Id).ToList();

            auxRecipe.Ingredients = ingredients;
            auxRecipe.Instructions = instructions;

            SRecipe currentRecipe = new SRecipe()
            {
                Title = auxRecipe.Title,
                Description = auxRecipe.Description,
                Serves = auxRecipe.Serves,
                ImageUrl = auxRecipe.ImageUrl,
                Id = auxRecipe.Id
            };

            List<SIngredient> currentIngredients = new List<SIngredient>();
            foreach (Ingredient ingredient in ingredients)
            {
                currentIngredients.Add(new SIngredient()
                {
                    Id = ingredient.Id,
                    name = ingredient.name,
                    amount = ingredient.amount
                });
            }

            List<SInstruction> currentInstructions = new List<SInstruction>();
            foreach (Instruction instruction in instructions)
            {
                currentInstructions.Add(new SInstruction()
                {
                    Id = instruction.Id,
                    instruction = instruction.instruction
                });
            }
            auxRecipe.Ingredients = ingredients;
            auxRecipe.Instructions = instructions;

            currentRecipe.Ingredients = currentIngredients;
            currentRecipe.Instructions = currentInstructions;

            return currentRecipe;
        }


        public void SaveRecipe([FromBody]SRecipe recipe)
        {
            if (recipe.Id == Guid.Empty)
            {
                recipe.Id = Guid.NewGuid();
                Recipe RecipeToSave = new Recipe
                {
                    Id = recipe.Id,
                    Title = recipe.Title,
                    Description = recipe.Description,
                    Serves = recipe.Serves,
                    ImageUrl = recipe.ImageUrl
                };

                List<Ingredient> ingredients = new List<Ingredient>();
                foreach(SIngredient ingredient in recipe.Ingredients)
                {
                    ingredients.Add(new Ingredient()
                    {
                        name = ingredient.name,
                        amount = ingredient.amount
                    });
                }

                List<Instruction> instructions = new List<Instruction>();
                foreach (SInstruction instruction in recipe.Instructions)
                {
                    instructions.Add(new Instruction()
                    {
                        instruction = instruction.instruction
                    });
                }

                RecipeToSave.Ingredients = ingredients;
                RecipeToSave.Instructions = instructions;
                recipesDAWContext.Recipes.Add(RecipeToSave);
            }
            else
            {
                Recipe currentRecipe = recipesDAWContext.Recipes.FirstOrDefault(Recipe => Recipe.Id == recipe.Id);
                currentRecipe.Title = recipe.Title;
                currentRecipe.Serves = recipe.Serves;
                currentRecipe.ImageUrl = recipe.ImageUrl;
                currentRecipe.Description = recipe.Description;

                UpdateIngredients(recipe.Ingredients, recipe.Id);
                UpdateInstructions(recipe.Instructions, recipe.Id);
            }

            recipesDAWContext.SaveChanges();
        }


        public void UpdateIngredients(List<SIngredient> ingredients, Guid id)
        {
            Ingredient[] dbIngridients = recipesDAWContext.Ingredients.Where(i => i.Recipe.Id == id).ToArray();

            foreach (Ingredient dbIngredient in dbIngridients)
            {
                SIngredient ingredient = ingredients.FirstOrDefault(x => x.Id == dbIngredient.Id);

                if (ingredient != null)
                {
                    dbIngredient.name = ingredient.name;
                    dbIngredient.amount = ingredient.amount;
                }
            }

            recipesDAWContext.Ingredients.RemoveRange(dbIngridients.Where(dbIngridient => !ingredients.Any(ingridient => ingridient.Id == dbIngridient.Id)));

            foreach (SIngredient ingredient in ingredients)
            {
                Ingredient dbIngredient = dbIngridients.FirstOrDefault(x => x.Id == ingredient.Id);

                if (dbIngredient == null)
                {
                    dbIngredient = new Ingredient()
                    {
                        Id = new Guid(),
                        name = ingredient.name,
                        amount = ingredient.amount,
                        Recipe = new Recipe() { Id = id }
                    };

                    recipesDAWContext.Ingredients.Add(dbIngredient);
                }
            }
        }


        public void UpdateInstructions(List<SInstruction> instructions, Guid id)
        {
            Instruction[] dbInstructions = recipesDAWContext.Instructions.Where(i => i.Recipe.Id == id).ToArray();

            foreach (Instruction dbInstruction in dbInstructions)
            {
                SInstruction instruction = instructions.FirstOrDefault(x => x.Id == dbInstruction.Id);

                if (instruction != null)
                {
                    dbInstruction.instruction = instruction.instruction;
                }
            }

            recipesDAWContext.Instructions.RemoveRange(dbInstructions.Where(dbInstruction => !instructions.Any(ingridient => ingridient.Id == dbInstruction.Id)));

            foreach (SInstruction instruction in instructions)
            {
                Instruction dbInstruction = dbInstructions.FirstOrDefault(x => x.Id == instruction.Id);

                if (dbInstruction == null)
                {
                    dbInstruction = new Instruction()
                    {
                        Id = new Guid(),
                        instruction = instruction.instruction,
                        Recipe = new Recipe() { Id = id }
                    };

                    recipesDAWContext.Instructions.Add(dbInstruction);
                }
            }
        }

        public void DeleteRecipe([FromQuery]Guid RecipeId)
        {
            Recipe dbRecipe = recipesDAWContext.Recipes.FirstOrDefault(db => db.Id == RecipeId);

            List<Ingredient> auxIngredient = recipesDAWContext.Ingredients.Where(ing => ing.Recipe.Id == RecipeId).ToList();
            foreach (Ingredient ingredient in auxIngredient)
            {
                recipesDAWContext.Ingredients.Remove(ingredient);
            }

            List<Instruction> auxInstruction = recipesDAWContext.Instructions.Where(instr => instr.Recipe.Id == RecipeId).ToList();
            foreach (Instruction instruction in auxInstruction)
            {
                recipesDAWContext.Instructions.Remove(instruction);
            }

            recipesDAWContext.Recipes.Remove(dbRecipe);

            recipesDAWContext.SaveChanges();
        }

        private bool RecipeExists(Guid id)
        {
            return recipesDAWContext.Recipes.Any(e => e.Id == id);
        }
    }
}
