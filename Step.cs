using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls.Primitives;
using System.Xml;

namespace Food_Recipe_Appplication
{
    public class Step
    {
        private string _text;
        private string _picture_name;

        private string _video_link;

        public string Text { get => _text; set => _text = value; }
        public string Picture_name { get => _picture_name; set => _picture_name = value; }
        public string Video_link { get => _video_link; set => _video_link = value; }


        public Step()
        {
            _text = _picture_name = _video_link = "";
        }
        public Step(string inputText, string inputPictureName, string inputVideoLink)
        {
            _text = inputText;
            _picture_name = inputPictureName;
            _video_link = inputVideoLink;
        }

        public static Step LoadedSingleStep(XmlReader reader)
        {
            reader.Read();
            string step_text = "";
            string step_video_link = "";
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
                        case "step_video_link":
                            {
                                reader.Read();
                                step_video_link = reader.Value;
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

            Step result = new Step(step_text, step_picture_name, step_video_link);
            return result;
        }

    }
}
