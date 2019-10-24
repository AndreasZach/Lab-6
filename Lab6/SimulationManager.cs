using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab6
{
    public class SimulationManager
    {
        public MainWindow pubWindow;

        public SimulationManager(MainWindow window)
        {
            pubWindow = window;
        }

        public void Run()
        {
            Time.SetPubHours(120);
            Time.PrintCountdown(pubWindow);
        }
        

    }
}
