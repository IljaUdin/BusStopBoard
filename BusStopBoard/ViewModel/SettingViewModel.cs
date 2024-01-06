using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MainWindow_BusStopBoard.View;
using ModelLibrary.BusStopBoard;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Drawing.Text;
using System.IO;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Media3D;

namespace BusStopBoard.ViewModel
{
    public partial class SettingViewModel : ViewModelBase, INotifyPropertyChanged
    {
        #region Поля

        ModelCollection modelCollection;
        MainWindow mainWindow;

        #endregion

        #region Свойства

        /// <summary>
        /// Закрытие программы
        /// </summary>
        public ICommand CloseProgrammCommand { get; }

        /// <summary>
        /// О программе
        /// </summary>
        public ICommand AboutProgrammCommand { get; }

        #endregion

        #region Методы

        public SettingViewModel()
        {
            ChangeParametersCommand = new RelayCommand(ChangeParameters);

            CloseProgrammCommand = new RelayCommand(CloseProgramm);

            AboutProgrammCommand = new RelayCommand(AboutProgramm);

            ChangePictureTransportCommand = new RelayCommand(ChangePictureTransport);

            AddNewMediaCommand = new RelayCommand(AddNewMedia);

            DeleteSelectedMediaCommand = new RelayCommand(DeleteSelectedMedia);

            CreateExampleConnect();

            CreateExampleMain();

            CreateExampleGraphic();

            CreateExampleMedia();

            modelCollection = new ModelCollection();

            mainWindow = new MainWindow(modelCollection);

            mainWindow.Show();

            InitializeParameters();
        }

        private void InitializeParameters()
        {
            // Connect
            IsAutoconnectServer = Properties.Settings.Default.IsAutoconnectServer;
            Login = Properties.Settings.Default.Login;
            Password = Properties.Settings.Default.Password;
            ServerIP = Properties.Settings.Default.ServerIP;
            ServerPort = Convert.ToInt32(Properties.Settings.Default.ServerPort);

            // Main
            NameBusStop = Properties.Settings.Default.NameBusStop;
            WidthWindow = Properties.Settings.Default.WidthWindow;
            HeightWindow = Properties.Settings.Default.HeightWindow;
            FontSize = Properties.Settings.Default.FontSize;
            AnimationSpeed = Properties.Settings.Default.AnimationSpeed;
            LenghtStartAnimation = Properties.Settings.Default.LenghtStartAnimation;
            NumberBusLinesValue = Properties.Settings.Default.NumberBusLinesValue;
            UpdateRateBusLinesValue = Properties.Settings.Default.UpdateRateBusLinesValue;
            FontFamily = Properties.Settings.Default.FontFamily;

            //Graphic
            StyleWindow = Properties.Settings.Default.StyleWindow;
            SelectedComponentColor = Properties.Settings.Default.SelectedComponentColor;
            ChangeColorComboBox = Properties.Settings.Default.MainTextColor;
            SelectedTransportType = Properties.Settings.Default.SelectedTransportType;

            modelCollection.DataToBoard.MainTextColor = Properties.Settings.Default.MainTextColor;
            modelCollection.DataToBoard.BusColor = Properties.Settings.Default.BusColor;
            modelCollection.DataToBoard.TrolleybusColor = Properties.Settings.Default.TrolleybusColor;
            modelCollection.DataToBoard.TramColor = Properties.Settings.Default.TramColor;

            modelCollection.DataToBoard.PathToBusPicture = Properties.Settings.Default.PathToBusPicture;
            modelCollection.DataToBoard.PathToTrolleybusPicture = Properties.Settings.Default.PathToTrolleybusPicture;
            modelCollection.DataToBoard.PathToTramPicture = Properties.Settings.Default.PathToTramPicture;

            //Media
            {
                MediaUpdateTimer = Properties.Settings.Default.MediaUpdateTimer;
                MediaDurationTimer = Properties.Settings.Default.MediaDurationTimer;

                {
                    // Чтение путей к файлам из настроек
                    System.Collections.Specialized.StringCollection filePaths = Properties.Settings.Default.ListMediaItems;

                    // Создание коллекции файлов на основе путей
                    ObservableCollection<FileInfo> myFileInfoCollection = new ObservableCollection<FileInfo>();

                    if (filePaths != null)
                        foreach (string filePath in filePaths)
                        {
                            myFileInfoCollection.Add(new FileInfo(filePath));
                        }

                    ListMediaItems = myFileInfoCollection;
                }
            }

            ChangeParameters();

            if (IsAutoconnectServer)
                Connect();
        }
                
        /// <summary>
        /// Закрытие программы
        /// </summary>
        private void CloseProgramm()
        {
            System.Windows.Application.Current.Shutdown();
        }

        /// <summary>
        /// О программе
        /// </summary>
        private void AboutProgramm()
        {
            MessageBox.Show(" Версия - 1.0\n\n Разработчик - UdinIlja\n\n Контакт для связи - scherbakov_ilja@mail.ru");
        }

        public new event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            Properties.Settings.Default.Save();
        }

        #endregion
    }
}
