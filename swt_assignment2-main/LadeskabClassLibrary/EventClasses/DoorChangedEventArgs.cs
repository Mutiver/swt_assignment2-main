namespace Ladeskab
{
    public class DoorChangedEventArgs : EventArgs
    {
        public Enums.LadeskabState DoorStatus { get; set; }
    }
}
