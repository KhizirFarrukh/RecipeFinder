using Microsoft.AspNetCore.Mvc;
using RecipeFinder.API.Models;
using RecipeFinder.BL.DTOs.Recipe;
using RecipeFinder.BL.ServiceInterfaces;
using RecipeFinder.Common.Commons;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace RecipeFinder.API.Controllers
{
    //[Route(DeveloperConstants.ENDPOINT_PREFIX)]
    [ApiController]
    public class RecipeController : ControllerBase
    {
        private IRecipeServiceRepo _recipeRepo;
        public RecipeController(IRecipeServiceRepo recipeRepo)
        {
            _recipeRepo = recipeRepo;
        }

        [HttpGet("AddRecipesFromJson")]
        public IActionResult AddRecipesFromJson(string filename)
        {
            try
            {
                string jsonFilePath = Path.Combine(Directory.GetCurrentDirectory(), "JSON_files", filename);
                string jsonContent = System.IO.File.ReadAllText(jsonFilePath);

                var recipeData = JsonSerializer.Deserialize<List<AddRecipeDTO>>(jsonContent);
                
                if (recipeData == null)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = string.Format(CustomMessage.ERROR_JSON_DESERIALIZING, "Recipes") });
                }
                
                List<string> unwantedWords = new List<string>
                {
                    "advertisement", "small", "cup", "cans", "ounce", "torn", "into", "pieces", ",", "finely", "diced", "skinless", "boneless",
                    "tablespoons", "packages", "refrigerated", "pounds", "pack", "packed", "teaspoons", "teaspoon", "tablespoon", "small", "crushed", "softened",
                    "medium", "cloves", "minced", "divided"
                    // Add other unwanted words here
                };

                var recipes = new List<AddRecipeDTO>();

                foreach (var recipe_kvp in recipeData)
                {
                    //var keywords = recipe_kvp.ingredients.Select(ingredient =>
                    //    Regex.Replace(ingredient, @"\b\d+(\.\d+)?(/\d+)?\b|\(|\)", "",
                    //        RegexOptions.IgnoreCase).Trim())
                    //    .ToList();

                    var keywords = recipe_kvp.ingredients.Select(ingredient =>
                    {
                        foreach (var word in unwantedWords)
                        {
                            ingredient = ingredient.Replace(word,"",StringComparison.OrdinalIgnoreCase);
                            ingredient = Regex.Replace(ingredient, @"\([^)]*\)|\d+", "").Trim();
                        }
                        return ingredient;
                    })
                    .Where(ingredient => !string.IsNullOrEmpty(ingredient))
                    .ToList();

                    recipes.Add(new AddRecipeDTO
                    {
                        title = recipe_kvp.title,
                        instructions = recipe_kvp.instructions,
                        ingredients = recipe_kvp.ingredients,
                        keywords = keywords
                    });
                }

                _recipeRepo.AddRecipes(recipes);

                return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.ADDED_SUCCESSFULLY, "Recipes") });
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }
    }
}
