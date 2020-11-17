using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Xml;
using System.Xml.Linq;

namespace Food_Recipe_Appplication
{
    public class RecipesList : IEnumerable<Recipe>
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

            root.Save("XMLFiles/Recipes.xml");

        }
        public BindingList<Recipe> GetBindingData()
        {
            var foods = new BindingList<Recipe>();
            foreach( var recipe in _recipes)
            {
                foods.Add(recipe);
            }

            return foods;
        }
        public BindingList<Recipe> GetCategoryBindingData()
        {
            var foods = new BindingList<Recipe>();
            foreach (var recipe in _recipes)
            {
                if (recipe.Category == "Dry Food")
                {
                    foods.Add(recipe);
                    break;
                }
            }
            foreach (var recipe in _recipes)
            {
                if (recipe.Category == "Water Dish")
                {
                    foods.Add(recipe);
                    break;
                }
            }
            foreach (var recipe in _recipes)
            {
                if (recipe.Category == "Drink")
                {
                    foods.Add(recipe);
                    break;
                }
            }
            return foods;
        }
        public RecipesList GetCategoryBindingList(string category)
        {
            var foods = new RecipesList();
            foreach (var recipe in _recipes)
            {
                if (recipe.Category == category)
                    foods.AddRecipeWithoutSaving(recipe);
            }
            return foods;
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
                            Debug.WriteLine(newRecipe.FoodName);
                        }
                    }
                }


            }
        }

        public RecipesList SearchExactName(string in_name)
        {
            RecipesList result = new RecipesList();
            IEnumerable<Recipe> filtered = this.Where(food => food.FoodName == in_name);
            foreach (var value in filtered)
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
            IEnumerable<Recipe> filtered = this.Where(food => food.FoodName.ToLower().Contains(in_name.ToLower()) ||
            HelperFunctions.RemovedUTF(food.FoodName.ToLower()).Contains(HelperFunctions.RemovedUTF(in_name.ToLower())));

            // Order by: search rate

            filtered.OrderByDescending(food => HelperFunctions.rateSearchResult(in_name ,food.FoodName));

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

        public RecipesList SortByName()
        {
            RecipesList result = new RecipesList();
            List<Recipe> filtered = this.OrderBy(food => food.FoodName).ToList();
            foreach (var value in filtered)
            {
                Debug.WriteLine(value.FoodName);
                result.AddRecipeWithoutSaving(value);
            }
            return result;
        }

        public RecipesList SortByNameDescending()
        {
            RecipesList result = new RecipesList();
            IEnumerable<Recipe> filtered = this.OrderByDescending(food => food.FoodName);
            foreach (var value in filtered)
            {
                result.AddRecipeWithoutSaving(value);
            }
            return result;
        }

        public RecipesList SortByDate()
        {
            RecipesList result = new RecipesList();
            IEnumerable<Recipe> filtered = this.OrderBy(food => food.Date_created);
            foreach (var value in filtered)
            {
                result.AddRecipeWithoutSaving(value);
            }
            return result;
        }

        public RecipesList SortByDateDescending()
        {
            RecipesList result = new RecipesList();
            IEnumerable<Recipe> filtered = this.OrderByDescending(food => food.Date_created);
            foreach (var value in filtered)
            {
                result.AddRecipeWithoutSaving(value);
            }
            return result;
        }

        // PAGING HELPER

        public RecipesList GetByPage(int pageNumber, int numberOfRecipePerPage)
        {
            RecipesList result = new RecipesList();
            int len = this.Count();
            // Skip the number of previous page's recipe
            IEnumerable<Recipe> filtered = this._recipes.Skip((pageNumber - 1) * numberOfRecipePerPage);
            // Take the appropiate number to take

            int numberToTake = numberOfRecipePerPage;

            int numberOfRecipeLeft = len - (pageNumber - 1) * numberOfRecipePerPage;

            if (numberOfRecipeLeft < numberOfRecipePerPage)
            {
                numberToTake = numberOfRecipeLeft;
            }

            filtered = filtered.Take(numberToTake);

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
