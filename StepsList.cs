using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Food_Recipe_Appplication
{
    public class StepsList
    {
        private List<Step> steps;

        public List<Step> Steps { get => steps; set => steps = value; }

        public Step this[int index]
        {
            get => Steps[index];
            set => Steps[index] = value;
        }

        public StepsList()
        {
            steps = new List<Step>();
        }
        public static StepsList LoadedStepsList(XmlReader reader)
        {
            StepsList result = new StepsList();
            while (reader.Read())
            {
                if (reader.IsStartElement())
                {
                    if (reader.Name == "step")
                    {
                        Step newStep = Step.LoadedSingleStep(reader.ReadSubtree());
                        result.steps.Add(newStep);


                    }
                }
            }

            return result;
        }
    }
}
