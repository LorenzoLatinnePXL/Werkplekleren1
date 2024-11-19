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
using System.Windows.Threading;

namespace MasterMind
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        // Variables
        StringBuilder sb = new StringBuilder();
        Random rnd = new Random();
        DispatcherTimer timer;
        TimeSpan elapsedTime;
        DateTime clicked;
        string color1, color2, color3, color4;
        string[] solution;
        string[] options = { "Red", "Yellow", "Orange", "White", "Green", "Blue" };
        int attempts = 0;
        bool debugMode = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            // Get the current time when the program is launched.
            clicked = DateTime.Now;

            // Set current Title in the StringBuilder.
            sb.Append(this.Title);

            // Randomize colors.
            color1 = GenerateRandomColor();
            color2 = GenerateRandomColor();
            color3 = GenerateRandomColor();
            color4 = GenerateRandomColor();
            solution = new string[] { color1, color2, color3, color4 };

            // Set solution in the hidden TextBox.
            solutionTextBox.Text = $"{color1}, {color2}, {color3}, {color4}";

            // Set timer to timerLabel.
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(1);
            timer.Tick += Timer_Tick;
            timerLabel.Content = "Timer: 0,000 / 10";

            // Show total attempts to the user:
            attemptsLabel.Content = $"Attempt: {attempts} / 10";

            // Generate 6 available colors for each ComboBox (from the options array variable)
            AddComboBoxItems(ComboBoxOption1);
            AddComboBoxItems(ComboBoxOption2);
            AddComboBoxItems(ComboBoxOption3);
            AddComboBoxItems(ComboBoxOption4);

            // Start timer when window is loaded.
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            elapsedTime = DateTime.Now - clicked;
            SetTimerLabelContent(elapsedTime);
            SetTimerLabelLayout(elapsedTime);
            SetAttemptLabelLayout();

            if (elapsedTime.TotalSeconds >= 10)
            {
                RestartTimer();
            }
        }

        private void RestartTimer()
        {
            timer.Stop();
            clicked = DateTime.Now;
            attempts++;
            attemptsLabel.Content = $"Attempt: {attempts} / 10";
            timer.Start();
        }

        private void SetTimerLabelLayout(TimeSpan elapsedTime)
        {
            if (elapsedTime.TotalSeconds >= 8)
            {
                timerLabel.Foreground = Brushes.Red;
                timerLabel.FontWeight = FontWeights.Bold;
            } else if (elapsedTime.TotalSeconds >= 5)
            {
                timerLabel.Foreground = Brushes.Orange;
                timerLabel.FontWeight = FontWeights.DemiBold;
            } else
            {
                timerLabel.Foreground = Brushes.Black;
                timerLabel.FontWeight = FontWeights.Regular;
            }
        }

        private void SetAttemptLabelLayout()
        {
            if (attempts >= 8)
            {
                attemptsLabel.Foreground = Brushes.Red;
                attemptsLabel.FontWeight = FontWeights.Bold;
            } else if (attempts >= 5)
            {
                attemptsLabel.Foreground = Brushes.Orange;
                attemptsLabel.FontWeight = FontWeights.DemiBold;
            } else
            {
                attemptsLabel.Foreground = Brushes.Black;
                attemptsLabel.FontWeight = FontWeights.Regular;
            }
        }

        private void SetTimerLabelContent(TimeSpan elapsedTime)
        {
            timerLabel.Content = $"Timer: {elapsedTime.TotalSeconds.ToString("N3")} / 10";
        }

        private string GenerateRandomColor()
        {
            // Generate a random number between 0 and 5.
            int randomColor = rnd.Next(0, 6);

            // Check which color to return from the picked number.
            switch (randomColor)
            {
                case 0:
                    return options[0];
                case 1:
                    return options[1];
                case 2:
                    return options[2];
                case 3:
                    return options[3];
                case 4:
                    return options[4];
                case 5:
                    return options[5];
                default:
                    return "Color choice out of range.";
            }
        }

        private ComboBox AddComboBoxItems(ComboBox ComboBox)
        {
            for (int i = 0; i < options.Length; i++)
            {
                ComboBox.Items.Add(options[i]);
            }
            return ComboBox;
        }

        private void ComboBoxOption_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;

            if (comboBox == ComboBoxOption1)
            {
                colorLabel1.Background = ChangeLabelBackgroundColor(comboBox);
            }
            else if (comboBox == ComboBoxOption2)
            {
                colorLabel2.Background = ChangeLabelBackgroundColor(comboBox);
            }
            else if (comboBox == ComboBoxOption3)
            {
                colorLabel3.Background = ChangeLabelBackgroundColor(comboBox);
            }
            else
            {
                colorLabel4.Background = ChangeLabelBackgroundColor(comboBox);
            }
        }

        private Brush ChangeLabelBackgroundColor(ComboBox ComboBox)
        {
            switch (ComboBox.SelectedIndex)
            {
                case 0:
                    return (Brush)new BrushConverter().ConvertFromString(options[0]);

                case 1:
                    return (Brush)new BrushConverter().ConvertFromString(options[1]);

                case 2:
                    return (Brush)new BrushConverter().ConvertFromString(options[2]);

                case 3:
                    return (Brush)new BrushConverter().ConvertFromString(options[3]);

                case 4:
                    return (Brush)new BrushConverter().ConvertFromString(options[4]);

                case 5:
                    return (Brush)new BrushConverter().ConvertFromString(options[5]);

                default:
                    return Brushes.Black;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            CheckCode(solution, ComboBoxOption1, colorLabel1, 0);
            CheckCode(solution, ComboBoxOption2, colorLabel2, 1);
            CheckCode(solution, ComboBoxOption3, colorLabel3, 2);
            CheckCode(solution, ComboBoxOption4, colorLabel4, 3);

            RestartTimer();
        }

        private bool ColorInCorrectPosition(string[] solution, ComboBox comboBox, int position)
        {
            if (comboBox.Text == solution[position].ToString())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CheckCode(string[] solution, ComboBox comboBox, Label colorLabel, int position)
        {
            if (comboBox.Text != null && solution.Contains(comboBox.Text) && !ColorInCorrectPosition(solution, comboBox, position))
            {
                colorLabel.BorderBrush = Brushes.Wheat;
                colorLabel.BorderThickness = new Thickness(5);
            }
            else if (ColorInCorrectPosition(solution, comboBox, position))
            {
                colorLabel.BorderBrush = Brushes.DarkRed;
                colorLabel.BorderThickness = new Thickness(5);
            }
            else
            {
                colorLabel.BorderThickness = new Thickness(0);
            }
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.F12 && !debugMode)
            {
                solutionTextBox.Visibility = Visibility.Visible;
                debugMode = true;
            }
            else if (e.KeyboardDevice.Modifiers == ModifierKeys.Control && e.Key == Key.F12 && debugMode)
            {
                solutionTextBox.Visibility = Visibility.Hidden;
                debugMode = false;
            }

        }
    }
}
