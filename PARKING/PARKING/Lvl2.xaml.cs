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
    /// Логика взаимодействия для Lvl2.xaml
    /// </summary>
    public partial class Lvl2 : Window
    {
        private double speed = 0; // Начальная скорость
        private double maxSpeed = 5; // Максимальная скорость
        private double acceleration = 0.25; // Ускорение
        private double angle = 0; // Угол поворота
        private DispatcherTimer timer; // Таймер для обновления картинки

        public Lvl2()
        {
            SetupTimer(); // Настройка таймера
            InitializeComponent();
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
            if (CheckCollision(Car, lvl1, lvl11, lvl12, lvl13, lvl14, lvl141, lvl142, lvl143, lvl145, Sten1, Sten2, Car2, Car3, Car4, Car5, Car6, Car7, Car8, Car9, Car10))
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
                    speed = 0;
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

        private bool CheckCollision(UIElement element1, UIElement element2, UIElement element3, UIElement element4, UIElement element5, UIElement element6, UIElement element7, UIElement element8, UIElement element9, UIElement element10, UIElement element11, UIElement element12, UIElement element13, UIElement element14, UIElement element15, UIElement element16, UIElement element17, UIElement element18, UIElement element19, UIElement element20, UIElement element21)
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
            double left13 = Canvas.GetLeft(element13);
            double top13 = Canvas.GetTop(element13);
            Rect bounds13 = new Rect(left13, top13, ((FrameworkElement)element13).ActualWidth, ((FrameworkElement)element13).ActualHeight);

            double left14 = Canvas.GetLeft(element14);
            double top14 = Canvas.GetTop(element14);
            Rect bounds14 = new Rect(left14, top14, ((FrameworkElement)element14).ActualWidth, ((FrameworkElement)element14).ActualHeight);

            double left15 = Canvas.GetLeft(element15);
            double top15 = Canvas.GetTop(element15);
            Rect bounds15 = new Rect(left15, top15, ((FrameworkElement)element15).ActualWidth, ((FrameworkElement)element15).ActualHeight);

            double left16 = Canvas.GetLeft(element16);
            double top16 = Canvas.GetTop(element16);
            Rect bounds16 = new Rect(left16, top16, ((FrameworkElement)element16).ActualWidth, ((FrameworkElement)element16).ActualHeight);

            double left17 = Canvas.GetLeft(element17);
            double top17 = Canvas.GetTop(element17);
            Rect bounds17 = new Rect(left17, top17, ((FrameworkElement)element17).ActualWidth, ((FrameworkElement)element17).ActualHeight);

            double left18 = Canvas.GetLeft(element18);
            double top18 = Canvas.GetTop(element18);
            Rect bounds18 = new Rect(left18, top18, ((FrameworkElement)element18).ActualWidth, ((FrameworkElement)element18).ActualHeight);

            double left19 = Canvas.GetLeft(element19);
            double top19 = Canvas.GetTop(element19);
            Rect bounds19 = new Rect(left19, top19, ((FrameworkElement)element19).ActualWidth, ((FrameworkElement)element19).ActualHeight);

            double left20 = Canvas.GetLeft(element20);
            double top20 = Canvas.GetTop(element20);
            Rect bounds20 = new Rect(left20, top20, ((FrameworkElement)element20).ActualWidth, ((FrameworkElement)element20).ActualHeight);

            double left21 = Canvas.GetLeft(element21);
            double top21 = Canvas.GetTop(element21);
            Rect bounds21 = new Rect(left21, top21, ((FrameworkElement)element21).ActualWidth, ((FrameworkElement)element21).ActualHeight);

            // Проверяем пересечение
            return bounds1.IntersectsWith(bounds2) || bounds1.IntersectsWith(bounds3) || bounds1.IntersectsWith(bounds4) || bounds1.IntersectsWith(bounds5) || bounds1.IntersectsWith(bounds6) || bounds1.IntersectsWith(bounds7) || bounds1.IntersectsWith(bounds8) || bounds1.IntersectsWith(bounds9) || bounds1.IntersectsWith(bounds10) || bounds1.IntersectsWith(bounds11) || bounds1.IntersectsWith(bounds12) || bounds1.IntersectsWith(bounds13) || bounds1.IntersectsWith(bounds14) || bounds1.IntersectsWith(bounds15) || bounds1.IntersectsWith(bounds16) || bounds1.IntersectsWith(bounds17) || bounds1.IntersectsWith(bounds18) || bounds1.IntersectsWith(bounds19) || bounds1.IntersectsWith(bounds20) || bounds1.IntersectsWith(bounds21);
        }

        private void play1_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rectangle1.Visibility = Visibility.Hidden;
            rectangle2.Visibility = Visibility.Hidden;
            play.Visibility = Visibility.Hidden;
            play2.Visibility = Visibility.Hidden;
            play3.Visibility = Visibility.Hidden;
            play4.Visibility = Visibility.Hidden;
            isPaused = false;
        }
        private void restart_MouseDown(object sender, MouseButtonEventArgs e)
        {
            rectangle1.Visibility = Visibility.Hidden;
            rectangle2.Visibility = Visibility.Hidden;
            play.Visibility = Visibility.Hidden;
            play2.Visibility = Visibility.Hidden;
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

        private void play2_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();
        }

        private void lvl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            parking = false;
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
            play2.Content = "Остальные уровни\n в разработке...";
            Canvas.SetLeft(play2, 600);
            Canvas.SetTop(play2, 365);
            Canvas.SetTop(play3, 520);
        }
        private void Button_Click(object sender, MouseButtonEventArgs e)
        {
            rectangle1.Visibility = Visibility.Visible;
            rectangle2.Visibility = Visibility.Visible;
            play.Visibility = Visibility.Visible;
            play2.Visibility = Visibility.Visible;
            play3.Visibility = Visibility.Visible;
            speed = 0;
            Canvas.SetLeft(play2, 653);
            isPaused = true;
        }
    }
}
