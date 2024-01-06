using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BusStopBoard.ViewModel
{
    public partial class SettingViewModel
    {
        #region Поля

        private string _nameBusStop;
        private int _widthWindow;
        private int _heightWindow;
        private int _fontSize;
        private int _animationSpeed;
        private int _lenghtStartAnimation;
        private int _numberBusLinesValue;
        private int _updateRateBusLinesValue;
        private string _fontFamily;
        private ObservableCollection<string> _listAvailableFonts;

        #endregion

        #region Свойства

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
                    Properties.Settings.Default.NameBusStop = NameBusStop;
                    OnPropertyChanged(nameof(NameBusStop));
                }
            }
        }

        /// <summary>
        /// Значение ширины табло
        /// </summary>
        public int WidthWindow
        {
            get { return _widthWindow; }
            set
            {
                if (_widthWindow != value)
                {
                    _widthWindow = value;
                    Properties.Settings.Default.WidthWindow = WidthWindow;
                    OnPropertyChanged(nameof(WidthWindow));
                }
            }
        }

        /// <summary>
        /// Значение высоты табло
        /// </summary>
        public int HeightWindow
        {
            get { return _heightWindow; }
            set
            {
                if (_heightWindow != value)
                {
                    _heightWindow = value;
                    Properties.Settings.Default.HeightWindow = HeightWindow;
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
                    Properties.Settings.Default.FontSize = FontSize;
                    OnPropertyChanged(nameof(FontSize));
                }
            }
        }

        /// <summary>
        /// Скорость анимации прокрутки текста
        /// </summary>
        public int AnimationSpeed
        {
            get { return _animationSpeed; }
            set
            {
                if (_animationSpeed != value)

                {
                    _animationSpeed = value;
                    modelCollection.DataToBoard.AnimationSpeed = value;
                    Properties.Settings.Default.AnimationSpeed = AnimationSpeed;
                    OnPropertyChanged(nameof(AnimationSpeed));
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
                    Properties.Settings.Default.LenghtStartAnimation = LenghtStartAnimation;
                    OnPropertyChanged(nameof(LenghtStartAnimation));
                }
            }
        }

        /// <summary>
        /// Количество строк на табло
        /// </summary>
        public int NumberBusLinesValue
        {
            get { return _numberBusLinesValue; }
            set
            {
                if (_numberBusLinesValue != value)
                {
                    _numberBusLinesValue = value;
                    modelCollection.DataToBoard.NumberBusLines = value;
                    OnPropertyChanged(nameof(NumberBusLinesValue));
                }
            }
        }

        /// <summary>
        /// Частота обновления строк с информацией об автобусах на табло
        /// </summary>
        public int UpdateRateBusLinesValue
        {
            get { return _updateRateBusLinesValue; }
            set
            {
                if (_updateRateBusLinesValue != value)
                {
                    _updateRateBusLinesValue = value;
                    Properties.Settings.Default.UpdateRateBusLinesValue = UpdateRateBusLinesValue;
                    OnPropertyChanged(nameof(UpdateRateBusLinesValue));
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
                    Properties.Settings.Default.FontFamily = FontFamily;
                    OnPropertyChanged(nameof(FontFamily));
                }
            }
        }

        /// <summary>
        /// Список доступных шрифтов
        /// </summary>
        public ObservableCollection<string> ListAvailableFonts
        {
            get { return _listAvailableFonts; }
            set
            {
                _listAvailableFonts = value;
                OnPropertyChanged(nameof(ListAvailableFonts));
            }
        }

        /// <summary>
        /// Изменение ширины и высоты табло
        /// </summary>
        public ICommand ChangeParametersCommand { get; }

        #endregion

        #region Методы

        private void CreateExampleMain()
        {
            FillFormFont();
        }

        /// <summary>
        /// Изменение параметров табло
        /// </summary>
        private void ChangeParameters()
        {
            if (NameBusStop != null &&
                WidthWindow > 0 &&
                HeightWindow > 0 &&
                FontSize > 0 &&
                AnimationSpeed > 0)
            {
                modelCollection.DataToBoard.NameBusStop = NameBusStop;

                modelCollection.DataToBoard.WidthWindow = WidthWindow;

                modelCollection.DataToBoard.HeightWindow = HeightWindow;

                modelCollection.DataToBoard.FontSize = FontSize;

                modelCollection.DataToBoard.AnimationSpeed = AnimationSpeed;

                modelCollection.DataToBoard.LenghtStartAnimation = LenghtStartAnimation;

                modelCollection.DataToBoard.UpdateRateBusLines = UpdateRateBusLinesValue;

                modelCollection.DataToBoard.FontFamily = FontFamily;
            }
        }

        /// <summary>
        /// Заполнение ComboBox списком доступных шрифтов
        /// </summary>
        void FillFormFont()
        {
            ListAvailableFonts = new ObservableCollection<string>();

            // Получить список всех доступных семейств шрифтов на компьютере
            InstalledFontCollection installedFonts = new InstalledFontCollection();
            System.Drawing.FontFamily[] fontFamilies = installedFonts.Families;

            // Добавить имена семейств шрифтов в ComboBox
            foreach (System.Drawing.FontFamily fontFamily in fontFamilies)
            {
                ListAvailableFonts.Add(fontFamily.Name);
            }
        }

        #endregion
    }
}
