using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ProtocolLibrary.BusStopBoard;
using System.Web.UI.WebControls;
using System.IO;
using System.Net;
using System.Windows.Media;
using ModelLibrary.BusStopBoard;

namespace BusStopBoard.ViewModel
{
    public partial class SettingViewModel
    {
        #region Поля

        TcpServerConnector tcpServerConnector;
        ServerSimulation serverSimulation;

        private bool _isPresentationModeCheckBox;
        private bool _isAutoconnectServer;
        private string _dataFromServerTextBlock;
        private string _login;
        private string _password;
        private string _serverIP;
        private int _serverPort;
        private bool _serverStatus;

        StreamWriter streamReadLog;

        private TransportArrivalInfo transportArrivalInfo;
        private List<TransportArrivalInfo> TransportEntries;

        #endregion

        #region Свойства

        /// <summary>
        /// Режим презентации
        /// </summary>
        public bool IsPresentationModeCheckBox
        {
            get { return _isPresentationModeCheckBox; }
            set
            {
                if (_isPresentationModeCheckBox != value)
                {
                    _isPresentationModeCheckBox = value;
                    modelCollection.DataToBoard.IsPresentationMode = value;
                    modelCollection.DataToBoard.IsLoadTickerAnimation = value;

                    if (value == true)
                    {
                        PresentationModeOn();
                    }
                    else if (serverSimulation != null && value == false)
                    {
                        PresentationModeOff();
                    }
                    OnPropertyChanged(nameof(IsPresentationModeCheckBox));
                }
            }
        }

        /// <summary>
        /// Автоподключение к серверу
        /// </summary>
        public bool IsAutoconnectServer
        {
            get { return _isAutoconnectServer; }
            set
            {
                if (_isAutoconnectServer != value)
                {
                    _isAutoconnectServer = value;
                    Properties.Settings.Default.IsAutoconnectServer = IsAutoconnectServer;
                    OnPropertyChanged(nameof(IsAutoconnectServer));
                }
            }
        }

        /// <summary>
        /// Текст сообщения, полученный от сервера
        /// </summary>
        public string DataFromServerTextBlock
        {
            get { return _dataFromServerTextBlock; }
            set
            {
                if (_dataFromServerTextBlock != value)
                {
                    _dataFromServerTextBlock += value + "\n";

                    if (_dataFromServerTextBlock.Length > 300) //очистка буфера
                        _dataFromServerTextBlock = value + "\n";

                    OnPropertyChanged(nameof(DataFromServerTextBlock));
                }
            }
        }

        /// <summary>
        /// Логин
        /// </summary>
        public string Login
        {
            get { return _login; }
            set
            {
                if (_login != value)
                {
                    _login = value;
                    Properties.Settings.Default.Login = Login;
                    OnPropertyChanged(nameof(Login));
                }
            }
        }

        /// <summary>
        /// Пароль
        /// </summary>
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                    Properties.Settings.Default.Password = Password;
                    OnPropertyChanged(nameof(Password));
                }
            }
        }

        /// <summary>
        /// IP-адрес
        /// </summary>
        public string ServerIP
        {
            get { return _serverIP; }
            set
            {
                if (_serverIP != value)
                {
                    _serverIP = value;
                    Properties.Settings.Default.ServerIP = ServerIP;
                    OnPropertyChanged(nameof(ServerIP));
                }
            }
        }

        /// <summary>
        /// Номер порта
        /// </summary>
        public int ServerPort
        {
            get { return _serverPort; }
            set
            {
                if (_serverPort != value)
                {
                    _serverPort = value;
                    Properties.Settings.Default.ServerPort = ServerPort.ToString();
                    OnPropertyChanged(nameof(ServerPort));
                }
            }
        }

        /// <summary>
        /// Состояние подключения к серверу
        /// </summary>
        public bool ServerStatus
        {
            get { return _serverStatus; }
            set
            {
                _serverStatus = value;
                modelCollection.ConnectServer.ServerStatus = value;
                OnPropertyChanged(nameof(ServerStatus));
            }
        }

        public ICommand ConnectServerButton { get; set; }
        public ICommand DisconnectServerButton { get; set; }

        #endregion

        #region Методы

        /// <summary>
        /// Создание экземпляра класса для подключения к серверу
        /// </summary>
        public void CreateExampleConnect()
        {
            ConnectServerButton = new RelayCommand(Connect);
            DisconnectServerButton = new RelayCommand(Disconnect);
        }

        /// <summary>
        /// Вкл режим презентации
        /// </summary>
        private void PresentationModeOn()
        {
            TransportEntries = new List<TransportArrivalInfo>();
            serverSimulation = new ServerSimulation();
            serverSimulation.DataReceived += GetDataFromServer;
            serverSimulation.Connect();
        }

        /// <summary>
        ///  Выкл режим презентации
        /// </summary>
        private void PresentationModeOff()
        {
            serverSimulation.Disconnect();
            serverSimulation.DataReceived -= GetDataFromServer;
            serverSimulation = null;
        }

        /// <summary>
        /// Подключение к серверу
        /// </summary>
        public async void Connect()
        {
            if (ServerStatus != true)
            {
                ServerStatus = true;

                await Task.Run(() =>
                {
                    TransportEntries = new List<TransportArrivalInfo>();

                    tcpServerConnector = new TcpServerConnector(Login, Password, ServerIP, ServerPort);

                    tcpServerConnector.DataReceived += GetDataFromServer;

                    tcpServerConnector.DataSent += SendDataToServer;

                    tcpServerConnector.Connect();
                });
            }
        }

        /// <summary>
        /// Отключение от сервера
        /// </summary>
        public void Disconnect()
        {
            ServerStatus = false;
            modelCollection.DataToBoard.TickerText = " ";

            if (tcpServerConnector != null)
            {
                tcpServerConnector.Disconnect();
                tcpServerConnector.DataReceived -= GetDataFromServer;
                tcpServerConnector = null;
            }
        }

        /// <summary>
        /// Добавление/изменение строки прогноза прибытия маршрута в Model
        /// </summary>
        private void CreateTransportArrivalInfo(string inputString)
        {
            string transportColor;
            string pathToPicture;

            try
            {
                string[] parts = inputString.Split(';');

                if (parts.Length >= 5)
                {
                    int transportNumber = 1;
                    int transportType = 2;
                    int finalStation = 3;
                    int arriavalTime = 4;

                    FillTransportData(parts[transportType], out transportColor, out pathToPicture);

                    transportArrivalInfo = new TransportArrivalInfo(parts[transportType], parts[transportNumber], parts[finalStation], Convert.ToInt32(parts[arriavalTime]), transportColor, pathToPicture);

                    if (TransportEntries.Count == 0)
                    {
                        TransportEntries.Add(transportArrivalInfo);
                    }
                    else
                    {
                        var existingEntry = TransportEntries.FirstOrDefault(entry =>
                            entry.FinalStation == transportArrivalInfo.FinalStation && entry.TransportNumber == transportArrivalInfo.TransportNumber);

                        if (existingEntry != null)
                        {
                            existingEntry.ArriavalTime = transportArrivalInfo.ArriavalTime; // обновление времени прибытия
                        }
                        else
                        {
                            TransportEntries.Add(transportArrivalInfo); // добавление нового экземпляра транспорта
                        }
                    }

                    modelCollection.DataToBoard.TransportEntries = TransportEntries;
                }

            }
            catch { }
        }

        /// <summary>
        /// Присваивание цвета и изображения в зависимости от типа транспорта
        /// </summary>
        private void FillTransportData(string transportType, out string TransportColor, out string PathToPicture)
        {
            switch (transportType)
            {
                case "A":
                    TransportColor = modelCollection.DataToBoard.BusColor;
                    PathToPicture = modelCollection.DataToBoard.PathToBusPicture;
                    break;
                case "T":
                    TransportColor = modelCollection.DataToBoard.TrolleybusColor;
                    PathToPicture = modelCollection.DataToBoard.PathToTrolleybusPicture;
                    break;
                case "Tp":
                    TransportColor = modelCollection.DataToBoard.TramColor;
                    PathToPicture = modelCollection.DataToBoard.PathToTramPicture;
                    break;
                default:
                    TransportColor = modelCollection.DataToBoard.BusColor;
                    PathToPicture = modelCollection.DataToBoard.PathToBusPicture;
                    break;
            }
        }

        /// <summary>
        /// Запись в файл данных с сервера
        /// </summary>
        /// <param name="receivedData"></param>
        private void WriteToFile(string receivedData)
        {
            streamReadLog = new StreamWriter("DataFromServer.txt", true);
            streamReadLog?.Write($"{DateTime.Now + " " + receivedData}");
            streamReadLog.Close();
        }

        /// <summary>
        /// Событие получения данных с сервера
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetDataFromServer(object sender, EventArgs e)
        {
            string receivedData = " ";

            if (tcpServerConnector != null)
                receivedData = tcpServerConnector.DataFromServer;

            if (serverSimulation != null)
                receivedData = serverSimulation.DataFromServer;

            DataFromServerTextBlock = $"Получено: {receivedData}";

            WriteToFile(receivedData);

            switch (receivedData)
            {
                case string str when receivedData.StartsWith("t;"): //установка времени    
                    break;

                case string str when receivedData.StartsWith("m;"): //добавление/изменение строки прогноза прибытия маршрута
                    System.Windows.Application.Current.Dispatcher.Invoke(() =>
                    {
                        CreateTransportArrivalInfo(str);
                    });
                    break;

                case string str when receivedData.StartsWith("r;"): //установка рекламного текста
                    modelCollection.DataToBoard.IsLoadTickerAnimation = true;
                    modelCollection.DataToBoard.TickerText = str.Trim('r', ';', '\n');
                    break;

                case string str when receivedData.StartsWith("rs;"): //установка скорости прокрутки рекламы
                    break;

                case string str when receivedData.StartsWith("s;"): //установка стартового текста
                    break;

                case string str when receivedData.StartsWith("d;"): //установка текста по умолчанию
                    break;

                case string str when receivedData.StartsWith("ds;"): //установка текста отсутствия связи с сервером
                    break;

                case string str when receivedData.StartsWith("ms;"): //добавление/изменение строкового информационного сообщения
                    break;

                case string str when receivedData.StartsWith("mp;"): //добавление/изменение страничного информационного сообщения
                    break;

                case string str when receivedData.StartsWith("mm;"): //добавление/изменение модального информационного сообщения
                    break;

                case string str when receivedData.StartsWith("c;"): //принятие команды
                    break;
            }
        }

        /// <summary>
        /// Событие отправки данных на сервер
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SendDataToServer(object sender, EventArgs e)
        {
            if (tcpServerConnector != null)
            {
                string sentData = tcpServerConnector.DataToServer;

                DataFromServerTextBlock = $"Отправлено: {sentData}";
            }
        }

        #endregion
    }
}
