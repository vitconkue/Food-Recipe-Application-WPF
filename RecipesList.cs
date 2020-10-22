﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Food_Recipe_Appplication
{
    public class RecipesList: IEnumerable<Recipe>
    {
        private List<Recipe> _recipes;
        private XDocument document = XDocument.Load("XMLFiles/Recipes.xml");


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

        public void AddRecipeWithoutSaving(Recipe in_recipe)
        {
            _recipes.Add(in_recipe);
        }

        public void AddRecipe(Recipe in_recipe)
        {
            // Add to the list on memory
            _recipes.Add(in_recipe);
            // Update to XML


            XElement root = document.Root;

            root.Add(in_recipe.ToXElement());

            root.Save("Recipes.xml");

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

                            newRecipe = Recipe.LoadedSingleRecipe(recipeReader.ReadSubtree());



                            _recipes.Add(newRecipe);
                        }
                    }
                }


            }
        }

        public RecipesList SearchExactName(string in_name)
        {
            RecipesList result = new RecipesList();
            IEnumerable<Recipe> filtered = this.Where(food => food.FoodName == in_name);
            foreach(var value in filtered)
            {
                result.AddRecipeWithoutSaving(value); 

            }

            return result;
            
        }

        public RecipesList SearchNameContains(string in_name)
        {
            RecipesList result = new RecipesList();
            IEnumerable<Recipe> filtered = this.Where(food => food.FoodName.Contains(in_name));
            foreach (var value in filtered)
            {
                result.AddRecipeWithoutSaving(value); 

            }

            return result;

        }

        public RecipesList SearchNameContains_NoneUtf(string in_name)
        {
            RecipesList result = new RecipesList();
            // query
            IEnumerable<Recipe> filtered = this.Where(food => food.FoodName.Contains(in_name) ||
            HelperFunctions.RemovedUTF(food.FoodName).Contains(HelperFunctions.RemovedUTF(in_name)));
            foreach (var value in filtered)
            {
                result.AddRecipeWithoutSaving(value);
            }

            return result;

        }

        public RecipesList SearchFavoriteRecipes()
        {
            RecipesList result = new RecipesList();
            IEnumerable<Recipe> filtered = this.Where(food => food.IsFavorite == true);

            foreach (var value in filtered)
            {
                result.AddRecipeWithoutSaving(value);
            }

            return result; 
        }

        public RecipesList SortName()
        {
            RecipesList result = new RecipesList();
            IEnumerable<Recipe> filtered = this.OrderBy(food => food.FoodName);
            foreach (var value in filtered)
            {
                result.AddRecipeWithoutSaving(value);
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
