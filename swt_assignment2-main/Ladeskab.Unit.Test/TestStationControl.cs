using Ladeskab.Interfaces;
using NSubstitute;

namespace Ladeskab.Unit.Test
{
    [TestFixture]
    public class TestStationControl
    {
        private StationControl _uut;

        private IDoor _doorsub;
        private IRFID_Reader _readersub;
        private IDisplay _displaysub;
        private ILogFile _logFilesub;
        private IChargeControl _chargeControlsub;

        [SetUp]
        public void Setup()
        {
            _doorsub = Substitute.For<IDoor>();
            _readersub = Substitute.For<IRFID_Reader>();
            _displaysub = Substitute.For<IDisplay>();
            _logFilesub = Substitute.For<ILogFile>();
            _chargeControlsub = Substitute.For<IChargeControl>();

            _uut = new StationControl(_doorsub, _readersub, _displaysub, _logFilesub, _chargeControlsub);
        }

        [Test]
        [TestCase(TestName = "Open Door")]
        public void DoorOpen()
        {
            //Act
            _doorsub.DoorChangedEvent += Raise.EventWith(new DoorChangedEventArgs { DoorStatus = Enums.LadeskabState.Open });

            //Assert
            _displaysub.Received().DisplayMessage("Connect Phone");
        }

        [Test]
        [TestCase(TestName = "Close Door")]
        public void DoorClosed()
        {
            //Act
            _doorsub.DoorChangedEvent += Raise.EventWith(new DoorChangedEventArgs { DoorStatus = Enums.LadeskabState.Closed });

            //Assert
            _displaysub.Received().DisplayMessage("Scan RFID");
        }

        [Test]
        [TestCase(0, Enums.LadeskabState.Open, true, "Please close the door", TestName = "RFID scanned with Open Door")]
        [TestCase(111, Enums.LadeskabState.Closed, false, "Please connect phone", TestName = "RFID scanned without connecting phone")]
        [TestCase(111, Enums.LadeskabState.Closed, true, "Door Locked", TestName = "RFID scanned and locked door")]
        public void TestRFIDScannedUnlocked(int RFID, Enums.LadeskabState doorState, bool connection, string result)
        {
            //Arrange
            _doorsub.DoorChangedEvent += Raise.EventWith(new DoorChangedEventArgs { DoorStatus = doorState });
            _chargeControlsub.isConnected().Returns(connection);

            //Act
            _readersub.RFIDChangedEvent += Raise.EventWith(new RFIDReaderChangedEventArgs { RFID = RFID });

            //Assert
            _displaysub.Received().DisplayMessage(result);
        }

        [Test]
        [TestCase(111, "Door Unlocked", TestName = "Correct RFID Scanned")]
        [TestCase(110, "Wrong RFID", TestName = "Incorrect RFID Scanned")]
        public void TestRFIDScannedLocked(int RFID, string result)
        {
            //Arrange
            _doorsub.DoorChangedEvent += Raise.EventWith(new DoorChangedEventArgs { DoorStatus = Enums.LadeskabState.Closed });
            _chargeControlsub.isConnected().Returns(true);
            _readersub.RFIDChangedEvent += Raise.EventWith(new RFIDReaderChangedEventArgs { RFID = 111 });

            //Act
            _readersub.RFIDChangedEvent += Raise.EventWith(new RFIDReaderChangedEventArgs { RFID = RFID });

            //Assert
            _displaysub.Received().DisplayMessage(result);
        }
    }
}
