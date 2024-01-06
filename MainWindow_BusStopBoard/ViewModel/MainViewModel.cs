using GalaSoft.MvvmLight;
using ModelLibrary.BusStopBoard;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Threading;

namespace MainWindow_BusStopBoard.ViewModel
{
    public class MainViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Поля

        DataToBoard _dataToBoard;
        private DataToConnectServer _connectServer;

        DispatcherTimer timerDecreaseTime; //Таймер уменьшения времени прибытия автобуса

        DispatcherTimer timerUpdateBoard; //Таймер обновления данных на табло

        private string _busStopName;
        private int _widthWindow;
        private int _heightWindow;
        private int _heightPicture;
        private int _fontSize;
        private string _mainTextColorMainWindow;
        private int _styleMainWindow;
        private string _timeAndTemperatureLabel;
        private string _tickerTextBlock;
        private bool _createNewRequest = true;
        private int _currentIndexOnBoard = 0;
        private ObservableCollection<TransportArrivalInfo> _transportEntries;
        private ObservableCollection<TransportArrivalInfo> _visibleTransportEntries;

        private string _font;

        #endregion

        #region Свойства

        /// <summary>
        /// Название остановки
        /// </summary>
        public string BusStopName
        {
            get { return _busStopName; }
            set
            {
                if (_busStopName != value)
                {
                    _busStopName = value;
                    OnPropertyChanged(nameof(BusStopName));
                }
            }
        }

        /// <summary>
        /// Ширина табло
        /// </summary>
        public int WidthWindow
        {
            get { return _widthWindow; }
            set
            {
                if (_widthWindow != value)
                {
                    _widthWindow = value;
                    OnPropertyChanged(nameof(WidthWindow));
                }
            }
        }
        /// <summary>
        /// Высота табло
        /// </summary>
        public int HeightWindow
        {
            get { return _heightWindow; }
            set
            {
                if (_heightWindow != value)
                {
                    _heightWindow = value;
                    OnPropertyChanged(nameof(HeightWindow));
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
                    OnPropertyChanged(nameof(FontSize));
                }
            }
        }

        /// <summary>
        /// Цвет текста
        /// </summary>
        public string MainTextColorMainWindow
        {
            get { return _mainTextColorMainWindow; }
            set
            {
                if (_mainTextColorMainWindow != value)
                {
                    _mainTextColorMainWindow = value;
                    OnPropertyChanged(nameof(MainTextColorMainWindow));
                }
            }
        }

        /// <summary>
        /// Стиль табло
        /// </summary>
        public int StyleMainWindow
        {
            get { return _styleMainWindow; }
            set
            {
                if (_styleMainWindow != value)
                {
                    _styleMainWindow = value;
                    OnPropertyChanged(nameof(StyleMainWindow));
                }
            }
        }

        /// <summary>
        /// Тип шрифта
        /// </summary>
        public string FontFamily
        {
            get { return _font; }
            set
            {
                if (_font != value)
                {
                    _font = value;
                    OnPropertyChanged(nameof(FontFamily));
                }
            }
        }

        /// <summary>
        /// Высота поля для изображения
        /// </summary>
        public int HeightPicture
        {
            get { return _heightPicture; }
            set
            {
                if (_heightPicture != value)
                {
                    _heightPicture = value;
                    OnPropertyChanged(nameof(HeightPicture));
                }
            }
        }

        /// <summary>
        /// Label вывода времени и температуры
        /// </summary>
        public string TimeAndTemperatureLabel
        {
            get { return _timeAndTemperatureLabel; }
            set
            {
                if (_timeAndTemperatureLabel != value)
                {
                    _timeAndTemperatureLabel = value;
                    OnPropertyChanged(nameof(TimeAndTemperatureLabel));
                }
            }
        }

        /// <summary>
        /// TextBlock вывода бегущей строки
        /// </summary>
        public string TickerTextBlockText
        {
            get { return _tickerTextBlock; }
            set
            {
                if (_tickerTextBlock != value)
                {
                    _tickerTextBlock = value;
                    OnPropertyChanged(nameof(TickerTextBlockText));
                }
            }
        }

        private bool _isTickerAnimationEnabled;
        public bool IsTickerAnimationEnabled
        {
            get { return _isTickerAnimationEnabled; }
            set
            {
                if (_isTickerAnimationEnabled != value)
                {
                    _isTickerAnimationEnabled = value;
                    OnPropertyChanged(nameof(IsTickerAnimationEnabled));
                }
            }
        }

        /// <summary>
        /// Список всех пребывающих маршрутов
        /// </summary>
        public ObservableCollection<TransportArrivalInfo> TransportEntries
        {
            get { return _transportEntries; }
            set
            {
                if (_transportEntries != value)
                {
                    _transportEntries = value;
                    OnPropertyChanged(nameof(TransportEntries));
                }
            }
        }

        /// <summary>
        /// Список пребывающих маршрутов, выводимых на экран
        /// </summary>
        public ObservableCollection<TransportArrivalInfo> VisibleTransportEntries
        {
            get { return _visibleTransportEntries; }
            set
            {
                if (_visibleTransportEntries != value)
                {
                    _visibleTransportEntries = value;
                    OnPropertyChanged(nameof(VisibleTransportEntries));
                }
            }
        }

        #endregion

        #region Методы
        public MainViewModel(ModelCollection modelCollection)
        {
            _dataToBoard = modelCollection.DataToBoard;
            _connectServer = modelCollection.ConnectServer;

            _transportEntries = new ObservableCollection<TransportArrivalInfo>();
            _visibleTransportEntries = new ObservableCollection<TransportArrivalInfo>();

            _dataToBoard.PresentationModeChanged += PresentationMode;

            // Подписка на событие изменения параметров размеров табло из статического класса Model
            _dataToBoard.ParametersChanged += ParametersModelChanged;

            // Подписка на событие изменение статуса подключения к серверу
            _connectServer.GetNewServerStatus += GetServerStatus;

            _dataToBoard.GetDataTicker += UpdateDataTicker;

            _dataToBoard.GetDataTransport += UpdateDataTransport;

            ParametersModelChanged(sender: 0, EventArgs.Empty);

            UpdateTimeAndTemperatureLabel();

            CreateTimerUpdateDataBoard();
        }

        /// <summary>
        /// Событие обновления новых данных о транспорте
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateDataTransport(object sender, EventArgs e)
        {
            _transportEntries.Clear();

            foreach (var item in _dataToBoard.TransportEntries)
            {
                _transportEntries.Add(item);
            }
        }

        /// <summary>
        /// Событие обновления текста в бегущей строке
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateDataTicker(object sender, EventArgs e)
        {
            TickerTextBlockText = _dataToBoard.TickerText;
        }

        /// <summary>
        /// Получение состояния подключения к серверу
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetServerStatus(object sender, EventArgs e)
        {
            TransportEntries.Clear();
            VisibleTransportEntries.Clear();
        }

        /// <summary>
        /// Обновление отображения даты, времени и погоды
        /// </summary>
        private async void UpdateTimeAndTemperatureLabel()
        {
            await Task.Run(() =>
            {
                while (true)
                {
                    TimeAndTemperatureLabel = DateTime.Now.ToString("dd.MM.yyyy");

                    Thread.Sleep(10000);

                    TimeAndTemperatureLabel = DateTime.Now.ToString("HH:mm");

                    Thread.Sleep(10000);
                }
            });
        }

        /// <summary>
        /// Обработка события запуска режима презентации
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PresentationMode(object sender, EventArgs e)
        {
            if (_dataToBoard.IsPresentationMode)
            {
                TransportEntries.Clear();
                VisibleTransportEntries.Clear();
            }
            else
            {
                TickerTextBlockText = "";
                TransportEntries.Clear();
                VisibleTransportEntries.Clear();
            }
        }

        /// <summary>
        /// Обработчик события изменения параметров
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ParametersModelChanged(object sender, EventArgs e)
        {
            BusStopName = _dataToBoard.NameBusStop;
            WidthWindow = _dataToBoard.WidthWindow;
            HeightWindow = _dataToBoard.HeightWindow;
            MainTextColorMainWindow = _dataToBoard.MainTextColor;
            FontSize = _dataToBoard.FontSize;
            HeightPicture = FontSize;
            FontFamily = _dataToBoard.FontFamily;
            StyleMainWindow = _dataToBoard.StyleMainWindow;

            if (timerUpdateBoard != null)
                timerUpdateBoard.Interval = TimeSpan.FromSeconds(_dataToBoard.UpdateRateBusLines);

            Timer_UpdateBoard(sender, e);
        }

        /// <summary>
        /// Создать таймер обновления новых данных с сервера
        /// </summary>
        private void CreateTimerUpdateDataBoard()
        {
            timerDecreaseTime = new DispatcherTimer();
            timerDecreaseTime.Interval = TimeSpan.FromSeconds(1);
            timerDecreaseTime.Tick += Timer_DecreaseTime;
            timerDecreaseTime.Start();

            timerUpdateBoard = new DispatcherTimer();
            timerUpdateBoard.Interval = TimeSpan.FromSeconds(_dataToBoard.UpdateRateBusLines);
            timerUpdateBoard.Tick += Timer_UpdateBoard;
            timerUpdateBoard.Start();
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
                if (_createNewRequest)
                {
                    _createNewRequest = false;

                    for (int i = 0; i < TransportEntries?.Count; i++)
                    {
                        if (TransportEntries[i].ArriavalTime != 0)
                            TransportEntries[i].ArriavalTime--;
                        else
                        {
                            TransportEntries.RemoveAt(i);
                        }
                    }

                    _createNewRequest = true;
                }
            });
        }

        /// <summary>
        /// Обновление списка автобусов на табло
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Timer_UpdateBoard(object sender, EventArgs e)
        {
            await Task.Run(() =>
            {
                // Вызов диспетчера для обновления UI
                System.Windows.Application.Current.Dispatcher.Invoke(() =>
                {
                    // Выбрать только элементы, у которых ArriavalTime больше 0 и не более 5 элементов
                    var validEntries = TransportEntries
                                       .Where(entry => entry.ArriavalTime > 0)
                                       .OrderBy(entry => entry.ArriavalTime)
                                       .Skip(_currentIndexOnBoard)
                                       .Take(_dataToBoard.NumberBusLines)
                                       .Select(entry => new TransportArrivalInfo(
                                               entry.TransportType,
                                               entry.TransportNumber,
                                               entry.FinalStation,
                                               entry.ArriavalTime / 60, // Преобразование из секунд в минуты
                                               entry.TransportColor,
                                               entry.PathToPicture
                                               ))
                                       .ToList();

                    //Добавление пробела для лучшего отображения цифр транспорта при темной теме
                    if (_dataToBoard.StyleMainWindow == 0)
                    {
                        foreach (var entry in validEntries)
                        {
                            entry.TransportNumber += " ";
                        }
                    }

                    VisibleTransportEntries = new ObservableCollection<TransportArrivalInfo>(validEntries);

                    _currentIndexOnBoard += _dataToBoard.NumberBusLines; // Увеличьте currentIndex для выборки следующей группы

                    if (_currentIndexOnBoard >= TransportEntries.Count || validEntries.Count == 0)
                    {
                        _currentIndexOnBoard = 0; // Возврат к началу, если достигнут конец списка
                    }
                });
            });
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
