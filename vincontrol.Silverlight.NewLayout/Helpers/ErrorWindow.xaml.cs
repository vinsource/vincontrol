using System.Windows.Controls;

namespace vincontrol.Silverlight.NewLayout.Helpers
{
    public partial class ErrorWindow : ChildWindow
    {
        public ErrorWindow(string message)
        {
            InitializeComponent();

            try
            {
                fldMessage.Text = message;
            }
            catch { }
        }
    }
}
