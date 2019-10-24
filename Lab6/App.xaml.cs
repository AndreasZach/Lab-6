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
            window.InitializeComponent();
            window.PauseBartenderButton.Click += PauseBartenderButton_Click;
            window.PauseWaiterButton.Click += PauseWaiterButton_Click;
            window.PauseWaiterButton.Click += PauseWaiterButton_Click1;
            window.PauseBarButton.Click += PauseBarButton_Click;
            window.OpenCloseButton.Click += OpenCloseButton_Click;
            window.SimStateComboBox.SelectionChanged += SimStateComboBox_SelectionChanged;
            window.Show();
            simManager = new SimulationManager(window);
        }

        private void SimStateComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void RefreshListBoxes()
        {
            window.BartenderListBox.Items.Refresh();
            window.WaiterListBox.Items.Refresh();
            window.PatronLisBox.Items.Refresh();
        }

        private void OpenCloseButton_Click(object sender, RoutedEventArgs e)
        {
            window.SimStateComboBox.IsEnabled = false;
            window.PauseBarButton.IsEnabled = true;
            window.PauseBartenderButton.IsEnabled = true;
            window.PauseBouncerButton.IsEnabled = true;
            window.PauseWaiterButton.IsEnabled = true;
            simManager.Run();
        }

        private void PauseBarButton_Click(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
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
