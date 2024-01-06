using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;

namespace ProtocolLibrary.BusStopBoard
{
    public class TcpServerConnector
    {
        #region Поля

        private string login;
        private string password;
        private string serverIp;
        private int serverPort;

        private string _dataFromServer;
        private string _dataToServer;

        private System.Timers.Timer pingTimer;
        private TcpClient client;
        private CancellationTokenSource cancellationTokenSource;

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

        /// <summary>
        /// Данные на сервер
        /// </summary>
        public string DataToServer
        {
            get { return _dataToServer; }
            set
            {
                _dataToServer = value;
                OnDataSent(nameof(DataToServer));
            }
        }

        #endregion

        #region Методы

        public TcpServerConnector(string login, string password, string serverIp, int serverPort)
        {
            this.login = login;
            this.password = password;
            this.serverIp = serverIp;
            this.serverPort = serverPort;

            cancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Подключение к серверу
        /// </summary>
        public void Connect()
        {
            try
            {
                client = new TcpClient(serverIp, serverPort);
                NetworkStream networkStream = client.GetStream();

                string login_Password = $"l;{login};{password}";

                WriteCommand(login_Password);

                CreatePingTimer();

                Task.Run(() => ReceiveDataFromServer(networkStream, cancellationTokenSource.Token));
            }
            catch (Exception ex)
            {
                DataFromServer = "Произошла ошибка: " + ex.Message;
            }
        }

        /// <summary>
        /// Отключение от сервера
        /// </summary>
        public void Disconnect()
        {
            cancellationTokenSource.Cancel();

            if (pingTimer != null)
            {
                pingTimer.Stop();
                pingTimer.Elapsed -= OnPingTimerElapsed;
            }
        }

        /// <summary>
        /// Получение данных с сервера
        /// </summary>
        private void ReceiveDataFromServer(NetworkStream networkStream, CancellationToken cancellationToken)
        {
            byte[] buffer = new byte[1024];

            while (!cancellationToken.IsCancellationRequested)
            {
                int bytesRead = networkStream.Read(buffer, 0, buffer.Length);

                if (bytesRead > 0)
                {
                    string receivedData = Encoding.GetEncoding("Windows-1251").GetString(buffer, 0, bytesRead);

                    DataFromServer = receivedData;

                    VerificationReceivedInformation(receivedData);
                }
            }
        }

        /// <summary>
        /// Подтверждение принятых данных
        /// </summary>
        /// <param name="receivedData"></param>
        private void VerificationReceivedInformation(string receivedData)
        {
            switch (receivedData)
            {
                case string str when receivedData.StartsWith("t;"):
                    WriteCommand("rt"); //установка времени 
                    break;

                case string str when receivedData.StartsWith("m;"):
                    WriteCommand("rm"); //добавление/изменение строки прогноза прибытия маршрута
                    break;

                case string str when receivedData.StartsWith("r;"):
                    WriteCommand("rr"); //установка рекламного текста
                    break;

                case string str when receivedData.StartsWith("rs;"):
                    WriteCommand("rrs"); //установка скорости прокрутки рекламы
                    break;

                case string str when receivedData.StartsWith("s;"):
                    WriteCommand("rs"); //установка стартового текста
                    break;

                case string str when receivedData.StartsWith("d;"):
                    WriteCommand("rd"); //установка текста по умолчанию
                    break;

                case string str when receivedData.StartsWith("ds;"):
                    WriteCommand("rds"); //установка текста отсутствия связи с сервером
                    break;

                case string str when receivedData.StartsWith("ms;"):
                    WriteCommand("rms"); //добавление/изменение строкового информационного сообщения
                    break;

                case string str when receivedData.StartsWith("mp;"):
                    WriteCommand("rmp"); //добавление/изменение страничного информационного сообщения
                    break;

                case string str when receivedData.StartsWith("mm;"):
                    WriteCommand("rmm"); //добавление/изменение модального информационного сообщения
                    break;

                case string str when receivedData.StartsWith("c;"):
                    WriteCommand("rc"); //принятие команды
                    break;
            }
        }

        /// <summary>
        /// Отправка на сервер ответа о приеме данных
        /// </summary>
        /// <param name="command"></param>
        private void WriteCommand(string command)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes($"{command}\n");
                NetworkStream networkStream = client.GetStream();
                networkStream.Write(buffer, 0, buffer.Length);
                networkStream.Flush();
                DataToServer = $"{command}";
            }
            catch (Exception ex)
            {
                DataToServer = $"Ошибка при отправке {command}: " + ex.Message;
            }
        }

        /// <summary>
        /// Создаем и настраиваем таймер для отправки "ping" каждые 30 секунд
        /// </summary>
        private void CreatePingTimer()
        {
            pingTimer = new System.Timers.Timer(30000); // 30 секунд в миллисекундах
            pingTimer.Elapsed += OnPingTimerElapsed;
            pingTimer.AutoReset = true;
            pingTimer.Start();
        }

        private void OnPingTimerElapsed(object sender, ElapsedEventArgs e)
        {
            string ping = "ping";

            pingTimer.Stop(); // Останавливаем таймер, чтобы избежать отправки "ping" до завершения предыдущей операции
            WriteCommand(ping);
            pingTimer.Start(); // Возобновляем таймер
        }

        /// <summary>
        /// Событие, которое сигнализирует о получении данных от сервера
        /// </summary>
        public event PropertyChangedEventHandler DataReceived;

        protected virtual void OnDataReceived(string propertyName)
        {
            DataReceived?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        /// <summary>
        /// Событие, которое сигнализирует об отправке данных на сервер
        /// </summary>
        public event PropertyChangedEventHandler DataSent;

        protected virtual void OnDataSent(string propertyName)
        {
            DataSent?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
