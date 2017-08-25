using System.Windows.Browser;

namespace VINCONTROL.Silverlight.Helpers
{
    public class HtmlHelper
    {
        public static void CloseForm()
        {
            HtmlPage.Window.Invoke("closeForm");
        }
    }
}
