using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecipeFinder.DAL.Entities
{
    public class Recipe : Base
    {
        public string Title { get; set; }
        public string Instructions { get; set; }
        public string IngredientsJson { get; set; }
        public string KeywordsJson { get; set; }
    }
}
