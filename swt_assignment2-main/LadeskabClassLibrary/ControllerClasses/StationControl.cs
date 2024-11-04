using Ladeskab.Interfaces;

namespace Ladeskab
{
    public class StationControl
    {
        


        // Her mangler flere member variable
        private Enums.LadeskabState _doorState;

        private int _currentRFID;
        private IDoor _door;
        private IRFID_Reader _reader;
        private IDisplay _display;
        private ILogFile _logFile;
        private IChargeControl _chargeControl;


       
        public StationControl(IDoor door, IRFID_Reader reader, IDisplay display, ILogFile logfile, IChargeControl chargeControl)
        {
            _door = door;
            _reader = reader;
            _display = display;
            _logFile = logfile;
            _chargeControl = chargeControl;
            _door.DoorChangedEvent += HandleDoorChangedEvent;
            _reader.RFIDChangedEvent += HandleRFIDChangedEvent;
        }

        //handler for Door
        private void HandleDoorChangedEvent(object sender, DoorChangedEventArgs e)
        {
            _doorState = e.DoorStatus;
            switch (_doorState)
            {
                case Enums.LadeskabState.Open:
                    _display.DisplayMessage("Connect Phone");
                    break;
                case Enums.LadeskabState.Closed:
                    _display.DisplayMessage("Scan RFID");
                    break;
            }
        }


        //handler for RFID-Readeren
        private void HandleRFIDChangedEvent(object sender, RFIDReaderChangedEventArgs e)
        {
            switch (_doorState)
            {
                case Enums.LadeskabState.Open:
                    _display.DisplayMessage("Please close the door");
                    break;
                case Enums.LadeskabState.Closed:
                    if (_chargeControl.isConnected())
                    {
                        _currentRFID = e.RFID;
                        _door.ToggleLockState();
                        string log = DateTime.Now + ": Skab låst med RFID: " + _currentRFID;
                        _logFile.WriteToLogFile(log);
                        _chargeControl.startCharge();
                        _doorState = Enums.LadeskabState.Locked;
                        _display.DisplayMessage("Door Locked");
                    }
                    else _display.DisplayMessage("Please connect phone");
                    break;
                case Enums.LadeskabState.Locked:
                    if (e.RFID == _currentRFID)
                    {
                        _display.DisplayMessage("Door Unlocked");
                        _chargeControl.stopCharge();
                        _door.ToggleLockState();
                        string log = DateTime.Now + ": Skab oplåst med RFID: " + e.RFID;
                        _logFile.WriteToLogFile(log);
                        _doorState = Enums.LadeskabState.Closed;
                        _currentRFID = 0;
                    }
                    else _display.DisplayMessage("Wrong RFID");
                    break;
            }

        }
    }
}
