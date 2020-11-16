using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
using System.Windows.Media.Animation;
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

            var config = ConfigurationManager.OpenExeConfiguration(
              ConfigurationUserLevel.None);
            var state = config.AppSettings.Settings["ShowSplashScreen"].Value;
            var showSplash = bool.Parse(state);
            ToggleSwitch1.IsChecked = showSplash;
            recipeList = list;
            SizeChanged += SettingPage_SizeChanged;
        }

        private void SettingPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var windowWidth = e.NewSize.Width;
            Debug.WriteLine(windowWidth);
            SearchBlock.Margin = new Thickness(windowWidth - 350, 0, 0, 0);
        }

        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MenuButton.Visibility = Visibility.Collapsed;


            LeftMenu.Visibility = Visibility.Visible;
            var sb = (Storyboard)FindResource("OpenMenu");
            this.BeginStoryboard(sb);
        }

        private void LeftMenuButton_Click(object sender, RoutedEventArgs e)
        {

            var sb = (Storyboard)FindResource("CloseMenu");
            this.BeginStoryboard(sb);
            var timer = new System.Timers.Timer();
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


        private void FavouriteButton_Click(object sender, RoutedEventArgs e)
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
                int result;
                if (int.TryParse(numberDisplay.Text, out result)&&result<=10&&result>0)
                {
                    checkImg.Visibility = Visibility.Visible;
                    invalidNofication.Visibility = Visibility.Collapsed;
                    numberDisplay.Focusable = false;
                    numberDisplay.CaretBrush = Brushes.White;
                    var config = ConfigurationManager.OpenExeConfiguration(
              ConfigurationUserLevel.None);
                    var number = config.AppSettings.Settings["NumberOfRecipePerPage"].Value = numberDisplay.Text;
                    config.Save(ConfigurationSaveMode.Minimal);
                   
                }
                else
                {
                    invalidNofication.Visibility = Visibility.Visible;
                    checkImg.Visibility = Visibility.Collapsed;
                    message.Text = "Number have to less than 11 and bigger than 0!!!";
                }
            }

        }

        private void numberDisplay_MouseDown(object sender, MouseButtonEventArgs e)
        {
            numberDisplay.CaretBrush = Brushes.Black;
            numberDisplay.Focusable = true;
          
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration(
          ConfigurationUserLevel.None);
            var number = config.AppSettings.Settings["NumberOfRecipePerPage"].Value;
            numberDisplay.Text = number;
        }

        private void ShutDownButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void CategoryButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new CategoryPage(recipeList));
        }
    }
}
