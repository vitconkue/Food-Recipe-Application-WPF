using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.RightsManagement;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Animation;
using System.Xml;
using System.Xml.Serialization;

namespace Food_Recipe_Appplication
{
    public class IngredientList
    {
        private List<string> _listIngredient;

        public List<string> ListIngredient { get => _listIngredient; set => _listIngredient = value; }

        public string  this[int indexer]
        {
            get => _listIngredient[indexer];

            set => _listIngredient[indexer] = value; 
        }
        
        public IngredientList()
        {
            _listIngredient = new List<string>(); 
        }
        public IngredientList(List<string> input)
        {
            _listIngredient = new List<string>(); 

            _listIngredient = input; 
        }

        public void AddIngredient(string newIngredient)
        {
            _listIngredient.Add(newIngredient); 
        }

        public static IngredientList LoadIngredientList(XmlReader reader)
        {
            IngredientList result = new IngredientList();
            while(reader.Read())
            {
                if(reader.IsStartElement())
                {
                    if(reader.Name == "ingredient")
                    {
                        reader.Read(); 
                        string newIngredient = "";
                        newIngredient = reader.Value;
                        result.AddIngredient(newIngredient); 
                    }
                }
            }

            return result;
        }
        
    }
}
