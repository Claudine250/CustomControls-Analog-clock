using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
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

        //register dependency property with analog clock to set up whether its shown or hidden, defaulted to true
        public static DependencyProperty ShowSecondsProperty = DependencyProperty.Register("ShowSeconds", typeof(bool), typeof(AnalogClock), new PropertyMetadata(true));

        //property name should be same as name given in the registered property
        public bool ShowSeconds
        {
            //get the value from the static ShowSeconds property
            get { return (bool)GetValue(ShowSecondsProperty); }
            //setting ShowProperty to the value that is passed into the setter
            set { SetValue(ShowSecondsProperty, value); }
        }

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

            /*//create a binding for the seconds hand
            Binding showSecondHandBinding = new Binding
            {
                //path to the binding
                Path = new PropertyPath(nameof(ShowSeconds)),
                Source = this,
                //if showsecond is true then our binding will return visible, and if its false it collapsed
                Converter = new BooleanToVisibilityConverter()

            };

            //add binding to the second hand
            secondHand.SetBinding(VisibilityProperty, showSecondHandBinding);*/

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
