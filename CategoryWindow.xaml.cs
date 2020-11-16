﻿using MaterialDesignThemes.Wpf;
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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Food_Recipe_Appplication
{
    /// <summary>
    /// Interaction logic for CategoryWindow.xaml
    /// </summary>
    public partial class CategoryWindow : Window
    {
        private RecipesList recipeList = new RecipesList();
        private RecipesList searchResultList = new RecipesList();
        private Recipe temp = new Recipe();
        private Recipe food = new Recipe();
        private int currentPage = 1;
        private int maxPage;
        public CategoryWindow()
        {
            InitializeComponent();
        }
        public CategoryWindow(Recipe foodTemp,RecipesList foodList)
        {
            InitializeComponent();
            food = foodTemp;
            recipeList = foodList;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            categoryText.Text = food.Category;
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
            ChangeBindingList(recipeList);
        }
        private int NumberOfRecipePerPage()
        {
            var config = ConfigurationManager.OpenExeConfiguration(
     ConfigurationUserLevel.None);
            var result = int.Parse(config.AppSettings.Settings["NumberOfRecipePerPage"].Value);
            return result;
        }
        private void StackPanel_MouseEnter(object sender, MouseEventArgs e)
        {
            BrushConverter bc = new BrushConverter();
            (sender as StackPanel).Background = (Brush)bc.ConvertFrom("#ededed");
        }

        private void panel_MouseLeave(object sender, MouseEventArgs e)
        {
            (sender as StackPanel).Background = Brushes.White;
        }
        private void ChangeBindingList(RecipesList input)
        {

            SkipButton.Children.Clear();
            var number = NumberOfRecipePerPage();
            var bindingList = input.GetByPage(1, number).GetCategoryBindingList(food.Category);
            dataListView.ItemsSource = bindingList;
            int len = input.Recipes.Count;
            int numberOfPage = len / number + (len % number == 0 ? 0 : 1);
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
            try
            {
                Button firstButton = (Button)SkipButton.Children[1];
                BrushConverter bc = new BrushConverter();
                firstButton.Background = (Brush)bc.ConvertFrom("#ed81a1");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void PreButton_Click(object sender, RoutedEventArgs e)
        {
            if (currentPage > 1)
            {
                var number = NumberOfRecipePerPage();
                currentPage = currentPage - 1;
                RecipesList toShow = recipeList.GetByPage(currentPage, number);
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
                RecipesList toShow = recipeList.GetByPage(currentPage, number);
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
            currentPage = nextPage;
            RecipesList toShow = recipeList.GetByPage(nextPage, number);
            dataListView.ItemsSource = toShow.GetBindingData();


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
            Window detailScreen = new RecipeDetailsPage(temp);
            detailScreen.Show();
        }

            
        private void ListViewItem_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            var item = sender as ListViewItem;
            if (item != null)
            {
                temp = (Recipe)item.Content;
            }
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string condition = ((ComboBoxItem)(sender as ComboBox).SelectedItem).ToString();
            Debug.WriteLine(condition);
            var tokens = condition.Split(new string[] { ": " }, StringSplitOptions.None);
            switch (tokens[tokens.Length - 1])
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
        }

         private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "";
            SearchBox.Foreground = Brushes.Black;
        }

        private void SearchBox_LostFocus(object sender, RoutedEventArgs e)
        {
            SearchBox.Text = "Search";
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
    }
}