using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Food_Recipe_Appplication
{
    class RecipeNameConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string foodName = (string)value;
            StringBuilder result = new StringBuilder(foodName);
            int len = foodName.Length;
            if (len > 16)
            {
                int num = len - 16;
                result.Remove(16, num);
                int newlen = result.Length; 
                for(int i = newlen - 4; i < newlen; i++)
                {
                    result[i] = '.';
                }
                
            }
            return result.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
