using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
    public enum Direction { Left, Right }
    public partial class MainWindow : Window
    {
        int repeats = 5;
        int actualRepeat = 0;
        Direction direction = Direction.Right;
        Random random = new Random();
        public MainWindow()
        {
            InitializeComponent();
            var width = ParentCanvas.Width;
            var height = ParentCanvas.Height;
            var leftP = width / 2 - ((double)Platform.GetValue(Canvas.WidthProperty) / 2);
            var leftE = width / 2 - ((double)Enemy.GetValue(Canvas.WidthProperty) / 2);
            var topP = height - ((double)Platform.GetValue(Canvas.HeightProperty) * 2);

            SetPosition(Enemy, leftE, 0);
            SetPosition(Platform, leftP, topP);

            DispatcherTimer timerAnimation = new DispatcherTimer();
            timerAnimation.Interval = new TimeSpan(100);
            timerAnimation.Tick += (s, args) => MovePlatform(2, direction, timerAnimation);

            DispatcherTimer timerPressedKey = new DispatcherTimer();
            timerPressedKey.Interval = new TimeSpan(1000);
            timerPressedKey.Tick += (s, args) => KeyPressed(timerAnimation);
            timerPressedKey.Start();


            DispatcherTimer timerEnemy = new DispatcherTimer();
            timerEnemy.Interval = TimeSpan.FromMilliseconds(6);
            timerEnemy.Tick += (s, args) => MoveEnemy(timerEnemy);
            timerEnemy.Start();
        }

        void KeyPressed(DispatcherTimer timerAnimation)
        {
            bool keyPressed = false;

            if (Keyboard.IsKeyDown(Key.Left))
            {
                direction = Direction.Left;
                keyPressed = true;
            }
            else if (Keyboard.IsKeyDown(Key.Right))
            {
                direction = Direction.Right;
                keyPressed = true;
            }
            if (keyPressed)
            {
                timerAnimation.Start();
            }
        }

        void MovePlatform(int distance, Direction direction, DispatcherTimer timerAnimation)
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
            Canvas.SetLeft(Platform, distanceToMove);
            if (actualRepeat == repeats)
            {
                timerAnimation.Stop();
                actualRepeat = 0;
            }
        }

        public void MoveEnemy(DispatcherTimer timerEnemy)
        {
            double top = (double)Enemy.GetValue(Canvas.TopProperty);
            double height = ParentCanvas.Height;
            if ((top + 1) >= height)
            {
                timerEnemy.Stop();
            }
            else
            {
                Canvas.SetTop(Enemy, top + 1);
                CheckCollision();
            }
        }

        public void SetPosition(UIElement who, double left, double top)
        {
            Canvas.SetLeft(who, left);
            Canvas.SetTop(who, top);
        }

        public void CheckCollision()
        {
            double topE = (double)Enemy.GetValue(Canvas.TopProperty);
            double leftE = (double)Enemy.GetValue(Canvas.LeftProperty);

            double topP = (double)Platform.GetValue(Canvas.TopProperty);
            double leftP = (double)Platform.GetValue(Canvas.LeftProperty);

            double sizeE = Enemy.Width;

            if (topE + Enemy.Height >= topP)
            {
                if (leftP >= leftE - sizeE && leftP <= leftE + sizeE)
                {
                    MessageBox.Show("hit");
                    ResetEnemy();
                }
            }
        }

        void ResetEnemy()
        {
            var screenWidth = ParentCanvas.Width;
            var enemyWidth = ((double)Enemy.GetValue(Canvas.WidthProperty));
            var leftE = random.Next((int)(0 + enemyWidth), (int)(screenWidth - enemyWidth));
            SetPosition(Enemy, leftE, 0);
        }
    }
}
