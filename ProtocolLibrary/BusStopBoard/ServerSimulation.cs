using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Threading;

namespace ProtocolLibrary.BusStopBoard
{
    public class ServerSimulation
    {
        #region Поля

        DispatcherTimer timerGetNewData;
        int UpdateNewDataSeconds = 2;

        DispatcherTimer timerDecreaseTime; //Таймер уменьшения времени прибытия
        int UpdateDecreaseTimeSeconds = 1;

        private int step;
        private string _dataFromServer;

        List<string> TransportEntries = new List<string>
            {
                "m;1;T;ex1Восточная;100;;\n",
                "r;Табло работает в тестовом режиме!\n",
                "m;11;A;ex1Областная клиническая больница;150;;\n",
                "m;2;T;ex1площадь Южная;200;;\n",
                "m;2;Tp;ex1Батенькова;250;;\n",
                "m;37;A;ex1Кольцевая Алтайская;200;;\n",
                "m;5;Tp;ex1площадь Южная;500;;\n",
                "m;3;T;ex1Восточная;500;;\n",
                "m;401;A;ex1Северск;500;;\n",
                "m;7;Tp;ex1Томск;500;;\n",
                "m;9;A;ex1Демьяна бедного;500;;\n"
            };

        #endregion

        #region Свойства

        /// <summary>
        /// Данные с сервера
        /// </summary>
        public string DataFromServer
        {
            get { return _dataFromServer; }
            set
            {
                _dataFromServer = value;
                OnDataReceived(nameof(DataFromServer));
            }
        }

        #endregion

        #region

        public void Connect()
        {
            timerGetNewData = new DispatcherTimer();
            timerGetNewData.Interval = TimeSpan.FromSeconds(UpdateNewDataSeconds);
            timerGetNewData.Tick += Timer_NewData;
            timerGetNewData.Start();

            timerDecreaseTime = new DispatcherTimer();
            timerDecreaseTime.Interval = TimeSpan.FromSeconds(UpdateDecreaseTimeSeconds);
            timerDecreaseTime.Tick += Timer_DecreaseTime;
            timerDecreaseTime.Start();
        }

        public void Disconnect()
        {
            timerGetNewData.Stop();
            timerGetNewData.Tick -= Timer_NewData;

            timerDecreaseTime.Stop();
            timerDecreaseTime.Tick -= Timer_DecreaseTime;

            TransportEntries.Clear();
        }

        /// <summary>
        /// Таймер уменьшения времени прибытия автобуса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Timer_DecreaseTime(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                for (int i = 0; i < TransportEntries?.Count; i++)
                {
                    if (TransportEntries[i].StartsWith("m;"))
                    {
                        string[] parts = TransportEntries[i].Split(';');

                        int arriavalTimeIndex = 4;

                        int arriavalTime = Convert.ToInt16(parts[arriavalTimeIndex]);

                        if (arriavalTime > 0)
                        {
                            parts[arriavalTimeIndex] = (arriavalTime - 5).ToString();
                        }
                        else
                        {
                            parts[arriavalTimeIndex] = (300).ToString(); // если время вышло - выставляем новое время прибытия

                        }

                        string updateParts = string.Join(";", parts);

                        TransportEntries[i] = updateParts;
                    }
                }
            });

        }

        private async  void Timer_NewData(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                DataFromServer = TransportEntries[step];

                if (step > 8)
                    step = 0;
                else step++;
            });
        }

        /// <summary>
        /// Событие, которое сигнализирует о получении данных от сервера
        /// </summary>
        public event PropertyChangedEventHandler DataReceived;

        protected virtual void OnDataReceived(string propertyName)
        {
            DataReceived?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
