using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Food_Recipe_Appplication
{
    public class RecipesList
    {
        private List<Recipe> _recipes;

        public List<Recipe> Recipes { get => _recipes; set => _recipes = value; }

        public RecipesList()
        {
            _recipes = new List<Recipe>();
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




    }
}
