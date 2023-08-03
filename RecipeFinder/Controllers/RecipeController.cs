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

        [HttpPost("AddRecipes")]
        public IActionResult AddRecipes(IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = string.Format(CustomMessage.NOT_UPLOADED, "File") });
                }
                using (var reader = new StreamReader(file.OpenReadStream()))
                {
                    string jsonContent = reader.ReadToEnd();
                    var recipeData = JsonSerializer.Deserialize<Dictionary<string, AddRecipeDTO>>(jsonContent);

                    if(recipeData == null)
                    {
                        return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, Message = string.Format(CustomMessage.ERROR_JSON_DESERIALIZING, "Recipes") });
                    }

                    List<string> unwantedWords = new List<string>
                    {
                        "advertisement", "small", "cup", "cans", "(", ")", "ounce", "torn", "into", "pieces", ",", "finely", "diced", "skinless", "boneless",
                        "tablespoons", "packages", "refrigerated", "pounds", "pack", "packed", "teaspoons", "teaspoon", "tablespoon", "small", "crushed", "softened",
                        "medium", "cloves", "minced", "divided"
                        // Add other unwanted words here
                    };

                    foreach (var recipe_kvp in recipeData)
                    {
                        //var keywords = recipe_kvp.Value.Ingredients.Select(ingredient =>
                        //    unwantedWords.Aggregate(System.Text.RegularExpressions.Regex.Replace(ingredient, @"\b\d+(\.\d+)?(/\d+)?\b", ""),
                        //    (current, word) => current.Replace(word, string.Empty, StringComparison.OrdinalIgnoreCase).Trim()))
                        //    .ToList();

                        var keywords = recipe_kvp.Value.Ingredients.Select(ingredient =>
                            unwantedWords.Aggregate(Regex.Replace(ingredient, @"\b\d+(\.\d+)?(/\d+)?\b", ""),
                                (current, word) => Regex.Replace(current, word, "", RegexOptions.IgnoreCase).Trim()))
                            .ToList();

                        var recipe = new AddRecipeDTO
                        {
                            Title = recipe_kvp.Value.Title,
                            Instructions = recipe_kvp.Value.Instructions,
                            Ingredients = recipe_kvp.Value.Ingredients,
                            Keywords = keywords
                        };
                    }

                    var recipes = new List<AddRecipeDTO>();

                    _recipeRepo.AddRecipes(recipes);

                    return Ok(new JSONResponse { Status = ResponseMessage.SUCCESS, Message = string.Format(CustomMessage.ADDED_SUCCESSFULLY, "Recipes") });
                }
            }
            catch (Exception ex)
            {
                return Ok(new JSONResponse { Status = ResponseMessage.FAILURE, ErrorMessage = ex.Message, ErrorDescription = ex?.InnerException?.ToString() ?? string.Empty });
            }
        }
    }
}
