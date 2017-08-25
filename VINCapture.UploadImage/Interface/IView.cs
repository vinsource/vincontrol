namespace VINCapture.UploadImage.Interface
{
    public interface IView
    {
        void SetDataContext(object context);
        string Path { get; set; }
    }
}