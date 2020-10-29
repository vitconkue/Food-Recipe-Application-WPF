using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Food_Recipe_Appplication
{
    class ImageLinkConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string path = (string)value;
            var folder = AppDomain.CurrentDomain.BaseDirectory;
            var imagePath = $"{folder}Data\\Images\\" + path;
            var bitmap = new BitmapImage(new Uri(imagePath, UriKind.Absolute));
            return bitmap;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
