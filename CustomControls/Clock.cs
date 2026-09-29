using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace AnalogClockControl.CustomControls
{
    public class Clock : Control
    {
        //register dependency property with analog clock to set up whether its shown or hidden, defaulted to true
        public static DependencyProperty ShowSecondsProperty = DependencyProperty.Register("ShowSeconds", typeof(bool), typeof(Clock), new PropertyMetadata(true));

        //routed event can be used on other elements, using Bubble strategy means any element that has the clock inside of it can handle the event.
        //Direct routing strategy means that only the Analog clock that fires this event can handle the time changed event
        //Tunnel routing strategy means anything that is inside of the clock can handle the timechanged event
        public static RoutedEvent TimeChangedEvent = EventManager.RegisterRoutedEvent("TimeChanged", RoutingStrategy.Bubble, typeof(TimeChangedEventHandler), typeof(Clock));



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
        public override void OnApplyTemplate()
        {


            /*//create a binding for the seconds hand
            Binding showSecondHandBinding = new Binding
            {
                //path to the binding
                Path = new PropertyPath(nameof(ShowSeconds)),
                Source = this,
                //if showsecond is true then our binding will return visible, and if its false it collapsed
                Converter = new BooleanToVisibilityConverter()

            };UpdateHandAngles

            //add binding to the second hand
            secondHand.SetBinding(VisibilityProperty, showSecondHandBinding);*/

            OnTimeChanged(DateTime.Now);

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = new TimeSpan(0, 0, 1);
            timer.Tick += (s, e) => OnTimeChanged(DateTime.Now);
            timer.Start();

            base.OnApplyTemplate();
        }
        protected virtual void OnTimeChanged(DateTime time)
        {


            RaiseEvent(new TimeChangedEventArgs(TimeChangedEvent, this) { NewTime = time });
        }

        private void UpdateTimeStates(DateTime time)
        {
            if (time.Hour > 6 && time.Hour < 18)
            {
                VisualStateManager.GoToState(this, "Day", false);
            }
            else
            {
                VisualStateManager.GoToState(this, "Night", false);
            }

        }

    }
}
