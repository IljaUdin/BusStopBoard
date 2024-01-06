using MainWindow_BusStopBoard.ViewModel;
using ModelLibrary.BusStopBoard;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace MainWindow_BusStopBoard.View
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        DataToBoard _dataToBoard;

        private DoubleAnimation _animationTicker;

        public MainWindow(ModelCollection modelCollection)
        {
            InitializeComponent();

            _dataToBoard = modelCollection.DataToBoard;

            this.DataContext = new MainViewModel(modelCollection);

            _dataToBoard.ParametersChanged += TickerTextBlockAnimation_Loaded;

            _dataToBoard.ParametersChanged += UpdateWidthColumnPicture;

            _dataToBoard.GetDataTicker += TickerTextBlockAnimation_Loaded;

            _dataToBoard.GetDataTransport += FinalStationTextBlock_Loaded;

            _dataToBoard.ParametersMediaChanged += UpdateMediaData;

            CreateMediaUpdateTimer();
        }

        /// <summary>
        /// Включить анимацию конечной станции автобуса
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FinalStationTextBlock_Loaded(object sender, EventArgs e)
        {
            TextBlock textBlock = sender as TextBlock;

            if (textBlock != null)
                if (textBlock.Text.Length > _dataToBoard.LenghtStartAnimation)
                {
                    // Создание анимации для смещения бегущей строки
                    DoubleAnimation animationFinalStation = new DoubleAnimation();
                    animationFinalStation.From = 0; // начальная позиция справа
                    animationFinalStation.To = (-1) * Math.Abs(GetTextWidth(textBlock.Text) / 2.5); // конечная позиция (смещение за пределы окна)
                    animationFinalStation.Duration = TimeSpan.FromSeconds(_dataToBoard.AnimationSpeed / 5); // скорость движения (в секундах)
                    animationFinalStation.RepeatBehavior = RepeatBehavior.Forever; // бесконечное повторение
                    animationFinalStation.AutoReverse = true; // возврат обратно вправо

                    // Применение анимации к свойству TranslateTransform
                    TranslateTransform translateTransformFinalStation = new TranslateTransform();
                    textBlock.RenderTransform = translateTransformFinalStation;
                    translateTransformFinalStation.BeginAnimation(TranslateTransform.XProperty, animationFinalStation);
                }
        }

        /// <summary>
        /// Вкл/выкл анимацию бегущей строки
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TickerTextBlockAnimation_Loaded(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                if (_dataToBoard.IsLoadTickerAnimation)
                {
                    CreatiTickerAnimation();
                }
                else
                {
                    if (_animationTicker != null)
                    {
                        // Прекращаем анимацию
                        TickerTextBlock.BeginAnimation(TranslateTransform.XProperty, null);

                        // Устанавливаем значение свойства как статичное
                        TickerTextBlock.RenderTransform = new TranslateTransform();
                        TickerTextBlock.RenderTransform.SetValue(TranslateTransform.XProperty, 0.0);

                        // Очищаем анимацию
                        _animationTicker = null;
                    }
                }
            });
        }

        /// <summary>
        /// Обновить ширину колонки c  изображением и номером автобуса
        /// </summary>
        private void UpdateWidthColumnPicture(object sender, EventArgs e)
        {
            System.Windows.Application.Current.Dispatcher.Invoke(() =>
            {
                DataGridTemplateColumn dataGridColumn = TransportEntriesList.FindName("BorderPictureColumn") as DataGridTemplateColumn;

                if (dataGridColumn != null)
                {
                    dataGridColumn.Width = 5; // требуется изменение величины ширины колонки иначе не работает режим автоматического перестроения

                    dataGridColumn.Width = DataGridLength.Auto;
                }
            });
        }

        /// <summary>
        /// Формирование анимации бегущей строки
        /// </summary>
        public void CreatiTickerAnimation()
        {
            // Создание анимации для смещения бегущей строки
            _animationTicker = new DoubleAnimation();
            _animationTicker.From = _dataToBoard.WidthWindow; // начальная позиция справа
            _animationTicker.To = -1 * (GetTextWidth(_dataToBoard.TickerText) + _dataToBoard.WidthWindow / 3); // конечная позиция (смещение за пределы окна)
            _animationTicker.Duration = TimeSpan.FromSeconds(_dataToBoard.AnimationSpeed); // скорость движения (в секундах)
            _animationTicker.RepeatBehavior = RepeatBehavior.Forever; // бесконечное повторение

            // Применение анимации к свойству TranslateTransform
            TranslateTransform translateTransformTicker = new TranslateTransform();
            TickerTextBlock.RenderTransform = translateTransformTicker;
            translateTransformTicker.BeginAnimation(TranslateTransform.XProperty, _animationTicker);
        }

        /// <summary>
        /// Определение ширины (конечной позиции) анимации
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private double GetTextWidth(string text)
        {
            if (text == null)
                text = string.Empty;

            var formattedText = new FormattedText(text,
                                                  System.Globalization.CultureInfo.CurrentCulture,
                                                  FlowDirection.LeftToRight,
                                                  new Typeface(TickerTextBlock.FontFamily, TickerTextBlock.FontStyle, TickerTextBlock.FontWeight, TickerTextBlock.FontStretch),
                                                  TickerTextBlock.FontSize,
                                                  Brushes.Black,
                                                  new NumberSubstitution(),
                                                  TextFormattingMode.Display);

            return formattedText.Width;
        }
    }
}