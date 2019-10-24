using System;
using System.Collections.Generic;
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

namespace Lab6
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            PauseBartenderButton.Click += PauseBartenderButton_Click;
            PauseWaiterButton.Click += PauseWaiterButton_Click;
            PauseWaiterButton.Click += PauseWaiterButton_Click1;
            PauseBarButton.Click += PauseBarButton_Click;
            OpenCloseButton.Click += OpenCloseButton_Click;
            SimStateComboBox.SelectionChanged += SimStateComboBox_SelectionChanged;
            // TODO: Add the rest of the methods/data needed by Agents.
            // TODO: Find a good way to implement it all Asynchronously
            // TODO: Add ways to change the total amount of glasses/chairs from the MainWindow.xaml
            // TODO: Add ways to change Agent individual action speed from the MainWindow.xaml
            // TODO: Turn off the StartURI, and create a startup event instead, in order to work Object-oriented with the MainWindow.
        }

        private void RefreshListBoxes()
        {
            BartenderListBox.Items.Refresh();
            WaiterListBox.Items.Refresh();
            PatronLisBox.Items.Refresh();
        }
        private void SimStateComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void OpenCloseButton_Click(object sender, RoutedEventArgs e)
        {
            SimStateComboBox.IsEnabled = false;
            PauseBarButton.IsEnabled = true;
            PauseBartenderButton.IsEnabled = true;
            PauseBouncerButton.IsEnabled = true;
            PauseWaiterButton.IsEnabled = true;

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

        private void SimStateComboBox_SelectionChanged_1()
        {

        }
    }
}
