using BusStopBoard.View;
using BusStopBoard.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace BusStopBoard
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs args)
        {
            var programDefender = new ProgramDefender();

            var settingWindow = new SettingWindow();
            settingWindow.DataContext = new SettingViewModel();

            MainWindow = settingWindow;

            settingWindow.Show();

            ShutdownMode = ShutdownMode.OnMainWindowClose;
            base.OnStartup(args);
        }
    }
}
