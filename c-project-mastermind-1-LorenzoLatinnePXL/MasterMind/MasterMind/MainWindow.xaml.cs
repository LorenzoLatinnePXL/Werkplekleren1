﻿using System;
using System.CodeDom;
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
        ComboBox[] comboBoxes;
        Label[] labels;
        string[] colors = new string[4];
        string[] solution = new string[4];
        Brush[] solutionBrushes = new Brush[4];
        string[] options = { "Red", "Yellow", "Orange", "White", "Green", "Blue" };
        int attempts;
        int currentRow;
        bool debugMode;
        bool hasWon = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            StartGame();
        }

        private void StartGame()
        {
            // Set all values to starting values.
            attempts = 0;
            currentRow = 0;
            debugMode = false;


            // Insert all ComboBoxes into an array of ComboBoxes and generate 6 avaliable colors for each ComboBox (from the options array variable).
            comboBoxes = new ComboBox[4] { ComboBoxOption1, ComboBoxOption2, ComboBoxOption3, ComboBoxOption4 };
            comboBoxes = AddComboBoxItems(comboBoxes);

            // Insert all Labels into an array of Labels
            labels = new Label[4] { colorLabel1, colorLabel2, colorLabel3, colorLabel4, };

            // Generate random Code
            solution = InitalizeColors();

            // Convert code to brushes
            for (int i = 0; i < solution.Length; i++)
            {
                solutionBrushes[i] = (Brush)new BrushConverter().ConvertFromString(solution[i]);
            }

            // Set solution in the hidden TextBox.
            solutionTextBox.Text = String.Join(", ", solution);

            // Set all layout to starting layout.
            UpdateAttempts();
            SetAttemptLabelLayout();
            ClearComboBoxSelection(labels);
            checkButton.Content = "Check code";
        }

        private string[] InitalizeColors()
        {
            // Randomize colors.
            for (int i = 0; i < colors.Length; i++)
            {
                colors[i] = GenerateRandomColor();
            }

            return solution = new string[] { colors[0], colors[1], colors[2], colors[3] };
        }

        private void UpdateAttempts()
        {
            attemptsLabel.Content = $"Attempt: {attempts} / 10";
        }

        private void SetAttemptLabelLayout()
        {
            if (attempts >= 8)
            {
                attemptsLabel.Foreground = Brushes.Red;
                attemptsLabel.FontWeight = FontWeights.Bold;
            }
            else if (attempts >= 5)
            {
                attemptsLabel.Foreground = Brushes.Orange;
                attemptsLabel.FontWeight = FontWeights.DemiBold;
            }
            else
            {
                attemptsLabel.Foreground = Brushes.Black;
                attemptsLabel.FontWeight = FontWeights.Regular;
            }
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

        private ComboBox[] AddComboBoxItems(ComboBox[] comboBoxes)
        {
            for (int i = 0; i < comboBoxes.Length; i++)
            {
                for (int j = 0; j < options.Length; j++)
                {
                    comboBoxes[i].Items.Add(options[j]);
                }
            }
            return comboBoxes;
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
                    return Brushes.White;
            }
        }

        private void checkButton_Click(object sender, RoutedEventArgs e)
        {
            
            if (attempts + 1 != 11)
            {
                CheckIfPlayerHasWon();
                attempts++;
                CreateRow();
                UpdateAttempts();
                SetAttemptLabelLayout();

                if (attempts + 1 == 11 && !hasWon)
                {
                    checkButton.Content = "Game Over";
                    MessageBoxResult result = MessageBox.Show("Game Over, try again?", "Game over", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        ClearGridSection();
                        StartGame();
                    }
                    else
                    {
                        this.Close();
                    }
                } else if (hasWon)
                {
                    checkButton.Content = "Victory";
                    MessageBoxResult result = MessageBox.Show($"You won in {attempts} attempts, play again?", "You won", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.Yes)
                    {
                        ClearGridSection();
                        StartGame();
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
        }


        private bool ColorInCorrectPosition(Label colorLabel, int position)
        {
            if (colorLabel.Background == solutionBrushes[position])
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void CheckCode(Label colorLabel, int position)
        {
            if (colorLabel.Background == null || !solutionBrushes.Contains(colorLabel.Background))
            {
                colorLabel.BorderThickness = new Thickness(0);
            }
            else if (solutionBrushes.Contains(colorLabel.Background) && !ColorInCorrectPosition(colorLabel, position))
            {
                colorLabel.BorderBrush = Brushes.Wheat;
                colorLabel.BorderThickness = new Thickness(5);
            } 
            else
            {
                colorLabel.BorderBrush = Brushes.DarkRed;
                colorLabel.BorderThickness = new Thickness(5);
            }
        }

        private void ToggleDebug(object sender, KeyEventArgs e)
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

        private void CreateRow()
        {
            RowDefinition rowDefinition = new RowDefinition();
            rowDefinition.Height = GridLength.Auto;
            HistoryGrid.RowDefinitions.Add(rowDefinition);

            for (int i = 0; i < 4; i++)
            {
                Label playerGuess = new Label();
                playerGuess.Background = labels[i].Background;
                playerGuess.Height = 50;
                playerGuess.Width = 50;
                playerGuess.Margin = new Thickness(2);

                Grid.SetRow(playerGuess, currentRow);
                Grid.SetColumn(playerGuess, i);

                HistoryGrid.Children.Add(playerGuess);

                CheckCode(playerGuess, i);
            }

            currentRow++;
        }

        private void ClearGridSection()
        {
            HistoryGrid.RowDefinitions.Clear();
            HistoryGrid.Children.Clear();
        }

        private void ClearComboBoxSelection(Label[] labels)
        {
            for (int i = 0; i < labels.Length; i++)
            {
                comboBoxes[i].SelectedValue = null;
                labels[i].BorderBrush = null;
            }
        }

        private bool CheckIfPlayerHasWon()
        {
            if (ComboBoxOption1.Text == solution[0] &&
                ComboBoxOption2.Text == solution[1] &&
                ComboBoxOption3.Text == solution[2] &&
                ComboBoxOption4.Text == solution[3])
            {
                return hasWon = true;
            } 
            else
            {
                return hasWon = false;
            }

        }
    }

}

