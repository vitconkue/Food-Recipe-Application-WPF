using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Food_Recipe_Appplication
{
    public class Recipe
    {
        private string _foodName;

        private bool _isFavorite;

        private string _mainPictureName;

        private string _mainVideoLink;

        private string _interesting_infomation; 

        private StepsList _steps;

        private DateTime _date_created;

        private IngredientList _ingredients;

        private string _category; // nước uống / món nước/ món khô

        public string FoodName { get => _foodName; set => _foodName = value; }
        public bool IsFavorite { get => _isFavorite; set => _isFavorite = value; }
        public string MainPictureName { get => _mainPictureName; set => _mainPictureName = value; }
        public StepsList Steps { get => _steps; set => _steps = value; }

        public DateTime Date_created { get => _date_created; set => _date_created = value; }
        public string MainVideoLink { get => _mainVideoLink; set => _mainVideoLink = value; }
        public string Interesting_infomation { get => _interesting_infomation; set => _interesting_infomation = value; }
        public IngredientList Ingredients { get => _ingredients; set => _ingredients = value; }
        public string Category { get => _category; set => _category = value; }

        public Recipe()
        {
            _date_created = DateTime.Now;
            _mainPictureName = "default_name";
            _mainVideoLink = "default_name"; 
            _foodName = "";
            _isFavorite = false;
            _steps = new StepsList();
            _interesting_infomation = "";
            _ingredients = new IngredientList();
            _category = ""; 
        }

        public static Recipe LoadedSingleRecipe(XmlReader reader)
        {
            reader.Read();
            bool isFavorite = (reader.GetAttribute(0) == "true") ? true : false;

            Recipe result = new Recipe();
            result.IsFavorite = isFavorite;

            CultureInfo provider = CultureInfo.InvariantCulture;

            DateTime dateCreated = DateTime.ParseExact(reader.GetAttribute(1), "dd-MM-yyyy", provider).Date;

            result.Date_created = dateCreated;

            string categoryCreated = reader.GetAttribute(2);
            result._category = categoryCreated;

            //TODO: Load single recipe after received reader from RecipeList
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "foodname":
                            {
                                reader.Read();
                                string foodName = reader.Value;
                                result._foodName = foodName;
                                break;
                            }
                        case "interesting_information":
                            {
                                reader.Read();

                                string interestingInformation = reader.Value;
                                result._interesting_infomation = interestingInformation;
                                break;
                            }
                        case "mainpicture_name":
                            {
                                reader.Read();

                                string mainPictureName = reader.Value;
                                result._mainPictureName = mainPictureName;
                                break;
                            }
                        case "mainvideo_link":
                            {
                                reader.Read();
                                string mainVideoLink = reader.Value;
                                result._mainVideoLink = mainVideoLink;

                                break;
                            }
                        case "steps":
                            {

                                result._steps = StepsList.LoadedStepsList(reader.ReadSubtree());

                                break;
                            }
                        case "ingredients":
                            {
                                result._ingredients = IngredientList.LoadIngredientList(reader.ReadSubtree()); 
                                
                                break;
                            }
                        default:
                            break;
                    }
                }


            }


            return result;
        }

        public XElement ToXElement()
        {
            XElement result = new XElement("recipe", new XAttribute("isFavorite", _isFavorite ? "true" : "false"),
                new XAttribute("date_created", _date_created.Date.ToString("dd-MM-yyyy")), new XAttribute("category", _category));

            result.Add(new XElement("foodname", _foodName));
            result.Add(new XElement("mainpicture_name", _mainPictureName));
            result.Add(new XElement("mainvideo_link", _mainVideoLink));
            result.Add(new XElement("interesting_information", _interesting_infomation));
            result.Add(_ingredients.ToXElement());
            result.Add(_steps.ToXElement());
            


            return result;
        }

        public void ToggleFavorite()
        {
            _isFavorite = !_isFavorite;
            XDocument document = XDocument.Load("XMLFiles/Recipes.xml");

            XElement root = document.Root;

            var query = (from recipe in root.Elements("recipe")
                         where (string)recipe.Element("foodname") == this.FoodName
                         select recipe.Attribute("isFavorite")).FirstOrDefault();

            query.SetValue(_isFavorite.ToString().ToLower());

            document.Save("XMLFiles/Recipes.xml");
        }
    }
}
