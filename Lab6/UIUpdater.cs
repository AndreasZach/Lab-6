using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Lab6
{
    public class UIUpdater
    {
        private MainWindow window;
        public event Action CountdownComplete;
        public bool StopCountdown;

        public void InitializeUpdater(MainWindow uiWindow)
        {
            window = uiWindow;
        }

        public void StartTimer()
        {
            Task.Run(() =>
            {
                bool countdownSubZero = false;
                while (!StopCountdown)
                {
                    double countdown = Time.Countdown();
                    if (countdown >= 0)
                    {
                        window.Dispatcher.Invoke(() => window.CountDownLabel.Content = $"{(int)countdown} s");
                    }
                    if (!countdownSubZero && countdown < 0)
                    {
                        window.Dispatcher.Invoke(() => window.CountDownLabel.Content = $"Pub Closing");
                        CountdownComplete();
                        countdownSubZero = true;
                    }
                    Thread.Sleep(100);
                }
            });
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

        public void ShowEndMessage()
        {
            window.Dispatcher.Invoke(() => window.CountDownLabel.Content = "Pub Closed");
            MessageBox.Show("Pub is now closed,\nplease leave the pub!", "Waiter:");
        }
    }
}
