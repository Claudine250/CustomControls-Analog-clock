using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AnalogClockControl.CustomControls;

namespace CustomControls
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void AnalogClock_TimeChanged(object sender, TimeChangedEventArgs e)
        {
            //take our textbox and set our text to event's new time
            tbTime.Text = e.NewTime.ToString("HH:mm:ss");
        }
    }
}