using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace RecipeFinder.BL.DTOs.Recipe
{
    public class AddRecipeDTO
    {
        public string Title { get; set; }
        public string Instructions { get; set; }
        public List<string> Ingredients { get; set; }
        [JsonIgnore]
        public List<string> Keywords { get; set; }
    }
}
