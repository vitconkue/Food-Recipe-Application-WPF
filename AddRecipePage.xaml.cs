﻿using System;
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
using System.Configuration;
using System.Windows.Media.Animation;

namespace Food_Recipe_Appplication
{
    /// <summary>
    /// Interaction logic for AddRecipePage.xaml
    /// </summary>
    public partial class AddRecipePage : Page
    {
        private RecipesList recipeList = new RecipesList();


        public AddRecipePage(RecipesList list)
        {
            InitializeComponent();
            SizeChanged += AddRecipePage_SizeChanged;
            recipeList = list;
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

        private void FavouriteButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new FavouritePage(recipeList.SearchFavoriteRecipes()));
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SettingPage(recipeList.SearchFavoriteRecipes()));
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
            DescriptionBox.Text = "Viết công thức chỗ này";
            DescriptionBox.Foreground = Brushes.Gray;
        }

        private void HomeButton_Click_1(object sender, RoutedEventArgs e)
        {

        }
    }
}
