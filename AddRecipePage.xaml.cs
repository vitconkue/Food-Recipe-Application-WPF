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
using System.Runtime.CompilerServices;
using System.Windows.Media.Animation;

namespace Food_Recipe_Appplication
{
    /// <summary>
    /// Interaction logic for AddRecipePage.xaml
    /// </summary>
    public partial class AddRecipePage : Page
    {
        private RecipesList recipeList = new RecipesList();
        //public AddRecipePage()
        //{
        //    InitializeComponent();
        //    SizeChanged += AddRecipePage_SizeChanged;
        //}

        public AddRecipePage(RecipesList recipes)
        {
            InitializeComponent();
            recipeList = recipes;
        }

        private int Step = 0;
        private Recipe recipe = new Recipe();
        private StepsList list = new StepsList();
        private List<string> pathList = new List<string>();
        private string IngredientsString = "123";
        private string filePath = "123";
        
      

     
        private void ShutDownButton_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            parentWindow.Close();
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
            this.NavigationService.Navigate(new HomePage());
        }

        private void FavouriteButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new FavouritePage(recipeList));
        }

        private void SettingButton_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new SettingPage(recipeList));
        }

        private void AddImage_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog();
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
            //Save this Step
            if (Step >= list.Count() || Step == 0)
            {
                if (Step == 0)
                {
                    recipe.FoodName = AddNameRecipeBox.Text;
                    recipe.MainVideoLink = VideoLinkBox.Text;
                    recipe.Interesting_infomation = InterestingInfoBox.Text;
                    recipe.Category = Category.Text;
                    IngredientsString = DescriptionBox.Text;
                    pathList.Add(filePath);
                    AddNameRecipeBox.Focusable = false;
                    VideoLinkBox.Focusable = false;
                    InterestingInfoBox.Focusable = false;
                    Category.IsEnabled = false;
                }
                else
                {
                    if (Step == list.Count())
                    {
                        Step recipeStep = new Step();
                        recipeStep.Text = DescriptionBox.Text;
                        pathList.RemoveAt(Step);
                        pathList.Add(filePath);
                        list.Steps[Step - 1] = recipeStep;
                    }
                    else
                    {
                        Step recipeStep = new Step();
                        recipeStep.Text = DescriptionBox.Text;
                        pathList.Add(filePath);
                        list.AddStep(recipeStep);
                    }
                }
                Step++;
                StepNumber.Text = Step.ToString();
                DescriptionBox.Text = "Nhập hướng dẫn thực hiện";
                DescriptionBox.Foreground = Brushes.Gray;
                var bitmap =
                new BitmapImage(
                    new Uri(
                        "Icons/unknown.png",
                        UriKind.Relative)
                    );
                RecipeImage.Source = bitmap;
            }
            else
            {
                //Save
                Step recipeStep = new Step();
                recipeStep.Text = DescriptionBox.Text;
                pathList.RemoveAt(Step);
                pathList.Insert(Step, filePath);
                list.Steps[Step - 1] = recipeStep;
                //Next Step
                Step++;
                StepNumber.Text = Step.ToString();
                DescriptionBox.Text = list[Step - 1].Text;
                filePath = pathList[Step];
                var bitmap =
                    new BitmapImage(
                    new Uri(
                        filePath,
                        UriKind.Absolute)
                    );
                RecipeImage.Source = bitmap;
            }
            //

            var directory = AppDomain.CurrentDomain.BaseDirectory;
            directory += "Data\\Images\\" + recipe.FoodName;
            // If directory does not exist, create it
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            for (int i = 0; i < pathList.Count(); i++)
            {
                //Tạo filename
                string fileName = "img" + i + ".png";
                //Thêm filename vào Step
                if (i == 0)
                {
                    recipe.MainPictureName = recipe.FoodName + "//"+ fileName;
                }
                else
                {
                    list[i - 1].Picture_name = recipe.FoodName + "//" + fileName;
                }
                string sourcePath = pathList[i];
                string targetPath = directory;
                //Combine file và đường dẫn
                string sourceFile = System.IO.Path.Combine(sourcePath, "");
                string destFile = System.IO.Path.Combine(targetPath, fileName);
                //Copy file từ file nguồn đến file đích
                System.IO.File.Copy(sourceFile, destFile, true);
            }
            recipe.Steps = list;
            //Tách chuỗi nguyên liệu
            string ingredient = "";
            for (int i = 0; i < IngredientsString.Length; i++)
            {
                if (IngredientsString[i] != '\r' && IngredientsString[i] != '\n')
                {
                    ingredient += IngredientsString[i];
                }
                else
                {
                    recipe.Ingredients.AddIngredient(ingredient);
                    ingredient = "";
                    while (IngredientsString[i] == '\r' || IngredientsString[i] == '\n')
                    {
                        i++;
                        if (i == IngredientsString.Length)
                        {
                            break;
                        }
                    }
                    i--;
                }
            }
            recipe.Ingredients.AddIngredient(ingredient);
            recipeList.AddRecipe(recipe);
            //Messagebox
            MessageBox.Show("Tạo món ăn mới thành công");
            this.NavigationService.Navigate(new HomePage());
        }

        private void LeftArrowButton_Click(object sender, RoutedEventArgs e)
        {
            if (Step != 1)
            {
                //Save
                if (Step < list.Count())
                {
                    Step recipeStep = new Step();
                    recipeStep.Text = DescriptionBox.Text;
                    pathList.RemoveAt(Step);
                    pathList.Insert(Step, filePath);
                    list.Steps[Step - 1] = recipeStep;
                }
                else if (Step == list.Count())
                {
                    Step recipeStep = new Step();
                    recipeStep.Text = DescriptionBox.Text;
                    pathList.RemoveAt(Step);
                    pathList.Add(filePath);
                    list.Steps[Step - 1] = recipeStep;
                }
                else
                {
                    Step recipeStep = new Step();
                    recipeStep.Text = DescriptionBox.Text;
                    pathList.Add(filePath);
                    list.AddStep(recipeStep);
                }
                //Previous Step
                Step--;
                StepNumber.Text = Step.ToString();
                if (Step == 0)
                {
                    AddNameRecipeBox.Text = recipe.FoodName;
                    VideoLinkBox.Text = recipe.MainVideoLink;
                    filePath = pathList[Step];
                    var bitmap =
                    new BitmapImage(
                        new Uri(
                            filePath,
                            UriKind.Absolute)
                        );
                    Debug.WriteLine(filePath);
                    RecipeImage.Source = bitmap;
                }
                else
                {
                    DescriptionBox.Text = list.Steps[Step - 1].Text;
                    filePath = pathList[Step];
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
        }

        private void RightArrowButton_Click(object sender, RoutedEventArgs e)
        {
            if (Step >= list.Count() || Step == 0)
            {
                if (Step == 0)
                {
                    recipe.FoodName = AddNameRecipeBox.Text;
                    recipe.MainVideoLink = VideoLinkBox.Text;
                    recipe.Interesting_infomation = InterestingInfoBox.Text;
                    if (Category.Text == "Món khô")
                    {
                        recipe.Category = "dryfood"; 
                    }
                    else if(Category.Text == "Món nước")
                    {
                        recipe.Category = "waterdish";
                    }
                    else
                    {
                        recipe.Category = "drink";
                    }
                    IngredientsString = DescriptionBox.Text;
                    pathList.Add(filePath);
                    AddNameRecipeBox.Focusable = false;
                    VideoLinkBox.Focusable = false;
                    InterestingInfoBox.Focusable = false;
                    Category.IsEnabled = false;
                }
                else
                {
                    if (Step == list.Count())
                    {
                        Step recipeStep = new Step();
                        recipeStep.Text = DescriptionBox.Text;
                        pathList.RemoveAt(Step);
                        pathList.Add(filePath);
                        list.Steps[Step - 1] = recipeStep;
                    }
                    else
                    {
                        Step recipeStep = new Step();
                        recipeStep.Text = DescriptionBox.Text;
                        pathList.Add(filePath);
                        list.AddStep(recipeStep);
                    }
                }
                Step++;
                StepNumber.Text = Step.ToString();
                DescriptionBox.Text = "Nhập hướng dẫn thực hiện";
                DescriptionBox.Foreground = Brushes.Gray;
                var directory = AppDomain.CurrentDomain.BaseDirectory;
                directory += "Data\\Images\\unknown.png";
                filePath = directory;
                var bitmap =
                new BitmapImage(
                    new Uri(
                        "Icons\\unknown.png",
                        UriKind.Relative)
                    );
                RecipeImage.Source = bitmap;
            }
            else
            {
                //Save
                Step recipeStep = new Step();
                recipeStep.Text = DescriptionBox.Text;
                pathList.RemoveAt(Step);
                pathList.Insert(Step, filePath);
                list.Steps[Step - 1] = recipeStep;
                //Next Step
                Step++;
                StepNumber.Text = Step.ToString();
                DescriptionBox.Text = list[Step - 1].Text;
                filePath = pathList[Step];
                var bitmap =
                    new BitmapImage(
                    new Uri(
                        filePath,
                        UriKind.Absolute)
                    );
                RecipeImage.Source = bitmap;
            }
        }
        private void DescriptionBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (DescriptionBox.Text == "Nhập danh sách nguyên liệu (xuống dòng mỗi loại)" || DescriptionBox.Text == "Nhập hướng dẫn thực hiện")
            {
                DescriptionBox.Text = "";
                DescriptionBox.Foreground = Brushes.Black;
            }
        }

        private void DescriptionBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (DescriptionBox.Text == "")
            {
                if (Step == 0)
                {
                    DescriptionBox.Text = "Nhập danh sách nguyên liệu (xuống dòng mỗi loại)";
                    DescriptionBox.Foreground = Brushes.Gray;
                }
                else
                {
                    DescriptionBox.Text = "Nhập hướng dẫn thực hiện";
                    DescriptionBox.Foreground = Brushes.Gray;
                }
            }

        }

        private void AddNameRecipeBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (AddNameRecipeBox.Text == "Nhập tên công thức")
            {
                AddNameRecipeBox.Text = "";
                AddNameRecipeBox.Foreground = Brushes.Black;
            }
        }

        private void AddNameRecipeBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (AddNameRecipeBox.Text == "")
            {
                AddNameRecipeBox.Text = "Nhập tên công thức";
                AddNameRecipeBox.Foreground = Brushes.Gray;
            }

        }


        private void VideoLinkBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (VideoLinkBox.Text == "Nhập đường dẫn video")
            {
                VideoLinkBox.Text = "";
                VideoLinkBox.Foreground = Brushes.Black;
            }
        }

        private void VideoLinkBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (VideoLinkBox.Text == "")
            {
                VideoLinkBox.Text = "Nhập đường dẫn video";
                VideoLinkBox.Foreground = Brushes.Gray;
            }
        }

        private void VideoLinkBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void InterestingInfoBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (InterestingInfoBox.Text == "Nhập thông tin thú vị về món ăn")
            {
                InterestingInfoBox.Text = "";
                InterestingInfoBox.Foreground = Brushes.Black;
            }
        }

        private void InterestingInfoBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (InterestingInfoBox.Text == "")
            {
                InterestingInfoBox.Text = "Nhập thông tin thú vị về món ăn";
                InterestingInfoBox.Foreground = Brushes.Gray;
            }
        }
        private void Category_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }
    }
}
