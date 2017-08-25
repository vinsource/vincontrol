namespace vincontrol.Backend.Interface
{
    public interface IView
    {
        void SetDataContext(object context);
        void Close();
    }

    public interface INavigate: IView
    {
        void Navigate(object item);
    }
}
