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
    //define deligate
    public delegate void TimeChangedEventHandler(object sender, TimeChangedEventArgs args);
    public class AnalogClock : Control
    {
        private Line hourHand;
        private Line minuteHand;
        private Line secondHand;

        //register dependency property with analog clock to set up whether its shown or hidden, defaulted to true
        public static DependencyProperty ShowSecondsProperty = DependencyProperty.Register("ShowSeconds", typeof(bool), typeof(AnalogClock), new PropertyMetadata(true));

        //routed event can be used on other elements, using Bubble strategy means any element that has the clock inside of it can handle the event.
        //Direct routing strategy means that only the Analog clock that fires this event can handle the time changed event
        //Tunnel routing strategy means anything that is inside of the clock can handle the timechanged event
        public static RoutedEvent TimeChangedEvent = EventManager.RegisterRoutedEvent("TimeChanged", RoutingStrategy.Bubble, typeof(TimeChangedEventHandler), typeof(AnalogClock));

        

        //property name should be same as name given in the registered property
        public bool ShowSeconds
        {
            //get the value from the static ShowSeconds property
            get { return (bool)GetValue(ShowSecondsProperty); }
            //setting ShowProperty to the value that is passed into the setter
            set { SetValue(ShowSecondsProperty, value); }
        }

        //set up event that our eventrouter wraps, name has to match our routed event name <TimeChangedEvent>, also define deligate which will be routedeventargs
        public event TimeChangedEventHandler TimeChanged
        {
            // when event is subscribed to, add a handler
            add
            {
                AddHandler(TimeChangedEvent, value);
            }
            //unsubscribing to or removing event
            remove
            {
                RemoveHandler(TimeChangedEvent, value);
            }
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

            UpdateHandAngles(DateTime.Now);

            DispatcherTimer timer = new  DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += (s, e) => OnTimeChanged(DateTime.Now);
            timer.Start();

            base.OnApplyTemplate();
        }

        //fire routedeventhandler when time changes
        protected virtual void OnTimeChanged(DateTime time)
        {

            UpdateHandAngles(time);
            UpdateTimeStates(time);
            RaiseEvent(new TimeChangedEventArgs(TimeChangedEvent, this) {NewTime = time });
        }


        //will be called everytime time changes to change our state to night time mode or daytime mode
        private void UpdateTimeStates(DateTime time)
        {
            if(time.Hour > 6 && time.Hour < 18)
            {
                VisualStateManager.GoToState(this, "Day", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "Night", false);
            }
            
        }

        //update line angles to corresponde to the time
        private void UpdateHandAngles(DateTime time)
        {
            hourHand.RenderTransform = new RotateTransform((time.Hour / 12.0) *360, 0.5, 05);
            minuteHand.RenderTransform = new RotateTransform((time.Minute / 60.0) * 360, 0.5, 05);
            secondHand.RenderTransform = new RotateTransform((time.Second / 60.0) * 360, 0.5, 05);
        }
    }
}
