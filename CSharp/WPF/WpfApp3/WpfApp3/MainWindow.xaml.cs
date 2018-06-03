using Microsoft.Expression.Media.Effects;
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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WpfApp3
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {

            // var ef = new SwirlEffect();
            // ef.Center = new Point(0.5, 0.5);
            // ef.TwistAmount = 50;
            // this.Effect = ef;

            var ef = new RippleEffect();
            ef.Center = new Point(0.5, 0.5);
            ef.Magnitude = 0.1;
            ef.Frequency = 100;
            this.Effect = ef;

            DoubleAnimation da = new DoubleAnimation();
            da.To = 0;
            da.Duration = TimeSpan.FromMilliseconds(3000);
            da.FillBehavior = FillBehavior.HoldEnd;

            Storyboard sb = new Storyboard();
            Storyboard.SetTarget(da, this);
            Storyboard.SetTargetProperty(da, new PropertyPath("Effect.Frequency"));
            sb.Children.Add(da);
            sb.Begin();

            await Task.Delay(3000);

            ef.Magnitude = 0;
            ef.Frequency = 0;
            this.Effect = null;
        }

        private async void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(this);

            var ef = new RippleEffect();
            ef.Center = new Point(pos.X / this.ActualWidth, pos.Y / this.ActualHeight);
            ef.Magnitude = 0.1;
            ef.Frequency = 20;
            this.Effect = ef;

            DoubleAnimation da = new DoubleAnimation();
            da.To = 0;
            da.Duration = TimeSpan.FromMilliseconds(2000);
            da.FillBehavior = FillBehavior.HoldEnd;

            Storyboard sb = new Storyboard();
            Storyboard.SetTarget(da, this);
            Storyboard.SetTargetProperty(da, new PropertyPath("Effect.Frequency"));
            sb.Children.Add(da);
            sb.Begin();

            await Task.Delay(2000);

            ef.Magnitude = 0;
            ef.Frequency = 0;
        }
    }
}
