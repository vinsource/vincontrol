using System.Windows.Controls;

namespace VINCONTROL.Silverlight.Helpers
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
