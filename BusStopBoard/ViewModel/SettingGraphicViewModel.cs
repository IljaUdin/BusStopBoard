using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media;


namespace BusStopBoard.ViewModel
{
    public partial class SettingViewModel
    {
        #region Поля

        private int _styleWindow;
        private string _selectedComponentColor;
        private string _changeColorComboBox;
        private string _selectedTransportType;
        private ObservableCollection<string> _listAvailableColors;

        #endregion

        #region Свойства

        /// <summary>
        /// Стиль табло
        /// </summary>
        public int StyleWindow
        {
            get { return _styleWindow; }
            set
            {
                if (_styleWindow != value)
                {
                    _styleWindow = value;
                    modelCollection.DataToBoard.StyleMainWindow = _styleWindow;
                    Properties.Settings.Default.StyleWindow = StyleWindow;
                    OnPropertyChanged(nameof(StyleWindow));
                }
            }
        }

        /// <summary>
        /// Выбор компонента для изменения цвета
        /// </summary>
        public string SelectedComponentColor
        {
            get { return _selectedComponentColor; }
            set
            {
                if (_selectedComponentColor != value)
                {
                    _selectedComponentColor = value;

                    switch (SelectedComponentColor)
                    {
                        case "Бегущая строка и время":
                            {
                                ChangeColorComboBox = Properties.Settings.Default.MainTextColor;
                                break;
                            }
                        case "Автобус":
                            {
                                ChangeColorComboBox = Properties.Settings.Default.BusColor;
                                break;
                            }
                        case "Троллейбус":
                            {
                                ChangeColorComboBox = Properties.Settings.Default.TrolleybusColor;
                                break;
                            }
                        case "Трамвай":
                            {
                                ChangeColorComboBox = Properties.Settings.Default.TramColor;
                                break;
                            }
                    }

                    OnPropertyChanged(nameof(SelectedComponentColor));
                }
            }
        }

        /// <summary>
        /// Изменение цвета текста
        /// </summary>
        public string ChangeColorComboBox
        {
            get { return _changeColorComboBox; }
            set
            {
                if (_changeColorComboBox != value)
                {
                    var color = (Color)ColorConverter.ConvertFromString(value);

                    _changeColorComboBox = value;

                    switch (SelectedComponentColor)
                    {
                        case "Бегущая строка и время":
                            {
                                modelCollection.DataToBoard.MainTextColor = ChangeColorComboBox;
                                Properties.Settings.Default.MainTextColor = ChangeColorComboBox;
                                OnPropertyChanged(nameof(ChangeColorComboBox));
                                break;
                            }
                        case "Автобус":
                            {
                                modelCollection.DataToBoard.BusColor = ChangeColorComboBox;
                                Properties.Settings.Default.BusColor = ChangeColorComboBox;

                                foreach (var transport in modelCollection.DataToBoard.TransportEntries)
                                {
                                    if (transport.TransportType == "A")
                                    {
                                        transport.TransportColor = _changeColorComboBox;
                                    }
                                }

                                OnPropertyChanged(nameof(ChangeColorComboBox));
                                break;
                            }
                        case "Троллейбус":
                            {
                                modelCollection.DataToBoard.TrolleybusColor = ChangeColorComboBox;
                                Properties.Settings.Default.TrolleybusColor = ChangeColorComboBox;

                                foreach (var transport in modelCollection.DataToBoard.TransportEntries)
                                {
                                    if (transport.TransportType == "T")
                                    {
                                        transport.TransportColor = _changeColorComboBox;
                                    }
                                }

                                OnPropertyChanged(nameof(ChangeColorComboBox));
                                break;
                            }
                        case "Трамвай":
                            {
                                modelCollection.DataToBoard.TramColor = ChangeColorComboBox;
                                Properties.Settings.Default.TramColor = ChangeColorComboBox;

                                foreach (var transport in modelCollection.DataToBoard.TransportEntries)
                                {
                                    if (transport.TransportType == "Tp")
                                    {
                                        transport.TransportColor = _changeColorComboBox;
                                    }
                                }

                                OnPropertyChanged(nameof(ChangeColorComboBox));
                                break;
                            }
                    }
                }
            }
        }

        /// <summary>
        /// Выбор транспортного средства для изменения рисунка
        /// </summary>
        public string SelectedTransportType
        {
            get { return _selectedTransportType; }
            set
            {
                if (_selectedTransportType != value)
                {
                    _selectedTransportType = value;
                }
            }
        }

        /// <summary>
        /// Список доступных цветов
        /// </summary>
        public ObservableCollection<string> ListAvailableColors
        {
            get { return _listAvailableColors; }
            set
            {
                _listAvailableColors = value;
                OnPropertyChanged(nameof(ListAvailableColors));
            }
        }

        /// <summary>
        /// Изменение рисунка выбранного транспортного средства
        /// </summary>
        public ICommand ChangePictureTransportCommand { get; }

        #endregion

        #region Методы

        public void CreateExampleGraphic()
        {
            FillFormColors();
        }

        /// <summary>
        /// Заполнение ComboBox списком доступных цветов
        /// </summary>
        void FillFormColors()
        {
            ListAvailableColors = new ObservableCollection<string>();

            foreach (PropertyInfo propertyInfo in typeof(Colors).GetProperties())
            {
                ListAvailableColors.Add(propertyInfo.Name);
            }
        }

        /// <summary>
        /// Изменение рисунка выбранного транспортного средства
        /// </summary>
        private void ChangePictureTransport()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG;*.ICO";

            if (openFileDialog.ShowDialog() == true)
            {
                switch (SelectedTransportType)
                {
                    case "Автобус":
                        {
                            modelCollection.DataToBoard.PathToBusPicture = openFileDialog.FileName;
                            Properties.Settings.Default.PathToBusPicture = openFileDialog.FileName;

                            foreach (var transport in modelCollection.DataToBoard.TransportEntries)
                            {
                                if (transport.TransportType == "A")
                                {
                                    transport.PathToPicture = openFileDialog.FileName;
                                }
                            }

                            break;
                        }
                    case "Троллейбус":
                        {
                            modelCollection.DataToBoard.PathToTrolleybusPicture = openFileDialog.FileName;
                            Properties.Settings.Default.PathToTrolleybusPicture = openFileDialog.FileName;

                            foreach (var transport in modelCollection.DataToBoard.TransportEntries)
                            {
                                if (transport.TransportType == "T")
                                {
                                    transport.PathToPicture = openFileDialog.FileName;
                                }
                            }

                            break;
                        }
                    case "Трамвай":
                        {
                            modelCollection.DataToBoard.PathToTramPicture = openFileDialog.FileName;
                            Properties.Settings.Default.PathToTramPicture = openFileDialog.FileName;

                            foreach (var transport in modelCollection.DataToBoard.TransportEntries)
                            {
                                if (transport.TransportType == "Tp")
                                {
                                    transport.PathToPicture = openFileDialog.FileName;
                                }
                            }

                            break;
                        }
                }

                Properties.Settings.Default.Save();
            }
        }

        #endregion
    }
}
