using System.Windows;
using System.Windows.Controls;

namespace vincontrol.Silverlight.NewLayout.Helpers
{
    public partial class WarningWindow : ChildWindow
    {
        public WarningWindow(string message)
        {
            InitializeComponent();

            try
            {
                fldMessage.Text = message;
            }
            catch { }
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DialogResult = true;
            }
            catch { }
        }
    }
}
