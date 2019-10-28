using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Lab6
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private MainWindow window;
        private SimulationManager simManager;
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            window = new MainWindow();
            simManager = new SimulationManager();
            UIUpdater.InitializeUpdater(window);
            window.InitializeComponent();
            simManager.PopulateTestCollection();
            window.SimStateComboBox.ItemsSource = simManager.stateHandlers.Keys;
            window.SpeedComboBox.ItemsSource = simManager.simSpeed.Keys;
            window.PauseBartenderButton.Click += PauseBartenderButton_Click;
            window.PauseWaiterButton.Click += PauseWaiterButton_Click;
            window.PauseWaiterButton.Click += PauseWaiterButton_Click1;
            window.OpenCloseButton.Click += OpenCloseButton_Click;
            window.SimStateComboBox.SelectionChanged += SimStateComboBox_SelectionChanged;
            window.SpeedComboBox.SelectionChanged += SpeedComboBox_SelectionChanged;
            window.SpeedComboBox.SelectedItem = simManager.simSpeed.Keys.First();
            window.Show();
        }

        private void SpeedComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            simManager.SetSimulationSpeed(window.SpeedComboBox.SelectedItem);
            window.SpeedComboBox.Items.Refresh();
        }

        private void SimStateComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (window.SimStateComboBox.SelectedItem != null)
            {
                simManager.SetTestState(window.SimStateComboBox.SelectedItem);
                window.OpenCloseButton.IsEnabled = true;
            }
            else
            {
                window.OpenCloseButton.IsEnabled = false;
            }
        }

        private void OpenCloseButton_Click(object sender, RoutedEventArgs e)
        {
            window.SimStateComboBox.IsEnabled = false;
            window.PauseBartenderButton.IsEnabled = true;
            window.PauseBouncerButton.IsEnabled = true;
            window.PauseWaiterButton.IsEnabled = true;
            window.OpenCloseButton.IsEnabled = false;
            if (window.SimStateComboBox.SelectedItem.ToString() == "BusinessTimeIncrease")
            {
                Time.SetPubHours(300);
            }
            else
            {
                Time.SetPubHours(120);
            }
            simManager.Run();
            Time.PrintCountdown(window);
        }

        private void PauseWaiterButton_Click1(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PauseWaiterButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void PauseBartenderButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
