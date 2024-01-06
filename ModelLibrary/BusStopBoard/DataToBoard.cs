using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace ModelLibrary.BusStopBoard
{
    public partial class DataToBoard
    {
        #region Поля

        private bool _isPresentationMode;
        private string _nameBusStop;
        private int _widthWindow;
        private int _heightWindow;
        private int _fontSize;
        private int _lenghtStartAnimation;
        private int _animationSpeed;
        private int _numberBusLines;
        private int _updateRateBusLines;
        private string _fontFamily;
        private string _tickerText;
        private bool _isLoadTickerAnimation;
        private List<TransportArrivalInfo> _transportEntries;

        #endregion

        #region Свойства

        /// <summary>
        /// Режим презентации
        /// </summary>
        public bool IsPresentationMode
        {
            get { return _isPresentationMode; }
            set
            {
                if (_isPresentationMode != value)
                {
                    _isPresentationMode = value;
                    OnPresentationModeChanged();
                }
            }
        }

        /// <summary>
        /// Название остановки
        /// </summary>
        public string NameBusStop
        {
            get { return _nameBusStop; }
            set
            {
                if (_nameBusStop != value)
                {
                    _nameBusStop = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Величина ширины табло
        /// </summary>
        public int WidthWindow
        {
            get { return _widthWindow; }
            set
            {
                if (_widthWindow != value)
                {
                    _widthWindow = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Величина высоты табло
        /// </summary>
        public int HeightWindow
        {
            get { return _heightWindow; }
            set
            {
                if (_heightWindow != value)
                {
                    _heightWindow = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Размер текста
        /// </summary>
        public int FontSize
        {
            get { return _fontSize; }
            set
            {
                if (_fontSize != value)
                {
                    _fontSize = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Скорость анимации прокручивания текста
        /// </summary>
        public int AnimationSpeed
        {
            get { return _animationSpeed; }
            set
            {
                if (_animationSpeed != value)
                {
                    _animationSpeed = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Размер текста, с которого начинается анимация прокручивания автобусной остановки
        /// </summary>
        public int LenghtStartAnimation
        {
            get { return _lenghtStartAnimation; }
            set
            {
                if (_lenghtStartAnimation != value)
                {
                    _lenghtStartAnimation = value;
                    NewDataTransportChanged();
                }
            }
        }

        /// <summary>
        /// Количество строк на табло
        /// </summary>
        public int NumberBusLines
        {
            get { return _numberBusLines; }
            set
            {
                if (_numberBusLines != value)
                {
                    _numberBusLines = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Частота обновления строк с информацией об автобусах на табло
        /// </summary>
        public int UpdateRateBusLines
        {
            get { return _updateRateBusLines; }
            set
            {
                if (_updateRateBusLines != value)
                {
                    _updateRateBusLines = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Тип шрифта
        /// </summary>
        public string FontFamily
        {
            get { return _fontFamily; }
            set
            {
                if (_fontFamily != value)
                {
                    _fontFamily = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Вкл/Выкл анимацию бегущей строки
        /// </summary>
        public bool IsLoadTickerAnimation
        {
            get { return _isLoadTickerAnimation; }
            set
            {
                if (_isLoadTickerAnimation != value)
                {
                    _isLoadTickerAnimation = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Текст бегущей строки
        /// </summary>
        public string TickerText
        {
            get { return _tickerText; }
            set
            {
                if (_tickerText != value)
                {
                    _tickerText = value;
                    NewDataTickerChanged();
                }
            }
        }

        /// <summary>
        /// Список автобусных маршрутов
        /// </summary>
        public List<TransportArrivalInfo> TransportEntries
        {
            get { return _transportEntries; }
            set
            {
                //if (_transportEntries != value)
                {
                    _transportEntries = value;
                    NewDataTransportChanged();
                }
            }
        }

        #endregion

        #region Методы

        public DataToBoard()
        {
            _transportEntries = new List<TransportArrivalInfo>();
            _listMediaItems = new ObservableCollection<FileInfo>();
        }

        // Событие, которое будет вызываться при вкл/выкл режима презентации
        public event EventHandler PresentationModeChanged;

        // Метод для вызова события PresentationModeChanged
        private void OnPresentationModeChanged()
        {
            PresentationModeChanged?.Invoke(null, EventArgs.Empty);
        }

        // Событие, которое будет вызываться при изменении параметров
        public event EventHandler ParametersChanged;

        // Метод для вызова события ParametersChanged
        private void OnPropertyChanged()
        {
            ParametersChanged?.Invoke(null, EventArgs.Empty);
        }

        /// <summary>
        /// Событие обновления новых данных о транспорте
        /// </summary>
        public event EventHandler GetDataTransport;

        // Метод для вызова события GetDataTransport
        private void NewDataTransportChanged()
        {
            GetDataTransport?.Invoke(null, EventArgs.Empty);
        }

        /// <summary>
        /// Событие обновления текста в бегущей строке
        /// </summary>
        public event EventHandler GetDataTicker;

        // Метод для вызова события GetDataTicker
        private void NewDataTickerChanged()
        {
            GetDataTicker?.Invoke(null, EventArgs.Empty);
        }

        #endregion
    }
}
