using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
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
    /// 
   
    public partial class FavouritePage : Page
    {
        private RecipesList _favoriteList = new RecipesList();
        private RecipesList searchResultList = new RecipesList();
        private Recipe temp = new Recipe();
        private int currentPage = 1;
        private int maxPage;
        //public FavouritePage()
        //{
        //    InitializeComponent();
        //    SizeChanged += FavouritePage_SizeChanged;
        //}

        public FavouritePage(RecipesList favoriteList)
        {
            InitializeComponent();
            SizeChanged += FavouritePage_SizeChanged;

            _favoriteList = favoriteList;
            _favoriteList = _favoriteList.SearchFavoriteRecipes(); 
        }

        private void FavouritePage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var windowWidth = e.NewSize.Width;
            SearchBlock.Margin = new Thickness(windowWidth - 550, 0, 0, 0);
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
            this.NavigationService.Navigate(new SettingPage(_favoriteList));
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null)
            {
                temp = (Recipe)item.Content;              
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
            BindingDataFromFavRecipeList(_favoriteList);
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
            icon.Foreground = Brushes.Pink;
            Debug.WriteLine(temp.FoodName);
            foreach (var recipe in _favoriteList)
            { 
                if (recipe.FoodName == temp.FoodName)
                {
                    recipe.ToggleFavorite();
                    _favoriteList = _favoriteList.SearchFavoriteRecipes();
                    BindingDataFromFavRecipeList(_favoriteList);
                }
            }


        }
        private void DetaisButton_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BindingDataFromFavRecipeList(RecipesList input)
        {
            try
            {
                SkipButton.Children.Clear();
                var number = NumberOfRecipePerPage();
                var bindingList = input.GetByPage(1, number).GetBindingData();
                dataListView.ItemsSource = bindingList;
                int len = input.Recipes.Count;
                int numberOfPage = len / number + (len % number == 0 ? 0 : 1);
                if (numberOfPage != 0)
                {
                    Button preButton = new Button();
                    preButton.Name = "prev";
                    preButton.Content = "Prev";
                    preButton.Background = Brushes.White;
                    preButton.BorderBrush = Brushes.Black;
                    preButton.Foreground = Brushes.Black;
                    preButton.Margin = new Thickness(5);
                    preButton.Click += PreButton_Click;
                    SkipButton.Children.Add(preButton);
                    maxPage = numberOfPage;
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
                    Button nextButton = new Button();
                    nextButton.Name = "next";
                    nextButton.Content = "Next";
                    nextButton.Background = Brushes.White;
                    nextButton.BorderBrush = Brushes.Black;
                    nextButton.Foreground = Brushes.Black;
                    nextButton.Margin = new Thickness(5);
                    nextButton.Click += NextButton_Click;
                    SkipButton.Children.Add(nextButton);
                    Button firstButton = (Button)SkipButton.Children[1];
                    firstButton.Background = Brushes.Orange;
                   
                }
                else
                {
                    TextBlock nofication = new TextBlock();
                    nofication.Text = "Empty!!!";
                    nofication.FontSize = 40;
                    StackPanel panel = new StackPanel();
                    panel.HorizontalAlignment = HorizontalAlignment.Center;
                    panel.Children.Add(nofication);
                    GridLayout.VerticalAlignment = VerticalAlignment.Center;
                    GridLayout.Children.Add(panel);
                }
            }
            catch (Exception ex)
            {
                
            }
        }
        private void PreButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                var number = NumberOfRecipePerPage();
                currentPage = currentPage - 1;
                RecipesList toShow = _favoriteList.GetByPage(currentPage, number);
                foreach (Button button in SkipButton.Children)
                {
                    if (button.Content.ToString() == (currentPage).ToString())
                    {
                        button.Background = Brushes.Orange;
                    }
                    else
                    {
                        button.Background = Brushes.White;
                    }
                };
                dataListView.ItemsSource = toShow.GetBindingData();
            }
        }

        private void NextButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage < maxPage)
            {
                var number = NumberOfRecipePerPage();
                currentPage = currentPage + 1;
                RecipesList toShow = _favoriteList.GetByPage(currentPage, number);
                foreach (Button button in SkipButton.Children)
                {
                    if (button.Content.ToString() == (currentPage).ToString())
                    {
                        button.Background = Brushes.Orange;
                    }
                    else
                    {
                        button.Background = Brushes.White;
                    }
                };
                dataListView.ItemsSource = toShow.GetBindingData();
            }
        }

        private void SearchBox_PreviewKeyUp(object sender, KeyEventArgs e)
        { 
            string key = (sender as TextBox).Text;
            if (key == "")
            {
                //searchResultList = recipeList.SearchNameContains_NoneUtf(key);

                ChangeBindingList(_favoriteList);
            }
            else
            {
                searchResultList = _favoriteList.SearchNameContains_NoneUtf(key);
                ChangeBindingList(searchResultList);
                //MessageBox.Show(key);
            }
        }
        private void ChangeBindingList(RecipesList input)
        {
            SkipButton.Children.Clear();
            var number = NumberOfRecipePerPage();
            var bindingList = input.GetByPage(1, number).GetBindingData();
            dataListView.ItemsSource = bindingList;
            int len = input.Recipes.Count;
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
            try
            {
                Button firstButton = (Button)SkipButton.Children[0];
                firstButton.Background = Brushes.Orange;
            }
            catch (Exception ex)
            {

            }
        }

        private void ShutDownButton_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
        }
    }
}
