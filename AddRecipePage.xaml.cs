using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using Microsoft.Win32;

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

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            string filePath;
            if (openFileDialog.ShowDialog() == true)
            {
                File.ReadAllText(openFileDialog.FileName);
                filePath = openFileDialog.FileName;
                var bitmap =
                new BitmapImage(
                    new Uri(
                        filePath,
                        UriKind.Absolute)
                    );
                Debug.WriteLine(filePath);
                RecipeImage.Source = bitmap;
            }
        }
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LeftArrowButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RightArrowButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DescriptionBox_GotFocus(object sender, RoutedEventArgs e)
        {
            DescriptionBox.Text = "";
            DescriptionBox.Foreground = Brushes.Black;
        }

        private void DescriptionBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if(DescriptionBox.Text=="")
            {
                DescriptionBox.Text = "Viết công thức chỗ này";
                DescriptionBox.Foreground = Brushes.Gray;
            }

        }

        private void AddNameRecipeBox_GotFocus(object sender, RoutedEventArgs e)
        {
            AddNameRecipeBox.Text = "";
            AddNameRecipeBox.Foreground = Brushes.Black;
        }

        private void AddNameRecipeBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (AddNameRecipeBox.Text == "")
            {
                AddNameRecipeBox.Text = "Nhập tên công thức";
                AddNameRecipeBox.Foreground = Brushes.Gray;
            }
            
        }

        private void AddRecipeImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
            string filePath;
            if (openFileDialog.ShowDialog() == true)
            {
                File.ReadAllText(openFileDialog.FileName);
                filePath = openFileDialog.FileName;
                var bitmap =
                new BitmapImage(
                    new Uri(
                        filePath,
                        UriKind.Absolute)
                    );
                Debug.WriteLine(filePath);
                BrowseRecipeImage.Source = bitmap;
            }
        }
    }
}
