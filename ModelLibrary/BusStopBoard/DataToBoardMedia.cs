using System;
using System.Collections.ObjectModel;
using System.IO;

namespace ModelLibrary.BusStopBoard
{

    public partial class DataToBoard
    {

        #region Поля

        private int _mediaUpdateTimer;
        private int _mediaDurationTimer;
        private ObservableCollection<FileInfo> _listMediaItems;

        #endregion

        #region Свойства

        /// <summary>
        /// Частота включения медиа
        /// </summary>
        public int MediaUpdateTimer
        {
            get { return _mediaUpdateTimer; }
            set
            {
                if (_mediaUpdateTimer != value && value > 0)
                {
                    _mediaUpdateTimer = value;
                    OnPropertyMediaChanged();
                }
            }
        }

        /// <summary>
        /// Длительность отображения медиа
        /// </summary>
        public int MediaDurationTimer
        {
            get { return _mediaDurationTimer; }
            set
            {
                if (_mediaDurationTimer != value && value > 0)
                {
                    _mediaDurationTimer = value;
                    OnPropertyMediaChanged();
                }
            }
        }

        /// <summary>
        /// Список медиа (видео и фото) для воспроизведения
        /// </summary>
        public ObservableCollection<FileInfo> ListMediaItems
        {
            get { return _listMediaItems; }
            set
            {
                //if (_transportEntries != value)
                {
                    _listMediaItems = value;
                    OnPropertyMediaChanged();
                }
            }
        }

        #endregion

        #region Методы

        /// <summary>
        /// Событие обновления новых данных медиа
        /// </summary>
        public event EventHandler ParametersMediaChanged;

        // Метод для вызова события ParametersMediaChanged
        private void OnPropertyMediaChanged()
        {
            ParametersMediaChanged?.Invoke(null, EventArgs.Empty);
        }

        #endregion
    }
}
