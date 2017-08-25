namespace vincontrol.Backend.Interface
{
    public interface IEditView : IView
    {
        bool IsAdded { get; set; }
        //int Id { get; set; }
    }

    public interface  IEditNameView :IEditView
    {
        //string ProfileName { get; set; }
    }
}