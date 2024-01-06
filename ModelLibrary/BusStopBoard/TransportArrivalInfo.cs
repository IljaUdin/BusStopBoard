namespace ModelLibrary.BusStopBoard
{
    public class TransportArrivalInfo
    {
        #region Поля

        private string _transportType;
        private int    _arriavalTime;
        private string _transportNumber;
        private string _finalStation;
        private string  _transportColor;
        private string _pathToPicture;

        #endregion

        #region Свойства

        /// <summary>
        /// Тип транспорта
        /// </summary>
        public string TransportType 
        { 
            get { return _transportType; } 
            set
            {
                if (_transportType != value)
                {
                    _transportType = value;
                }
            }
        
        }

        /// <summary>
        /// Номер транспорта
        /// </summary>
        public string TransportNumber
        {
            get { return _transportNumber; }
            set
            {
                if (_transportNumber != value)
                {
                    _transportNumber = value;
                }
            }
        }

        /// <summary>
        /// Конечная станция
        /// </summary>
        public string FinalStation
        {
            get { return _finalStation; }
            set
            {
                if (_finalStation != value)
                {
                    _finalStation = value;
                }
            }
        }

        /// <summary>
        /// Время пребытия
        /// </summary>
        public int ArriavalTime
        {
            get { return _arriavalTime; }
            set
            {
                if (_arriavalTime != value)
                {
                    _arriavalTime = value;
                }
            }
        }
                        
        /// <summary>
        /// Цвет типа транспорта
        /// </summary>
        public string TransportColor
        {
            get { return _transportColor; }
            set
            {
                if (_transportColor != value)
                {
                    _transportColor = value;
                }
            }
        }

        /// <summary>
        /// Путь к изображению типа транспорта
        /// </summary>
        public string PathToPicture 
        { 
            get { return _pathToPicture; }  
            set
            {
                if (_pathToPicture != value)
                {
                    _pathToPicture = value;
                }
            }
        
        }

        #endregion

        #region Методы

        public TransportArrivalInfo(string transportType, string transportNumber, string finalStation, int arriavalTime, string transportColor, string pathToPicture)
        {
            _transportType = transportType;
            _transportNumber = transportNumber;
            _finalStation = finalStation;
            _arriavalTime = arriavalTime;
            _transportColor = transportColor;
            _pathToPicture = pathToPicture;
        }

        #endregion
    }
}
