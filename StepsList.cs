using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace Food_Recipe_Appplication
{
    public class StepsList: IEnumerable<Step>
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

        public void AddStep(Step input)
        {
            steps.Add(input);
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

        public BindingList<Step> GetBindingData() {

            var stepArr = new BindingList<Step>();
            foreach (var step in steps)
            {
                stepArr.Add(step);
            }

            return stepArr;
        }
        public XElement ToXElement()
        {
            XElement result = new XElement("steps");
            foreach (var step in steps)
            {
                result.Add(step.ToXElement());
            }


            return result;
        }

        public IEnumerator<Step> GetEnumerator()
        {
            foreach (var val in steps)
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
