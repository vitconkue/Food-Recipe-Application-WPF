using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;



namespace Food_Recipe_Appplication
{
    /// <summary>
    /// Interaction logic for HomePage.xaml
    /// </summary>
    public partial class HomePage : Page
    {
        private bool isFavoriteClicked = false;
        private RecipesList recipeList = new RecipesList();
        private Recipe temp = new Recipe();
        public HomePage()
        {
            InitializeComponent();
            SizeChanged += HomePage_SizeChanged;
            Debug.WriteLine("iN home page");
            recipeList.LoadAll(); 
            
        }

        private void HomePage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var windowWidth = e.NewSize.Width;
            Debug.WriteLine(windowWidth);
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

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SettingPage(recipeList));
        }

 

        private void AddRecipeButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new AddRecipePage(recipeList));
        }

        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new FavouritePage(recipeList.SearchFavoriteRecipes()));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            var number = NumberOfRecipePerPage();
            var bindingList = recipeList.GetByPage(1,number).GetBindingData();
            dataListView.ItemsSource = bindingList;
            int len = recipeList.Recipes.Count; 
            int numberOfPage = len / number + (len % number == 0 ? 0:1);
            
            for(int i = 1; i <= numberOfPage; i++)
            {
                Button numberButton = new Button();
                numberButton.Name = $"page_{i}";
                numberButton.Content =$"{i}";
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

            RecipesList toShow = recipeList.GetByPage(nextPage,number);
            dataListView.ItemsSource = toShow.GetBindingData(); 
            

        }


        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            
            var item = sender as ListViewItem;          
            if (item != null)
            {                   
                temp = (Recipe)item.Content;
            }          
        }

        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            (sender as StackPanel).Background = (Brush)bc.ConvertFrom("#CAC9C7");
        }

        private void panel_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as StackPanel).Background = Brushes.White;
        }


        private void PackIcon_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var icon = sender as PackIcon;
            icon.Foreground = Brushes.Red;
      
            
            
        }

        private void DetaisButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

