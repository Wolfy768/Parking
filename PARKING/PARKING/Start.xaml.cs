using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace PARKING
{
    /// <summary>
    /// Логика взаимодействия для Start.xaml
    /// </summary>
    public partial class Start : Window
    {
        private MainWindow _mainWindow;
        private DispatcherTimer timer;

        public Start()
        {
            InitializeComponent();
            SetupTimer();
        }
        private void SetupTimer()
        {
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(10); // Интервал обновления 100 мс
            timer.Tick += Timer_Tick; // Подписка на событие Tick
            timer.Start(); // Запуск таймера
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            UpdateCarPosition(); // Обновляем позицию машины
        }
        private void UpdateCarPosition()
        {
            Canvas.SetLeft(Car, Canvas.GetLeft(Car) + 0.5);
            double pos = Canvas.GetLeft(Car);
            if (pos > Wind.ActualWidth)
            {
                Canvas.SetLeft(Car, -30);
            }
        }


        private void Label_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _mainWindow = new MainWindow();
            _mainWindow.Show();
            this.Close();
        }

        private void Label2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }
    }
}
