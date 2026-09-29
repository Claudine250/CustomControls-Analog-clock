using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace AnalogClockControl.CustomControls
{
    //define deligate
    public delegate void TimeChangedEventHandler(object sender, TimeChangedEventArgs args);
    public class AnalogClock : Clock
    {
        private Line hourHand;
        private Line minuteHand;
        private Line secondHand;

        
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

            base.OnApplyTemplate();
        }

        //fire routedeventhandler when time changes
        protected override void OnTimeChanged(DateTime time)
        {

            UpdateHandAngles(time);
            base.OnTimeChanged(time);
        }        

        //update line angles to corresponde to the time
        private void UpdateHandAngles(DateTime time)
        {
            hourHand.RenderTransform = new RotateTransform((time.Hour / 12.0) * 360, 0.5, 05);
            minuteHand.RenderTransform = new RotateTransform((time.Minute / 60.0) * 360, 0.5, 05);
            secondHand.RenderTransform = new RotateTransform((time.Second / 60.0) * 360, 0.5, 05);
        }
    }
}