using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
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

namespace PongSample
{
    public enum Direction { Left, Right}
    public partial class MainWindow : Window
    {
        int moveDistance = 10;
        public MainWindow()
        {
            InitializeComponent();
            var width = this.Width;
            var height = this.Height;
            var left = width / 2 - ((double)Platform.GetValue(Canvas.WidthProperty) / 2);
            Canvas.SetLeft(Platform, left);

            var top = height - ((double)Platform.GetValue(Canvas.HeightProperty)) - 50;
            Canvas.SetTop(Platform, top);

            timer2.Interval = new TimeSpan(100);
            timer2.Tick += (s, args) => MovePlatform(2, direction);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(1000);
            timer.Tick += new EventHandler(KeyPressed);
            timer.Start();
        }

        int repeats = 5;
        int actualRepeat = 0;
        bool isRepeating = false;

        DispatcherTimer timer2 = new DispatcherTimer();
        Direction direction = Direction.Right;
        void KeyPressed(object source, EventArgs e)
        {  
  
            
            bool keyPressed = false;

            if (Keyboard.IsKeyDown(Key.Left))
            {
                direction = Direction.Left;
                keyPressed = true;
            }
            else if (Keyboard.IsKeyDown(Key.Right))
            {
                direction = Direction.Right ;
                keyPressed = true;
            }
            if (keyPressed)
            {
                timer2.Start();
            }
        }

        void MovePlatform(int distance, Direction direction)
        {
            actualRepeat++;
            double left = (double)Platform.GetValue(Canvas.LeftProperty);

            double distanceToMove = left;
            if (direction == Direction.Left)
            {
                distanceToMove -= distance;
            }
            else if (direction == Direction.Right)
            {
                distanceToMove += distance;
            }
            Platform.Visibility = Visibility.Hidden;
            Canvas.SetLeft(Platform, distanceToMove);
            Platform.Visibility = Visibility.Visible;
            if (actualRepeat == repeats)
            {
                timer2.Stop();
                isRepeating = false;
                actualRepeat = 0;
            }
        }
    }
}
