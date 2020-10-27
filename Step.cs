using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Xml;
using System.Xml.Linq;

namespace Food_Recipe_Appplication
{
    public class Step
    {
        private string _text;
        private string _picture_name;

        
        public string Text { get => _text; set => _text = value; }
        public string Picture_name { get => _picture_name; set => _picture_name = value; }
        


        public Step()
        {
            _text = _picture_name  = "";
        }
        public Step(string inputText, string inputPictureName)
        {
            _text = inputText;
            _picture_name = inputPictureName;
            
        }

        public static Step LoadedSingleStep(XmlReader reader)
        {
            reader.Read();
            string step_text = "";
            string step_picture_name = "";

            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    switch (reader.Name)
                    {
                        case "step_text":
                            {
                                reader.Read();
                                step_text = reader.Value;
                                break;
                            }
                        case "step_picture_name":
                            {
                                reader.Read();
                                step_picture_name = reader.Value;
                                break;
                            }
                        default:
                            break;
                    }
                }

            }

            Step result = new Step(step_text, step_picture_name);
            return result;
        }

        public XElement ToXElement()
        {
            XElement result = new XElement("step",
                new XElement("step_text", _text),
                new XElement("step_picture_name", _picture_name));

            return result;

        }

    }
}
