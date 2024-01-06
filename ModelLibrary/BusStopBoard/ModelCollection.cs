namespace ModelLibrary.BusStopBoard
{
    /// <summary>
    /// Класс для хранения экземпляров моделей приложения
    /// </summary>
    public class ModelCollection
    {
        private DataToBoard _dataToBoard;

        private DataToConnectServer _connectServer;

        public DataToBoard DataToBoard { get { return _dataToBoard; } }

        public DataToConnectServer ConnectServer { get { return _connectServer; } }

        public ModelCollection()
        {
            _dataToBoard = new DataToBoard();

            _connectServer = new DataToConnectServer();
        }
    }
}
