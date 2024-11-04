using Ladeskab.Interfaces;

namespace Ladeskab
{
    public class Door : IDoor
    {
        

        public event EventHandler<DoorChangedEventArgs>? DoorChangedEvent;

        private Enums.LadeskabState _oldDoorState = Enums.LadeskabState.Open;
        public Enums.LadeskabState OldDoorState => _oldDoorState;
        public void setDoor(Enums.LadeskabState NewDoorStatus)
        {
            //Door open = true
            //Door Closed = false
            if (NewDoorStatus != _oldDoorState)
            {
                OnDoorChanged(new DoorChangedEventArgs { DoorStatus = NewDoorStatus });
                _oldDoorState = NewDoorStatus;
            }
        }
        public virtual void OnDoorChanged(DoorChangedEventArgs eventArgs)
        {
            DoorChangedEvent?.Invoke(this, eventArgs);
        }

        public void ToggleLockState()
        {
            switch (_oldDoorState)
            {
                case Enums.LadeskabState.Closed:
                    _oldDoorState = Enums.LadeskabState.Locked; break;
                case Enums.LadeskabState.Locked:
                    _oldDoorState = Enums.LadeskabState.Closed; break;
            }
            Console.WriteLine($"Door {_oldDoorState}");
        }

        public void CloseDoor()
        {
            Console.WriteLine("Door Closed");
            _oldDoorState = Enums.LadeskabState.Closed;
        }

        public void OpenDoor()
        {
            Console.WriteLine("Door Opened");
            _oldDoorState = Enums.LadeskabState.Open;
        }
    }
}