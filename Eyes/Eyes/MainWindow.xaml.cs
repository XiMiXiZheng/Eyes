using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
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

namespace Eyes
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.Counter.DataContext = this;
            this.Timer.Interval = TimeSpan.FromSeconds(1);
            this.Time = TimeSpan.FromMinutes(this.IsTimeToExerciseEye() ? 2 : 1);
            this.Topmost = true;
            this.Timer.Tick += (sender, e) =>
            {
                this.Time = this.Time.Add(TimeSpan.FromSeconds(-1));
                this.TimeString = this.Time.ToString("hh\\:mm\\:ss");
                if (this.Time == TimeSpan.FromSeconds(0))
                {
                    this.WhenTimeIsOver();
                }
            };
            this.Timer.Start();
            this.Synth.SpeakAsync(this.IsTimeToExerciseEye() ? "眼保健操开始啦！" : "护眼开始啦！");
        }

        public DispatcherTimer Timer { get; set; } = new DispatcherTimer();

        public TimeSpan Time { get; set; }

        public bool IsTimeOver { get; set; }

        public SpeechSynthesizer Synth { get; set; } = new SpeechSynthesizer();

        public string TimeString
        {
            get { return (string)GetValue(TimeStringProperty); }
            set { SetValue(TimeStringProperty, value); }
        }

        // Using a DependencyProperty as the backing store for TimeString.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TimeStringProperty =
            DependencyProperty.Register("TimeString", typeof(string), typeof(MainWindow), new PropertyMetadata(string.Empty));

        private void WhenTimeIsOver()
        {
            this.Synth.Speak(this.IsTimeToExerciseEye() ? "眼保健操结束啦！" : "护眼结束啦！");
            this.IsTimeOver = true;
            this.Close();
        }

        private bool IsTimeToExerciseEye()
        {
            var now = DateTime.Now;
            return (now.Hour >= 12 && now.Minute < 30) || (now.Hour >= 15 && now.Minute < 30);
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (this.IsTimeOver == false)
                e.Cancel = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this.Timer.Stop();
        }
    }
}
