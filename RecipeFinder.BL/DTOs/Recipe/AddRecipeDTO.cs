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
        public string title { get; set; }
        public string instructions { get; set; }
        public List<string> ingredients { get; set; }
        public List<string> keywords { get; set; }
    }
}
