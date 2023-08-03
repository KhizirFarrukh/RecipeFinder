using RecipeFinder.BL.DTOs.Recipe;
using RecipeFinder.BL.ServiceInterfaces;
using RecipeFinder.DAL;
using RecipeFinder.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RecipeFinder.BL.Services
{
    public class RecipeServiceRepo : IRecipeServiceRepo
    {
        public ApplicationDBContext _context;
        public RecipeServiceRepo(ApplicationDBContext context)
        {
            _context = context;
        }

        public void AddRecipes(List<AddRecipeDTO> recipesDTO)
        {
            foreach(var recipe in recipesDTO)
            {
                string ingredientsJson = JsonSerializer.Serialize(recipe.Ingredients);
                string keywordsJson = JsonSerializer.Serialize(recipe.Keywords);

                _context.Recipes.Add(new Recipe
                {
                    Title = recipe.Title,
                    Instructions = recipe.Instructions,
                    IngredientsJson = ingredientsJson,
                    KeywordsJson = keywordsJson
                });
            }

            _context.SaveChanges();
        }
    }
}
