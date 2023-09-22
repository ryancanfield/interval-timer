using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
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

namespace IntervalTimer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            SetIntervalTime();
            TickTimer.Interval = new TimeSpan(0, 0, 1);
            TickTimer.Tick += new EventHandler(TickTimer_Tick);
            TickTimer.Start();
        }

        private TimeSpan IntervalTime { get; set; }
        private DateTime EndTime { get; set; }
        DispatcherTimer TickTimer = new DispatcherTimer();
        System.Media.SoundPlayer Player = new System.Media.SoundPlayer(@"C:\Windows\Media\Alarm10.wav");
        private bool Playing = false;

        private void obj_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if ((e.Key >= Key.D1 && e.Key <= Key.D9) ||
                (e.Key >= Key.NumPad1 && e.Key <= Key.NumPad9))
                {
                    string s = e.Key.ToString();
                    s = s.Substring(s.Length-1);
                    IntervalTimeTextBox.Text = s;
                    SetIntervalTime();
                }
                if (e.Key == Key.Space)
                {
                    ResetStartTime();
                }
            }
            catch
            {
                ; // no action
            }
        }

        private void TickTimer_Tick(object sender, EventArgs e)
        {
            SetElapsedTime();
        }

        private void SetElapsedTime()
        {
            TimeSpan timeRemaining = EndTime - DateTime.Now;
            if (timeRemaining < TimeSpan.Zero)
            {
                TimeRemainingTextBox.Text = String.Format("-{0:m\\:ss}", timeRemaining);
                if (!Playing)
                {
                    Playing = true;
                    Player.PlayLooping();
                }

            }
            else
            {
                TimeRemainingTextBox.Text = String.Format("{0:m\\:ss}", timeRemaining);
                Player.Stop();
                Playing = false;
            }
        }

        private void SetIntervalTime()
        {
            IntervalTime = new TimeSpan(0, int.Parse(IntervalTimeTextBox.Text), 0);
            ResetStartTime();
            SetElapsedTime();
        }

        private void ResetStartTime()
        {
            EndTime = DateTime.Now + IntervalTime;
            SetElapsedTime();
        }
    }
}
