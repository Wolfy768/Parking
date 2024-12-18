using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using static System.Net.Mime.MediaTypeNames;

namespace PARKING
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private double speed = 0; // Начальная скорость
        private double maxSpeed = 5; // Максимальная скорость
        private double acceleration = 0.25; // Ускорение
        private double angle = 0; // Угол поворота
        private DispatcherTimer timer; // Таймер для обновления картинки
        private Lvl2 _mainWindow;

        public MainWindow()
        {
            InitializeComponent();
            SetupTimer(); // Настройка таймера
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
            UpdateCarRotation();
        }

        private void UpdateCarPosition()
        {
            // Логика обновления положения машинки
            double radians = angle * Math.PI / 180;
            double dx = speed * Math.Sin(radians);
            double dy = speed * Math.Cos(radians);

            if (isMovingForward)
            {
                if (speed < maxSpeed)
                {
                    speed += acceleration; // Увеличиваем скорость
                }
            }
            else if (isMovingBackward)
            {
                if (speed > -maxSpeed)
                {
                    speed -= acceleration; // Уменьшаем скорость
                }
            }
            else
            {
                // Если не движемся, замедляемся до нуля
                if (speed > 0)
                {
                    speed -= acceleration;
                }
                else if (speed < 0)
                {
                    speed += acceleration;
                }
            }
            Canvas.SetLeft(Car, Canvas.GetLeft(Car) + dx);
            Canvas.SetTop(Car, Canvas.GetTop(Car) - dy);
            if (CheckCollision(Car, lvl1, lvl11, lvl12, lvl13, lvl14, lvl141, lvl142, lvl143, lvl145, Sten1, Sten2))
            {
                Colission();
            }
        }

        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (isPaused)
            {
                return;
            }
            
            switch (e.Key)
            {
                case Key.W:
                    isMovingForward = true;
                    break;
                case Key.S:
                    isMovingBackward = true;
                    break;
                case Key.A:
                    isTurnLeft = true;
                    break;
                case Key.D:
                    isTurnRight = true;
                    break;
                case Key.Space:
                    if (parking)
                    {
                        parkovka();
                    }
                    if (speed > 0)
                    {
                        speed -= acceleration;
                    }
                    break;
            }
            CheckParking();
        }
        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.W:
                    isMovingForward = false; // Сбрасываем флаг движения вперед
                    break;
                case Key.S:
                    isMovingBackward = false; // Сбрасываем флаг движения назад
                    break;
                case Key.A:
                    isTurnLeft = false;
                    break;
                case Key.D:
                    isTurnRight = false;
                    break;
            }
        }

        private bool isMovingForward = false;
        private bool isMovingBackward = false;
        private bool isTurnLeft = false;
        private bool isTurnRight = false;
        private bool parking = false;
        private bool isPaused = false;

        private void UpdateCarRotation()
        {
            if (isTurnLeft && isMovingForward)
            {
                angle -= 0.5;
                Car.RenderTransform = new RotateTransform(angle, Car.Width / 2, Car.Height / 2);
            }
            else if (isTurnLeft && isMovingBackward)
            {
                angle += 0.5;
                Car.RenderTransform = new RotateTransform(angle, Car.Width / 2, Car.Height / 2);
            }
            else if (isTurnRight && isMovingForward)
            {
                angle += 0.5;
                Car.RenderTransform = new RotateTransform(angle, Car.Width / 2, Car.Height / 2);
            }
            else if (isTurnRight && isMovingBackward)
            {
                angle -= 0.5;
                Car.RenderTransform = new RotateTransform(angle, Car.Width / 2, Car.Height / 2);
            }
        }

        private void CheckParking()
        {
            double carLeft = Canvas.GetLeft(Car);
            double carTop = Canvas.GetTop(Car);
            double parkingLeft = Canvas.GetLeft(ParkingSpot);
            double parkingTop = Canvas.GetTop(ParkingSpot);

            if (carLeft >= parkingLeft && carLeft + Car.Width <= parkingLeft + ParkingSpot.Width && carTop >= parkingTop && carTop + Car.Height <= parkingTop + ParkingSpot.Height)
            {
                ParkingSpot.Stroke = new SolidColorBrush(Colors.Lime);
                parking = true;
            }
            else
            {
                ParkingSpot.Stroke = new SolidColorBrush(Colors.Yellow);
            }
        }

        private bool CheckCollision(UIElement element1, UIElement element2, UIElement element3, UIElement element4, UIElement element5, UIElement element6, UIElement element7, UIElement element8, UIElement element9, UIElement element10, UIElement element11, UIElement element12)
        {
            // Получаем границы первого элемента
            double left1 = Canvas.GetLeft(element1);
            double top1 = Canvas.GetTop(element1);
            Rect bounds1 = new Rect(left1, top1, ((FrameworkElement)element1).ActualWidth, ((FrameworkElement)element1).ActualHeight);

            // Получаем границы второго элемента
            double left2 = Canvas.GetLeft(element2);
            double top2 = Canvas.GetTop(element2);
            Rect bounds2 = new Rect(left2, top2, ((FrameworkElement)element2).ActualWidth, ((FrameworkElement)element2).ActualHeight);

            double left3 = Canvas.GetLeft(element3);
            double top3 = Canvas.GetTop(element3);
            Rect bounds3 = new Rect(left3, top3, ((FrameworkElement)element3).ActualWidth, ((FrameworkElement)element3).ActualHeight);

            double left4 = Canvas.GetLeft(element4);
            double top4 = Canvas.GetTop(element4);
            Rect bounds4 = new Rect(left4, top4, ((FrameworkElement)element4).ActualWidth, ((FrameworkElement)element4).ActualHeight);

            // Получаем границы второго элемента
            double left5 = Canvas.GetLeft(element5);
            double top5 = Canvas.GetTop(element5);
            Rect bounds5 = new Rect(left5, top5, ((FrameworkElement)element5).ActualWidth, ((FrameworkElement)element5).ActualHeight);

            double left6 = Canvas.GetLeft(element6);
            double top6 = Canvas.GetTop(element6);
            Rect bounds6 = new Rect(left6, top6, ((FrameworkElement)element6).ActualWidth, ((FrameworkElement)element6).ActualHeight);

            double left7 = Canvas.GetLeft(element7);
            double top7 = Canvas.GetTop(element7);
            Rect bounds7 = new Rect(left7, top7, ((FrameworkElement)element7).ActualWidth, ((FrameworkElement)element7).ActualHeight);

            // Получаем границы второго элемента
            double left8 = Canvas.GetLeft(element8);
            double top8 = Canvas.GetTop(element8);
            Rect bounds8 = new Rect(left8, top8, ((FrameworkElement)element8).ActualWidth, ((FrameworkElement)element8).ActualHeight);

            double left9 = Canvas.GetLeft(element9);
            double top9 = Canvas.GetTop(element9);
            Rect bounds9 = new Rect(left9, top9, ((FrameworkElement)element9).ActualWidth, ((FrameworkElement)element9).ActualHeight);

            double left10 = Canvas.GetLeft(element10);
            double top10 = Canvas.GetTop(element10);
            Rect bounds10 = new Rect(left10, top10, ((FrameworkElement)element10).ActualWidth, ((FrameworkElement)element10).ActualHeight);

            double left11 = Canvas.GetLeft(element11);
            double top11 = Canvas.GetTop(element11);
            Rect bounds11 = new Rect(left11, top11, ((FrameworkElement)element11).ActualWidth, ((FrameworkElement)element11).ActualHeight);

            double left12 = Canvas.GetLeft(element12);
            double top12 = Canvas.GetTop(element12);
            Rect bounds12 = new Rect(left12, top12, ((FrameworkElement)element12).ActualWidth, ((FrameworkElement)element12).ActualHeight);

            // Проверяем пересечение
            return bounds1.IntersectsWith(bounds2) || bounds1.IntersectsWith(bounds3) || bounds1.IntersectsWith(bounds4) || bounds1.IntersectsWith(bounds5) || bounds1.IntersectsWith(bounds6) || bounds1.IntersectsWith(bounds7) || bounds1.IntersectsWith(bounds8) || bounds1.IntersectsWith(bounds9) || bounds1.IntersectsWith(bounds10) || bounds1.IntersectsWith(bounds11) || bounds1.IntersectsWith(bounds12);
        }

        private void play1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rectangle1.Visibility = Visibility.Hidden;
            rectangle2.Visibility = Visibility.Hidden;
            play.Visibility = Visibility.Hidden;
            play2.Visibility = Visibility.Hidden;
            play3.Visibility = Visibility.Hidden;
            isPaused = false;
        }
        private void restart_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rectangle1.Visibility = Visibility.Hidden;
            rectangle2.Visibility = Visibility.Hidden;
            play.Visibility = Visibility.Hidden;
            play4.Visibility = Visibility.Hidden;
            play3.Visibility = Visibility.Hidden;
            speed = 0;
            angle = 0;
            Car.RenderTransform = new RotateTransform(angle);
            Canvas.SetLeft(Car, 60);
            Canvas.SetTop(Car, 650);
            isMovingForward = false;
            isMovingBackward = false;
            isTurnLeft = false;
            isTurnRight = false;
        }

        private void lvl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            parking = false;
            _mainWindow = new Lvl2();
            _mainWindow.Show();
            this.Close();
        }

        private void play2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void Colission()
        {
            rectangle1.Visibility = Visibility.Visible;
            rectangle2.Visibility = Visibility.Visible;
            play.Visibility = Visibility.Visible;
            play4.Visibility = Visibility.Visible;
            play3.Visibility = Visibility.Visible;
            speed = 0;
        }
        
        private void parkovka()
        {
            rectangle1.Visibility = Visibility.Visible;
            rectangle2.Visibility = Visibility.Visible;
            play.Visibility = Visibility.Visible;
            play2.Visibility = Visibility.Visible;
            play3.Visibility = Visibility.Visible;
            play2.MouseDown += lvl_MouseDown;
            play2.Content = "Следующий уровень";
            Canvas.SetLeft(play2, 590);
        }
        private void Button_Click(object sender, MouseButtonEventArgs e)
        {
            rectangle1.Visibility = Visibility.Visible;
            rectangle2.Visibility = Visibility.Visible;
            play.Visibility = Visibility.Visible;
            play2.Visibility = Visibility.Visible;
            play3.Visibility = Visibility.Visible;
            speed = 0;
            isPaused = true;
        }
    }
}
