namespace Ladeskab.Interfaces
{
    public interface IDoor
    {
        public event EventHandler<DoorChangedEventArgs> DoorChangedEvent;
        public void ToggleLockState();
        public void CloseDoor();

        public void OpenDoor();

    }
}
