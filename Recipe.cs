using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Food_Recipe_Appplication
{
    class Step
    {
        private string _text; 
        private string _video_link;

        private string _image_name;

        public string Text { get => _text; set => _text = value; }
        public string Video_link { get => _video_link; set => _video_link = value; }
        public string Image_name { get => _image_name; set => _image_name = value; }
    }
    class Recipe
    {
        private string _name;
        public string Name
        {
            get{
                return _name; 
            }
            set{
                _name = value; 
                // getter for name
            }
            
        }
    }
}
