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

        private RecipesList recipeList = new RecipesList();
        private RecipesList searchResultList = new RecipesList();
        private Recipe temp = new Recipe();
        private int currentPage;
        public HomePage()
        {
            InitializeComponent();
            SizeChanged += HomePage_SizeChanged;
            Debug.WriteLine("iN home page");
            recipeList.LoadAll();
            searchResultList = recipeList;

        }

        private void HomePage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var windowWidth = e.NewSize.Width;
            Debug.WriteLine(windowWidth);
            SearchBlock.Margin = new Thickness(windowWidth - 700, 0, 0, 0);
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
            this.NavigationService.Navigate(new FavouritePage(recipeList));
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration(
              ConfigurationUserLevel.None);
            var option = config.AppSettings.Settings["DisplayOption"].Value;
            switch (option)
            {
                case "Order by name A-Z":
                    {
                        AtoZ.IsSelected = true;
                        recipeList = recipeList.SortByName();
                        ChangeBindingList(recipeList);
                        break;
                    }
                case "Order by name Z-A":
                    {
                        ZtoA.IsSelected = true;
                        recipeList = recipeList.SortByNameDescending();
                        ChangeBindingList(recipeList);
                        break;
                    }
                case "Order by date descending":
                    {
                        DateDescending.IsSelected = true;
                        recipeList = recipeList.SortByDateDescending();
                        ChangeBindingList(recipeList);
                        break;
                    }
                case "Order by date ascending":
                    {
                        DateAscending.IsSelected = true;
                        recipeList = recipeList.SortByDate();
                        ChangeBindingList(recipeList);
                        break;
                    }
            }
        }
        private int NumberOfRecipePerPage()
        {
            var config = ConfigurationManager.OpenExeConfiguration(
     ConfigurationUserLevel.None);
            var result = int.Parse(config.AppSettings.Settings["NumberOfRecipePerPage"].Value);
            return result;
        }
        private void  PageNumber_Click(object sender, RoutedEventArgs e)
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

            RecipesList toShow = recipeList.GetByPage(nextPage, number);
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
            foreach (var recipe in recipeList)
            {
                if (recipe.FoodName == temp.FoodName)
                {
                    recipe.ToggleFavorite();
                    if (recipe.IsFavorite == true)
                    {
                        icon.Foreground = Brushes.Red;
                    }
                    else
                    {
                        icon.Foreground = Brushes.Pink;
                    }
                }
            }


        }

        private void DetaisButton_Click(object sender, RoutedEventArgs e)
        {

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


        private void SearchBox_PreviewKeyUp(object sender, KeyEventArgs e)
        {
            string key = (sender as TextBox).Text;
            if (key == "")
            {
                //searchResultList = recipeList.SearchNameContains_NoneUtf(key);
                
                ChangeBindingList(recipeList);
            }
            else
            {
                searchResultList = recipeList.SearchNameContains_NoneUtf(key);
                ChangeBindingList(searchResultList);
                //MessageBox.Show(key);
            }
        }

      

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string condition = ((ComboBoxItem)(sender as ComboBox).SelectedItem).ToString();
            Debug.WriteLine(condition); 
            var tokens = condition.Split(new string[] { ": " }, StringSplitOptions.None);
            switch(tokens[tokens.Length-1])
            {
                case "Order by name A-Z":
                    {
                        recipeList = recipeList.SortByName();
                        ChangeBindingList(recipeList);
                        break;
                    }
                case "Order by name Z-A":
                    {
                        recipeList = recipeList.SortByNameDescending();
                        ChangeBindingList(recipeList);
                        break;
                    }
                case "Order by date descending":
                    {
                        recipeList = recipeList.SortByDateDescending();
                        ChangeBindingList(recipeList); 
                        break;
                    }
                case "Order by date ascending":
                    {
                        recipeList = recipeList.SortByDate();
                        ChangeBindingList(recipeList);
                        break;
                    }
            }
            var config = ConfigurationManager.OpenExeConfiguration(
             ConfigurationUserLevel.None);
            config.AppSettings.Settings["DisplayOption"].Value = tokens[tokens.Length - 1];
            config.Save(ConfigurationSaveMode.Minimal);
        }
    }
}

