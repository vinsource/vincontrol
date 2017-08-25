using System.Windows.Browser;

namespace vincontrol.Silverlight.NewLayout.Helpers
{
    public class HtmlHelper
    {
        public static void CloseForm()
        {
            HtmlPage.Window.Invoke("closeForm");
        }
    }
}
