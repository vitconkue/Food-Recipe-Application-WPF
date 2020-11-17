using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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
    /// Interaction logic for SplashScreen.xaml
    /// </summary>
   
    public partial class SplashScreen : Window
    {
        Timer timer;
        double count = 0;
        double time = 5;
        Random rng = new Random();
        private Recipe recipe = new Recipe();
        private RecipesList recipeList = new RecipesList();
        
        public SplashScreen()
        {
            InitializeComponent();
        }
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
           
            var value = ConfigurationManager.AppSettings["ShowSplashScreen"];
            var showSplash = bool.Parse(value);
            
            if (showSplash == false)
            {
                var screen = new MainWindow();
                screen.Show();
                
                this.Close();
            }
            else
            {
                recipeList.LoadAll();
                List<Recipe> temp = recipeList.Recipes;
                int len = temp.Count();
                Rerandom: int index = rng.Next(0, len - 1);
                info.Text = temp[index].Interesting_infomation;
                if (info.Text == "")
                    goto Rerandom;
                timer = new Timer();
                timer.Elapsed += Timer_Elapsed;
                timer.Interval = 10;
                timer.Start();
            }
        }

        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            count=count+ 0.01;
            if (count.CompareTo(time)==1)
            {
                timer.Stop();
                Dispatcher.Invoke(() =>
                {
                    var screen = new MainWindow();
                    screen.Show();
                    this.Close();
                });

            }

            Dispatcher.Invoke(() =>
            {
                progress.Value = count;
            });
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            var config = ConfigurationManager.OpenExeConfiguration(
                ConfigurationUserLevel.None);
            config.AppSettings.Settings["ShowSplashScreen"].Value = "false";
            config.Save(ConfigurationSaveMode.Minimal);
        }

  
        private void SkipButton_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            var screen = new MainWindow();
            screen.Show();
            this.Close();
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
