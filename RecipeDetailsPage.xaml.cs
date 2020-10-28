using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Food_Recipe_Appplication
{
    /// <summary>
    /// Interaction logic for RecipeDetailsPage.xaml
    /// </summary>
    public partial class RecipeDetailsPage : Window
    {
        private Recipe displayFood = new Recipe();
        public RecipeDetailsPage()
        {
            InitializeComponent();
        }
        public RecipeDetailsPage(Recipe food)
        {
            InitializeComponent();
            displayFood = food;
        }
        private void PackIcon_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var bindingList = displayFood.Steps.GetBindingData();
            dataListView.ItemsSource = bindingList;
            int totalPage = bindingList.Count();
        }
        
    }
}
