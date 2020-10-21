using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        private StepsList _steps;

        public string FoodName { get => _foodName; set => _foodName = value; }
        public bool IsFavorite { get => _isFavorite; set => _isFavorite = value; }
        public string MainPictureName { get => _mainPictureName; set => _mainPictureName = value; }
        public StepsList Steps { get => _steps; set => _steps = value; }

        public Recipe()
        {
            _mainPictureName = "default_name";
            _foodName = "";
            _isFavorite = false;
            _steps = new StepsList();
        }

        public static Recipe LoadedSingleRecipe(XmlReader reader)
        {
            reader.Read();
            Recipe result = new Recipe();
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
                        case "mainpicture_name":
                            {
                                reader.Read();

                                string mainPictureName = reader.Value;
                                result._mainPictureName = mainPictureName;
                                break;
                            }
                        case "steps":
                            {

                                result._steps = StepsList.LoadedStepsList(reader.ReadSubtree());

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
            XElement result = new XElement("recipe", new XAttribute("isFavorite", _isFavorite ? "true" : "false"));

            result.Add(new XElement("foodname", _foodName));
            result.Add(new XElement("mainpicture_name", _mainPictureName));

            result.Add(_steps.ToXElement());


            return result;
        }
    }
}
