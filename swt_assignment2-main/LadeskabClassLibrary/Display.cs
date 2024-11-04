using Ladeskab.Interfaces;

namespace Ladeskab
{
    public class Display : IDisplay
    {
        public void DisplayMessage(string message)
        {
            Console.WriteLine(message);
        }
    }
}
