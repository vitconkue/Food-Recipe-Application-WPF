using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Shapes;

namespace Food_Recipe_Appplication
{
    /// <summary>
    /// Interaction logic for RecipeDetailsPage.xaml
    /// </summary>
    public partial class RecipeDetailsPage : Window
    {
        private Recipe displayFood = new Recipe();
        private BindingList<Step> bindingList;
        private string videoUrl;
        private int currentStep = 0;
        private int totalStep;
        public RecipeDetailsPage()
        {
            InitializeComponent();
        }
        public RecipeDetailsPage(Recipe food)
        {
            InitializeComponent();
            displayFood = food;
        }
     

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            videoUrl = displayFood.MainVideoLink;
            Info.Text = displayFood.Interesting_infomation;
            FoodName.Text = displayFood.FoodName;
            Step.Text = "Bước 1";
            ExitButton.Margin = new Thickness(0,0,1100,0);
            bindingList = displayFood.Steps.GetBindingData();
            var ingredients = displayFood.Ingredients.ListIngredient;
            foreach(var ingredient in ingredients)
            {
                TextBlock text = new TextBlock();
                if (ingredient != "")
                {
                    text.Text = $"- {ingredient}";
                    text.TextWrapping = TextWrapping.Wrap;
                    ingredientPanel.Children.Add(text);
                }
            }
            totalStep = bindingList.Count();
            Button nextStep = new Button();
            Button prevStep = new Button();
            nextStep.Content = "Next Step";
            prevStep.Content = "Prev Step";
            nextStep.Margin = new Thickness(30, 0, 20, 0);
            prevStep.Margin = new Thickness(20, 0, 0, 0);
            ButtonPanel.Children.Add(prevStep);
            ButtonPanel.Children.Add(nextStep);
        
            nextStep.Click += NextStep_Click;
            prevStep.Click += PrevStep_Click;
            var step = bindingList[currentStep];
            BindingList<Step> temp = new BindingList<Step>();
            temp.Add(step);
            dataListView.ItemsSource = temp;
        }

        private void PrevStep_Click(object sender, RoutedEventArgs e)
        {
            if (currentStep > 0)
            {
                Debug.WriteLine(currentStep);

                var step = bindingList[--currentStep];
                BindingList<Step> temp = new BindingList<Step>();
                temp.Add(step);
                dataListView.ItemsSource = temp;
                Step.Text = $"Bước {currentStep+1}";
            }
        }

        private void NextStep_Click(object sender, RoutedEventArgs e)
        {
            if (currentStep < totalStep-1)
            {
                Debug.WriteLine(currentStep);
                var step = bindingList[++currentStep];   
                BindingList<Step> temp = new BindingList<Step>();
                temp.Add(step);
                dataListView.ItemsSource = temp;
                Step.Text = $"Bước {currentStep+1}";
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
           
            videoGrid.Navigate("https://www.google.com/");
            this.Close();
        }

        private void WebBrowser_Loaded(object sender, RoutedEventArgs e)
        {
            string[] separator = new string[] { "=" };
            var tokens = videoUrl.Split(separator, StringSplitOptions.None);
            string videoID = tokens[tokens.Length - 1];
            string html = "<html><head>";
            html += "<meta content='IE=Edge' http-equiv='X-UA-Compatible'/>";
            html += "</head><body>";
            html += $"<iframe width='560' height='315' src='https://www.youtube.com/embed/{videoID}' frameborder='0' allow='accelerometer; autoplay; clipboard-write; encrypted-media; gyroscope; picture-in-picture' allowfullscreen></iframe>";
            html += "</body></html>";
            (sender as WebBrowser).NavigateToString(html);
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }
    }
}

