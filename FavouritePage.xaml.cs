using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
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
    /// Interaction logic for FavouritePage.xaml
    /// </summary>
    public partial class FavouritePage : Page
    {
        private RecipesList _favoriteList = new RecipesList(); 
        public FavouritePage()
        {
            InitializeComponent();
            SizeChanged += FavouritePage_SizeChanged;
        }

        public FavouritePage(RecipesList favoriteList)
        {
            InitializeComponent();
            SizeChanged += FavouritePage_SizeChanged;

            _favoriteList = favoriteList; 
        }

        private void FavouritePage_SizeChanged(object sender, SizeChangedEventArgs e)
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

        private void HomeButton_Click(object sender, RoutedEventArgs e)
        {
            MenuButton.Visibility = Visibility.Visible;
            LeftMenuButton.Visibility = Visibility.Collapsed;
            this.NavigationService.Navigate(new HomePage());

        }
        private void AddRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddRecipePage(_favoriteList));
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
        
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            var temp = dataListView.SelectedItem;
            if (item != null)
            {
                Recipe tempRecipe = (Recipe)item.Content;
                MessageBox.Show(tempRecipe.FoodName); 
            }
        }

        private int NumberOfRecipePerPage()
        {
            var config = ConfigurationManager.OpenExeConfiguration(
     ConfigurationUserLevel.None);
            var result = int.Parse(config.AppSettings.Settings["NumberOfRecipePerPage"].Value);
            return result;
        }


        private void PageNumber_Click(object sender, RoutedEventArgs e)
        {
            var number = NumberOfRecipePerPage();
            string[] separator = new string[] { "_" };
            string pageNumber = (sender as Button).Name;
            foreach (Button button in SkipButton.Children)
            {
                button.Background = Brushes.White;
            };
            (sender as Button).Background = Brushes.Orange;
            var tokens = pageNumber.Split(separator, StringSplitOptions.None);
            int nextPage = int.Parse(tokens[1]);

            RecipesList toShow = _favoriteList.GetByPage(nextPage, number);
            dataListView.ItemsSource = toShow.GetBindingData();





        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var number = NumberOfRecipePerPage();
            var bindingList = _favoriteList.GetByPage(1, number).GetBindingData();
            dataListView.ItemsSource = bindingList;
            int len = _favoriteList.Recipes.Count;
            int numberOfPage = len / number + (len % number == 0 ? 0 : 1);

            for (int i = 1; i <= numberOfPage; i++)
            {
                Button numberButton = new Button();
                numberButton.Name = $"page_{i}";
                numberButton.Content = $"{i}";
                numberButton.Background = Brushes.White;
                numberButton.BorderBrush = Brushes.Black;
                numberButton.Foreground = Brushes.Black;
                numberButton.Margin = new Thickness(5);
                numberButton.Click += PageNumber_Click;
                SkipButton.Children.Add(numberButton);
            }
            Button firstButton = (Button)SkipButton.Children[0];
            firstButton.Background = Brushes.Orange;
        }
    }
}
