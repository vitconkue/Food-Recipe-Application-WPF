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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Food_Recipe_Appplication
{
    /// <summary>
    /// Interaction logic for AddRecipePage.xaml
    /// </summary>
    public partial class AddRecipePage : Page
    {
        public AddRecipePage()
        {
            InitializeComponent();
            SizeChanged += AddRecipePage_SizeChanged;
        }

        private void AddRecipePage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var windowWidth = e.NewSize.Width;
            SearchBlock.Margin = new Thickness(windowWidth - 480, 0, 0, 0);
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "";
            SearchBox.Foreground = Brushes.Black;
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "Search";
            SearchBox.Foreground = Brushes.Gray;
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            LeftMenu.Visibility = Visibility.Visible;
            MenuButton.Visibility = Visibility.Collapsed;
        }

        private void LeftMenuButton_Click(object sender, RoutedEventArgs e)
        {
            LeftMenu.Visibility = Visibility.Collapsed;
            MenuButton.Visibility = Visibility.Visible;
        }

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new HomePage());
        }

        private void FavouriteButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new FavouritePage());
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SettingPage());
        }
    }
}
