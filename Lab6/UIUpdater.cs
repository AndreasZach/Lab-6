
namespace Lab6
{
    public class UIUpdater
    {
        static private MainWindow window;

        public void InitializeUpdater(MainWindow uiWindow)
        {
            window = uiWindow;
        }

        public void StartTimer()
        {
            Time.Countdown(window);
        }

        public void UpdatePatronLabel(int currentPatrons)
        {
            window.Dispatcher.Invoke(() => window.PatronLabel.Content = $"Patrons: {currentPatrons}");
        }

        public void UpdateChairLabel(int availableChairs)
        {
            window.Dispatcher.Invoke(() => window.ChairsLabel.Content = $"Free chairs: {availableChairs}");
        }

        public void UpdateGlassesLabel(int availableGlasses)
        {
            window.Dispatcher.Invoke(() => window.GlasesLabel.Content = $"Clean glasses: {availableGlasses}");
        }

        public void UpdateCountDownLabel(int testStateTime)
        {
            window.Dispatcher.Invoke(() => window.CountDownLabel.Content = $"{testStateTime} s");
        }

        public void LogBartenderAction(string action)
        {
            window.Dispatcher.Invoke(() =>
            {
                window.BartenderListBox.Items.Insert(0, $"<{Time.GetTimeStamp()}> {action}");
                window.BartenderListBox.Items.Refresh();
            });
        }

        public void LogWaiterAction(string action)
        {
            window.Dispatcher.Invoke(() => 
            {
                window.WaiterListBox.Items.Insert(0, $"<{Time.GetTimeStamp()}> {action}");
                window.WaiterListBox.Items.Refresh();
            });
        }

        public void LogPatronAction(string action)
        {
            window.Dispatcher.Invoke(() => 
            {
                window.PatronLisBox.Items.Insert(0, $"<{Time.GetTimeStamp()}> {action}");
                window.PatronLisBox.Items.Refresh();
            });
        }
    }
}
