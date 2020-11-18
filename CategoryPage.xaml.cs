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
    /// Interaction logic for CategoryPage.xaml
    /// </summary>
    /// 
    public partial class CategoryPage : Page
    {
        private RecipesList categoryList = new RecipesList();
        private Recipe temp = new Recipe();
        public CategoryPage()
        {
            InitializeComponent();
        }
        public CategoryPage(RecipesList recipesList)
        {
            InitializeComponent();
            SizeChanged += CategoryPage_SizeChanged;
            categoryList = recipesList;
        }

        private void CategoryPage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var windowWidth = e.NewSize.Width;
            Debug.WriteLine(windowWidth);
            SearchBlock.Margin = new Thickness(windowWidth - 350, 0, 0, 0);
        }

        private void PackIcon_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var icon = sender as PackIcon;
            icon.Foreground = Brushes.Pink;
            Debug.WriteLine(temp.FoodName);
            foreach (var recipe in categoryList)
            {
                if (recipe.FoodName == temp.FoodName)
                {
                    recipe.ToggleFavorite();
                    categoryList = categoryList.SearchFavoriteRecipes();
                    
                }
            }


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
            this.NavigationService.Navigate(new AddRecipePage(categoryList));
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SettingPage(categoryList));
        }

        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var item = sender as ListViewItem;
            if (item != null)
            {
                temp = (Recipe)item.Content;
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            ChangeBindingList(categoryList);

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


        private void ChangeBindingList(RecipesList input)
        {
            try
            {
                dataListView.ItemsSource = input.GetCategoryBindingData();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

  

        private void SeeAllButton_Click(object sender, RoutedEventArgs e)
        {
            Window categoryWindow = new CategoryWindow(temp,categoryList);
            categoryWindow.Show();
        }

        private void FavoriteButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new FavouritePage(categoryList));
        }
    }
}
