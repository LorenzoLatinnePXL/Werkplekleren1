using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace MasterMind
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        StringBuilder sb = new StringBuilder();

        Random rnd = new Random();
        string color1, color2, color3, color4;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {

            // Set current Title in the StringBuilder
            sb.Append(this.Title);

            // Randomize colors
            color1 = GenerateRandomColor(rnd.Next(0, 6));
            color2 = GenerateRandomColor(rnd.Next(0, 6));
            color3 = GenerateRandomColor(rnd.Next(0, 6));
            color4 = GenerateRandomColor(rnd.Next(0, 6));

            sb.Append($" {color1}, {color2}, {color3}, {color4}");

            this.Title = sb.ToString();

        }

        private static string GenerateRandomColor(int randomColor)
        {
            switch (randomColor)
            {
                case 0:
                    return "Red";

                case 1:
                    return "Yellow";

                case 2:
                    return "Orange";

                case 3:
                    return "White";

                case 4:
                    return "Green";

                case 5:
                    return "Blue";

                default:
                    return "Color choice out of range.";
            }
        }
    }
}
