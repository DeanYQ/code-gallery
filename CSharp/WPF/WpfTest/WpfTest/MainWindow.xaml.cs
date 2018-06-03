using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;

namespace WpfTest
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
        //保存引用的元素
        private FrameworkElement  reflectorElement;
        private FrameworkElement originElement;
        private MediaClock clock;

		public MainWindow()
		{
			this.InitializeComponent();
            reflectorElement = reflector;
            originElement = orgin;
            MediaTimeline  timeline =new MediaTimeline(new Uri("bg_sky.mp4", UriKind.RelativeOrAbsolute));
            clock = timeline.CreateClock();//创建控制时钟

            start.IsEnabled = false ;
            pause.IsEnabled = false;
            resume.IsEnabled = false;
            stop.IsEnabled = false;
		}

		private void tme_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
            Reset();

            MediaElement mediaElement = Resources["video"] as MediaElement;//得到资源
            orgin.Child = mediaElement;

            mediaElement.Clock = clock;
            clock.Controller.Seek(new TimeSpan(0, 0, 0, 2), TimeSeekOrigin.BeginTime);//跳过固定的时间线

            //以下是联合VisualBrush和MediaElement的设置
            VisualBrush brush = new VisualBrush();
            brush.Visual = mediaElement;//使用MediaElement对象
            brush.RelativeTransform = new ScaleTransform { ScaleY = -1, CenterY = 0.5 };//通过VisualBrush对象进行布局控制

            LinearGradientBrush maskBrush = new LinearGradientBrush { StartPoint = new Point(0, 0), EndPoint = new Point(0, 1) };
            GradientStop stopOne = new GradientStop { Color = Colors.Black, Offset = 0 };
            GradientStop stopTwo = new GradientStop { Color = Colors.Transparent, Offset = 0.875 };
            maskBrush.GradientStops.Add(stopOne);
            maskBrush.GradientStops.Add(stopTwo);//生成掩码的对象

            reflector.Fill = brush;
            reflector.OpacityMask = maskBrush;
            clock.Controller.Stop();

            start.IsEnabled = true;
            pause.IsEnabled = false;
            resume.IsEnabled = false;
            stop.IsEnabled = false;
		}

		private void tmp_Click(object sender, System.Windows.RoutedEventArgs e)
		{
			// TODO: Add event handler implementation here.
           
            // Insert code required on object creation below this point.
            Reset();

            reflector.LayoutTransform = new ScaleTransform { ScaleY = -1, CenterY = 0.5 };//设置自己的布局

            LinearGradientBrush maskBrush = new LinearGradientBrush { StartPoint = new Point(0, 0), EndPoint = new Point(0, 1) };
            GradientStop stopOne = new GradientStop { Color = Colors.Black, Offset = 0.875 };
            GradientStop stopTwo = new GradientStop { Color = Colors.Transparent, Offset = 0 };
            maskBrush.GradientStops.Add(stopOne);
            maskBrush.GradientStops.Add(stopTwo);

            reflector.OpacityMask = maskBrush;

            //以下是联合VideoDrawing和MediaPlayer的设置
            MediaPlayer player = new MediaPlayer();//使用MediaPlayer对象
            player.Clock = clock;

            VideoDrawing drawing = new VideoDrawing();
            drawing.Rect = new Rect(150, 0, 100, 100);
            drawing.Player = player;//将MediaPlayer赋给VideoDrawing
            DrawingBrush brush = new DrawingBrush(drawing);//得到使用的绘图画刷

            Rectangle border = new Rectangle();
            border.Stretch = Stretch.Fill;
            orgin.Child = border;
            border.Fill = brush;
            orgin.Child = border;

            reflector.Fill = brush;
            clock.Controller.Stop();

            start.IsEnabled = true;
            pause.IsEnabled = false;
            resume.IsEnabled = false;
            stop.IsEnabled = false;
		}

        private void  Reset()
        {
            reflectorElement.OpacityMask = null;
            reflectorElement.LayoutTransform = null;
        }

        private void start_Click(object sender, RoutedEventArgs e)
        {
            clock.Controller.Stop();
            clock.Controller.Begin();

            pause.IsEnabled =true ;
            resume.IsEnabled = false  ;
            stop.IsEnabled = true ;
            start.IsEnabled = false;
        }

        private void stop_Click(object sender, RoutedEventArgs e)
        {
            clock.Controller.Stop();

            start.IsEnabled = true;
            pause.IsEnabled = false;
            resume.IsEnabled = false;
            stop.IsEnabled = false;
        }

        private void resume_Click(object sender, RoutedEventArgs e)
        {
            clock.Controller.Resume();

            start.IsEnabled = false;
            pause.IsEnabled = true ;
            resume.IsEnabled = true ;
            stop.IsEnabled = true ;
        }

        private void pause_Click(object sender, RoutedEventArgs e)
        {
            clock.Controller.Pause();

            start.IsEnabled = false ;
            pause.IsEnabled = false;
            resume.IsEnabled = true ;
            stop.IsEnabled = false;
        }
	}
}