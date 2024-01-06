using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;

namespace BusStopBoard
{
    internal class ProgramDefender
    {
        DispatcherTimer timerKillProgram;


        public DateTime dateTime = new DateTime(2024, 3, 20);

        public ProgramDefender()
        {
            timerKillProgram = new DispatcherTimer();
            timerKillProgram.Interval = TimeSpan.FromSeconds(3);
            timerKillProgram.Tick += Timer_CloseProgram;
            timerKillProgram.Start();
        }

        private void Timer_CloseProgram(object sender, EventArgs e)
        {
            if (DateTime.Now > dateTime)
            {
                // Закрываем приложение
                System.Windows.Application.Current.Shutdown();
                return;
            }
        }
    }
}
