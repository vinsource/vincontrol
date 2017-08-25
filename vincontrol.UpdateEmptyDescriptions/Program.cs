using vincontrol.Services;

namespace vincontrol.UpdateEmptyDescriptions
{
    public class Program
    {
        static void Main(string[] args)
        {
            var process = new UpdateEmptyDescriptionProcess();
            process.Run();
        }
    }
}
