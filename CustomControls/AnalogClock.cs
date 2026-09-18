using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AnalogClockControl.CustomControls
{
    public class AnalogClock : Control
    {
        private Line hourHand;
        private Line minuteHand;
        private Line secondHand;

        //give analog clock the style to define what style to use
        static AnalogClock()
        {
            //tells the constructor to override the style using existing(created) style for this clock
            DefaultStyleKeyProperty.OverrideMetadata(typeof(AnalogClock), new FrameworkPropertyMetadata(typeof(AnalogClock)));
        }
        //make clock update in real time by manipulating lines when template is applied

        public override void OnApplyTemplate()
        {
            hourHand = Template.FindName("PART_HourHand", this) as Line;
            minuteHand = Template.FindName("PART_MinuteHand", this) as Line;
            secondHand = Template.FindName("PART_SecondHand", this) as Line;

            UpdateHandAngles();

            DispatcherTimer timer = new  DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += (s, e) => UpdateHandAngles();
            timer.Start();

            base.OnApplyTemplate();
        }
        //update line angles to corresponde to the time
        private void UpdateHandAngles()
        {
            hourHand.RenderTransform = new RotateTransform((DateTime.Now.Hour / 12.0) *360, 0.5, 05);
            minuteHand.RenderTransform = new RotateTransform((DateTime.Now.Minute / 60.0) * 360, 0.5, 05);
            secondHand.RenderTransform = new RotateTransform((DateTime.Now.Second / 60.0) * 360, 0.5, 05);
        }
    }
}
