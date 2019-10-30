using System.Linq;
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
            simManager.uiUpdater.InitializeUpdater(window);
            window.InitializeComponent();
            simManager.PopulateTestCollection();
            window.SimStateComboBox.ItemsSource = simManager.stateHandlers.Keys;
            window.SpeedComboBox.ItemsSource = simManager.simSpeed.Keys;
            window.OpenPubButton.Click += OpenCloseButton_Click;
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
                window.OpenPubButton.IsEnabled = true;
            }
            else
            {
                window.OpenPubButton.IsEnabled = false;
            }
        }

        private void OpenCloseButton_Click(object sender, RoutedEventArgs e)
        {
            window.SimStateComboBox.IsEnabled = false;
            window.OpenPubButton.IsEnabled = false;
            simManager.Run();
        }
    }
}
