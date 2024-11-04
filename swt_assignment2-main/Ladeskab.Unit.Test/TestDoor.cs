using Ladeskab.Interfaces;
using NSubstitute;

namespace Ladeskab.Unit.Test
{
    [TestFixture]
    public class DoorTests
    {
        private Door _door;
        private DoorChangedEventArgs _eventArgs;
        private IDoor _doorSub;

        [SetUp]
        public void Setup()
        {
            _door = new Door();
            _eventArgs = null;

            _doorSub = Substitute.For<IDoor>();
            // Set up event handler to capture event arguments
            _door.DoorChangedEvent += (sender, args) => _eventArgs = args;
        }

        [Test]
        public void SetDoor_WhenDoorStatusChanges_RaisesDoorChangedEvent()
        {
            // Arrange
            Enums.LadeskabState initialStatus = Enums.LadeskabState.Open;
            Enums.LadeskabState newStatus = Enums.LadeskabState.Closed;

            // Act
            _door.setDoor(newStatus);

            // Assert
            Assert.That(_eventArgs, Is.Not.Null);
            Assert.That(_eventArgs.DoorStatus, Is.EqualTo(newStatus));
        }

        [Test]
        public void SetDoor_WhenDoorStatusDoesNotChange_DoesNotRaiseDoorChangedEvent()
        {
            // Arrange
            Enums.LadeskabState initialStatus = Enums.LadeskabState.Open;

            // Act
            _door.setDoor(initialStatus);

            // Assert
            Assert.That(_eventArgs, Is.Null);
        }

        [Test]
        public void OpenDoor_ShouldSetDoorStateToOpen()
        {
            // Act
            _door.OpenDoor();

            // Assert
            Assert.That(_door.OldDoorState, Is.EqualTo(Enums.LadeskabState.Open));
        }

        [Test]
        public void CloseDoor_ShouldSetDoorStateToClosed()
        {
            // Act
            _door.CloseDoor();

            // Assert
            Assert.That(_door.OldDoorState, Is.EqualTo(Enums.LadeskabState.Closed));
        }

        [Test]
        public void ToggleLockState_ShouldChangeDoorStateFromClosedToLocked()
        {
            // Arrange
            _door.CloseDoor();

            //Act
            _door.ToggleLockState();

            //Assert
            Assert.That(_door.OldDoorState, Is.EqualTo(Enums.LadeskabState.Locked));
        }

        [Test]
        public void ToggleLockState_ShouldChangeDoorStateFromLockedToClosed()
        {
            // Arrange
            _door.CloseDoor();

            //Act
            _door.ToggleLockState();
            _door.ToggleLockState();

            //Assert
            Assert.That(_door.OldDoorState, Is.EqualTo(Enums.LadeskabState.Closed));
        }

    }
}
