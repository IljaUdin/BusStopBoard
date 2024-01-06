using System;
using System.Windows;
using System.Windows.Threading;

namespace MainWindow_BusStopBoard.View
{
    public partial class MainWindow
    {
        private int indexMedia;

        private DispatcherTimer _mediaUpdateTimer;
        private DispatcherTimer _mediaDurationTimer;
        private int elapsedTime;   // Прошедшее время воспроизведения

        /// <summary>
        /// Создание таймеров отображения медиа на экране
        /// </summary>
        private void CreateMediaUpdateTimer()
        {
            _mediaUpdateTimer = new DispatcherTimer();
            _mediaUpdateTimer.Tick += MediaUpdateTimer_Tick;
            _mediaUpdateTimer.Interval = TimeSpan.FromSeconds(_dataToBoard.MediaUpdateTimer); // Частота отображения медиа

            _mediaDurationTimer = new DispatcherTimer(); // Таймер длительности воспроизведения медиа
            _mediaDurationTimer.Tick += MediaDurationTimer_Tick;
            _mediaDurationTimer.Interval = TimeSpan.FromSeconds(1);

            MediaElement.MediaEnded += MediaElement_MediaEnded;
        }

        /// <summary>
        /// Таймер частоты вызова медиа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaUpdateTimer_Tick(object sender, EventArgs e)
        {
            MediaSwitchingCounter();

            string filePath = _dataToBoard.ListMediaItems[indexMedia].FullName;

            if (System.IO.File.Exists(filePath))
            {
                MediaElement.Source = new Uri(filePath, UriKind.RelativeOrAbsolute);
            }

            Application.Current.Dispatcher.Invoke(() =>
            {
                TransportEntriesList.Visibility = Visibility.Hidden;
                MediaElement.Visibility = Visibility.Visible;
            });

            elapsedTime = 0;
            _mediaDurationTimer.Start();
            MediaElement.Play();
        }

        /// <summary>
        /// Таймер длительности отображения медиа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaDurationTimer_Tick(object sender, EventArgs e)
        {
            if (elapsedTime >= _dataToBoard.MediaDurationTimer) // Длительность отображения медиа
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    TransportEntriesList.Visibility = Visibility.Visible;
                    MediaElement.Visibility = Visibility.Hidden;
                });

                elapsedTime = 0;
                MediaElement.Stop();
                _mediaDurationTimer.Stop();
            }
            else
            {
                elapsedTime++;
            }
        }

        /// <summary>
        /// Событие обновления медиа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UpdateMediaData(object sender, EventArgs e)
        {
            if (_mediaUpdateTimer != null)
                _mediaUpdateTimer.Interval = TimeSpan.FromSeconds(_dataToBoard.MediaUpdateTimer);

            if (_dataToBoard.ListMediaItems.Count == 0)
            {
                _mediaUpdateTimer.Stop();
                _mediaDurationTimer.Stop();
                MediaElement.Stop();

                Application.Current.Dispatcher.Invoke(() =>
                {
                    TransportEntriesList.Visibility = Visibility.Visible;
                    MediaElement.Visibility = Visibility.Hidden;
                });
            }
            else
                _mediaUpdateTimer.Start();
        }

        /// <summary>
        /// Событие окончание воспроизведения видео
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MediaElement_MediaEnded(object sender, RoutedEventArgs e)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                TransportEntriesList.Visibility = Visibility.Visible;
                MediaElement.Visibility = Visibility.Hidden;
            });

            elapsedTime = 0;
            MediaElement.Stop();
            _mediaDurationTimer.Stop();
        }

        /// <summary>
        /// Счетчик переключения медиа
        /// </summary>
        private void MediaSwitchingCounter()
        {
            if (_dataToBoard.ListMediaItems.Count - 1 > indexMedia)
                indexMedia++;
            else
                indexMedia = 0;
        }
    }
}
