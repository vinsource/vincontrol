namespace VINCONTROL.Silverlight.Helpers
{
    public class ErrorHandler
    {
        public static void ShowWarning(string message)
        {
            var popup = new WarningWindow(message);
            popup.Show();
        }

        public static void ShowError(string message)
        {
            var popup = new ErrorWindow(message);
            popup.Show();
        }
    }

    public enum UploadStatus
    {
        Existed, Uploading, Finish
    }
}
