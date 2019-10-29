using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    static public class UIUpdater
    {
        static private MainWindow window;

        static public void InitializeUpdater(MainWindow uiWindow)
        {
            window = uiWindow;
        }

        static public void UpdatePatronLabel(int currentPatrons)
        {
            window.Dispatcher.Invoke(() => window.PatronLabel.Content = $"Patrons: {currentPatrons}");
        }

        static public void UpdateChairLabel(int availableChairs)
        {
            window.Dispatcher.Invoke(() => window.ChairsLabel.Content = $"Chairs: {availableChairs}");
        }

        static public void UpdateGlassesLabel(int availableGlasses)
        {
            window.Dispatcher.Invoke(() => window.GlasesLabel.Content = $"Glasses: {availableGlasses}");
        }

        static public void LogBartenderAction(string action)
        {
            window.Dispatcher.Invoke(() =>
            {
                window.BartenderListBox.Items.Insert(0, $"<{Time.GetTimeStamp()}> {action}");
                window.BartenderListBox.Items.Refresh();
            });
        }

        static public void LogWaiterAction(string action)
        {
            window.Dispatcher.Invoke(() => 
            {
                window.WaiterListBox.Items.Insert(0, $"<{Time.GetTimeStamp()}> {action}");
                window.WaiterListBox.Items.Refresh();
            });
        }

        static public void LogPatronAction(string action)
        {
            window.Dispatcher.Invoke(() => 
            {
                window.PatronLisBox.Items.Insert(0, $"<{Time.GetTimeStamp()}> {action}");
                window.PatronLisBox.Items.Refresh();
            });
        }
    }
}
