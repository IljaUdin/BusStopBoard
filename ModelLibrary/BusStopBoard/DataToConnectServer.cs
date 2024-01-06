using System;

namespace ModelLibrary.BusStopBoard
{
    public class DataToConnectServer
    {
        #region Поля

        private string _login;
        private string _password;
        private string _serverIp;
        private int _serverPort;
        private string _dataFromServer;
        private bool _serverStatus;

        #endregion

        #region Свойства

        public string Login
        {
            get { return _login; }
            set
            {
                if (_login != value)
                {
                    _login = value;
                }
            }
        }
        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    _password = value;
                }
            }
        }

        public string ServerIp
        {
            get { return _serverIp; }
            set
            {
                if (_serverIp != value)
                {
                    _serverIp = value;
                }
            }
        }

        public int ServerPort
        {
            get { return _serverPort; }
            set
            {
                if (_serverPort != value)
                {
                    _serverPort = value;
                }
            }
        }

        /// <summary>
        /// Сообщение от сервера
        /// </summary>
        public string DataFromServer
        {
            get { return _dataFromServer; }
            set
            {
                if (_dataFromServer != value)
                {
                    _dataFromServer = value;
                    OnDataFromServerChanged();
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
                OnServerStatusChanged();
            }
        }

        #endregion

        #region Методы

        // Событие, которое будет вызываться при изменении параметров данных подключения к серверу
        public event EventHandler GetNewDataEvent;

        // Метод для вызова события
        private void OnDataFromServerChanged()
        {
            GetNewDataEvent?.Invoke(null, EventArgs.Empty);
        }

        // Событие, которое будет вызываться при изменении статуса подключения к серверу
        public event EventHandler GetNewServerStatus;

        // Метод для вызова события
        private void OnServerStatusChanged()
        {
            GetNewServerStatus?.Invoke(null, EventArgs.Empty);
        }

        #endregion
    }
}
