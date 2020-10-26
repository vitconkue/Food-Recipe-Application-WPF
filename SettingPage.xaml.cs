using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
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
    /// Interaction logic for SettingPage.xaml
    /// </summary>
    /// 
    
    public partial class SettingPage : Page
    {
        private RecipesList recipeList = new RecipesList();


        public SettingPage(RecipesList list)
        {
            InitializeComponent();
            SizeChanged += SettingPage_SizeChanged;
            var config = ConfigurationManager.OpenExeConfiguration(
              ConfigurationUserLevel.None);
            var state = config.AppSettings.Settings["ShowSplashScreen"].Value;
            var showSplash = bool.Parse(state);
            ToggleSwitch1.IsChecked = showSplash;
            recipeList = list;
        }
  
        private void SettingPage_SizeChanged(object sender, SizeChangedEventArgs e)
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

        private void ToggleSwitch1_Click(object sender, RoutedEventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration(
                ConfigurationUserLevel.None);
            if (ToggleSwitch1.IsChecked == true)
            {
                config.AppSettings.Settings["ShowSplashScreen"].Value = "true";
                config.Save(ConfigurationSaveMode.Minimal);
            }
            else
            {
                config.AppSettings.Settings["ShowSplashScreen"].Value = "false";
                config.Save(ConfigurationSaveMode.Minimal);
            }
        }



        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MenuButton.Visibility = Visibility.Visible;
            LeftMenuButton.Visibility = Visibility.Collapsed;
            this.NavigationService.Navigate(new HomePage());
        }


        private void FavouriteButtton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new FavouritePage(recipeList.SearchFavoriteRecipes()));
        }

        private void AddRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddRecipePage(recipeList));
        }

        private void numberDisplay_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex("[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
            
        }

        private void numberDisplay_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                if (int.Parse(numberDisplay.Text) > 10)
                {
                    invalidNofication.Visibility = Visibility.Visible;
                    checkImg.Visibility = Visibility.Collapsed;
                    message.Text = "Number have to less than 10 and bigger than 0!!!";
                }
                else
                {
                    checkImg.Visibility = Visibility.Visible;
                    invalidNofication.Visibility = Visibility.Collapsed;
                    var config = ConfigurationManager.OpenExeConfiguration(
              ConfigurationUserLevel.None);
                    var number = config.AppSettings.Settings["NumberOfRecipePerPage"].Value=numberDisplay.Text;
                    config.Save(ConfigurationSaveMode.Minimal);
                    numberDisplay.Focusable = false;
                }
            }
           
        }

        private void numberDisplay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            numberDisplay.Focusable = true;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration(
          ConfigurationUserLevel.None);
            var number = config.AppSettings.Settings["NumberOfRecipePerPage"].Value;
            numberDisplay.Text = number;
        }
    }
}
