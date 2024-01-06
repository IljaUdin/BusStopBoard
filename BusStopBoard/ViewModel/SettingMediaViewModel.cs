using Microsoft.Win32;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows.Input;

namespace BusStopBoard.ViewModel
{
    public partial class SettingViewModel
    {
        // Медиа - видео и фото

        #region Поля

        private int _mediaUpdateTimer;
        private int _mediaDurationTimer;
        private string _selectMediaItem;

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
                    modelCollection.DataToBoard.MediaUpdateTimer = value;
                    Properties.Settings.Default.MediaUpdateTimer = value;
                    OnPropertyChanged(nameof(MediaUpdateTimer));
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
                    modelCollection.DataToBoard.MediaDurationTimer = value;
                    Properties.Settings.Default.MediaDurationTimer = value;
                    OnPropertyChanged(nameof(MediaDurationTimer));
                }
            }
        }

        /// <summary>
        /// Выбранный элемент в списке медиа
        /// </summary>
        public string SelectMediaItem
        {
            get { return _selectMediaItem; }
            set
            {
                if (_selectMediaItem != value)
                {
                    _selectMediaItem = value;
                    OnPropertyChanged(nameof(_selectMediaItem));
                }
            }
        }

        /// <summary>
        /// Список медиа
        /// </summary>
        public ObservableCollection<FileInfo> ListMediaItems
        {
            get { return _listMediaItems; }
            set
            {
                _listMediaItems = value;
                modelCollection.DataToBoard.ListMediaItems = _listMediaItems;
                OnPropertyChanged(nameof(ListMediaItems));
            }
        }

        /// <summary>
        /// Добавить новый элемент в медиа
        /// </summary>
        public ICommand AddNewMediaCommand { get; }

        /// <summary>
        /// Удаление выбранного элемента из списка медиа
        /// </summary>
        public ICommand DeleteSelectedMediaCommand { get; }

        #endregion

        #region Методы

        private void CreateExampleMedia()
        {
            _listMediaItems = new ObservableCollection<FileInfo>();
        }

        /// <summary>
        /// Добавить новый элемент в медиа
        /// </summary>
        private void AddNewMedia()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Video Files(*.AVI;*.MP4;*.MKV;*.MOV)|*.AVI;*.MP4;*.MKV;*.MOV |Image Files(*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG;*.ICO ";

            if (openFileDialog.ShowDialog() == true)
            {
                //string fileName = System.IO.Path.GetFileName(openFileDialog.FileName);
                FileInfo selectedFile = new FileInfo(openFileDialog.FileName);

                if (!ListMediaItems.Contains(selectedFile))
                {
                    ListMediaItems.Add(selectedFile);
                    modelCollection.DataToBoard.ListMediaItems = ListMediaItems;

                    System.Collections.Specialized.StringCollection filePaths = new System.Collections.Specialized.StringCollection();
                    foreach (FileInfo fileInfo in ListMediaItems)
                    {
                        filePaths.Add(fileInfo.FullName);
                    }
                    Properties.Settings.Default.ListMediaItems = filePaths;
                    Properties.Settings.Default.Save();
                }
            }
        }

        /// <summary>
        /// Удалить выбранный элемент медиа
        /// </summary>
        private void DeleteSelectedMedia()
        {
            var selectedFile = ListMediaItems.FirstOrDefault(entry => entry.FullName == SelectMediaItem);

            if (selectedFile != null)
            {
                ListMediaItems.Remove(selectedFile);
                modelCollection.DataToBoard.ListMediaItems = ListMediaItems;

                System.Collections.Specialized.StringCollection filePaths = new System.Collections.Specialized.StringCollection();
                foreach (FileInfo fileInfo in ListMediaItems)
                {
                    filePaths.Add(fileInfo.FullName);
                }
                Properties.Settings.Default.ListMediaItems = filePaths;
                Properties.Settings.Default.Save();
            }
        }

        #endregion
    }
}
