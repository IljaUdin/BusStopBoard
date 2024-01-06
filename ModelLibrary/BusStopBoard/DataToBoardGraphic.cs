using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLibrary.BusStopBoard
{
    public partial class DataToBoard
    {
        #region Поля

        private int _styleMainWindow;
        private string _mainTextColor;
        private string _busColor;
        private string _trolleybusColor;
        private string _tramColor;
        private string _pathToBusPicture; 
        private string _pathToTrolleybusPicture;
        private string _pathToTramPicture;

        #endregion

        #region Свойства

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
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Цвет основного текста
        /// </summary>
        public string MainTextColor
        {
            get { return _mainTextColor; }
            set
            {
                if (_mainTextColor != value)
                {
                    _mainTextColor = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Цвет строки автобуса
        /// </summary>
        public string BusColor
        {
            get { return _busColor; }
            set
            {
                if (_busColor != value)
                {
                    _busColor = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Цвет строки троллейбуса
        /// </summary>
        public string TrolleybusColor
        {
            get { return _trolleybusColor; }
            set
            {
                if (_trolleybusColor != value)
                {
                    _trolleybusColor = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Цвет строки трамвая
        /// </summary>
        public string TramColor
        {
            get { return _tramColor; }
            set
            {
                if (_tramColor != value)
                {
                    _tramColor = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Расположение изображения автобуса
        /// </summary>
        public string PathToBusPicture
        {
            get { return _pathToBusPicture; }
            set
            {
                if (_pathToBusPicture != value)
                {
                    _pathToBusPicture = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Расположение изображения троллейбуса
        /// </summary>
        public string PathToTrolleybusPicture
        {
            get { return _pathToTrolleybusPicture; }
            set
            {
                if (_pathToTrolleybusPicture != value)
                {
                    _pathToTrolleybusPicture = value;
                    OnPropertyChanged();
                }
            }
        }

        /// <summary>
        /// Расположение изображения трамвая
        /// </summary>
        public string PathToTramPicture
        {
            get { return _pathToTramPicture; }
            set
            {
                if (_pathToTramPicture != value)
                {
                    _pathToTramPicture = value;
                    OnPropertyChanged();
                }
            }
        }

        #endregion

        #region Методы



        #endregion
    }
}
