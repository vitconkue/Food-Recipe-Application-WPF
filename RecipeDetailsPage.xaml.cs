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
            FoodName.Text = displayFood.FoodName;
            ExitButton.Margin = new Thickness(0,0,1100,0);
            bindingList = displayFood.Steps.GetBindingData();
            var ingredients = displayFood.Ingredients.ListIngredient;
            foreach(var ingredient in ingredients)
            {
                TextBlock text = new TextBlock();
                text.Text = $"-{ingredient}";
                ingredientPanel.Children.Add(text);
            }
            totalStep = bindingList.Count();
            Button nextStep = new Button();
            Button prevStep = new Button();
            nextStep.Content = "Next Step";
            prevStep.Content = "Prev Step";
            nextStep.Margin = new Thickness(20, 0, 20, 0);
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
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
