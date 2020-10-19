using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Food_Recipe_Appplication
{
    public class RecipesList: IEnumerable<Recipe>
    {
        private List<Recipe> _recipes;

        public List<Recipe> Recipes { get => _recipes; set => _recipes = value; }

        public Recipe this[int index]
        {
            get => _recipes[index];
            set => _recipes[index] = value;
        }

        public RecipesList()
        {
            _recipes = new List<Recipe>();
        }

        public void AddRecipe(Recipe in_recipe)
        {
            _recipes.Add(in_recipe);
        }

        public void LoadAll()
        {
            //TODO: implement
            XmlReaderSettings reader_setting = new XmlReaderSettings();
            reader_setting.IgnoreWhitespace = true;
            reader_setting.IgnoreComments = true;
            XmlReader recipeReader = XmlReader.Create("XMLFiles/Recipes.xml", reader_setting);
        
            using (recipeReader)
            {
                recipeReader.MoveToContent();

                while (recipeReader.Read())
                {
                    if (recipeReader.IsStartElement())
                    {
                        if (recipeReader.Name == "recipe")
                        {
                            Recipe newRecipe = new Recipe();
                            bool isFavorite = (recipeReader.GetAttribute(0) == "true") ? true : false;
                            newRecipe = Recipe.LoadedSingleRecipe(recipeReader.ReadSubtree());

                            newRecipe.IsFavorite = isFavorite;

                            _recipes.Add(newRecipe);
                        }
                    }
                }


            }
        }

        public RecipesList SearchName(string in_name)
        {
            RecipesList result = new RecipesList();
            IEnumerable<Recipe> filtered = this.Where(food => food.FoodName == in_name);
            foreach(var value in filtered)
            {
                result.AddRecipe(value); 
            }

            return result;
            
        }

        public IEnumerator<Recipe> GetEnumerator()
        {
            foreach (var val in _recipes)
            {
                yield return val;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
