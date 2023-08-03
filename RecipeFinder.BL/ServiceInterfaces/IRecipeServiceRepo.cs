using RecipeFinder.BL.DTOs.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeFinder.BL.ServiceInterfaces
{
    public interface IRecipeServiceRepo
    {
        void AddRecipes(List<AddRecipeDTO> recipes);
    }
}
