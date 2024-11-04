using Ladeskab.Interfaces;
using NSubstitute;

namespace Ladeskab.Unit.Test
{
    [TestFixture]
    public class TestChargeControl
    {
        private ChargeControl _uut;
        private IUSBCharger _usbsub;
        private IDisplay _displaysub;

        [SetUp]
        public void Setup()
        {
            _usbsub = Substitute.For<IUSBCharger>();
            _displaysub = Substitute.For<IDisplay>();
            _uut = new ChargeControl(_usbsub, _displaysub);
        }

        [Test]
        [TestCase(TestName = "startCharge")]
        public void StartChargingTest()
        {
            //Act
            _uut.startCharge();

            //Assert
            _usbsub.Received().StartCharge();
        }


        [Test]
        [TestCase(TestName = "stopCharge")]
        public void StopChargingTest()
        {
            //Act
            _uut.stopCharge();

            //Assert
            _usbsub.Received().StopCharge();
        }

        [Test]
        [TestCase(true, TestName = "test connection is true")]
        [TestCase(false, TestName = "test connection is false")]
        public void Connectiontest(bool connection)
        {
            //Arrange
            _usbsub.Connected.Returns(connection);

            //Assert
            Assert.IsTrue(_uut.isConnected() == connection);
        }

        [Test]
        [TestCase(0, "No connection", TestName = "Handle Current 0")]
		
		[TestCase(0.1, "Phone is fully charged", TestName = "Handle Current 1")]
		[TestCase(4.9, "Phone is fully charged", TestName = "Handle Current 1")]
		
		[TestCase(51, "Charge is in progress", TestName = "Handle Current 100")]
		[TestCase(499, "Charge is in progress", TestName = "Handle Current 100")]
			
		[TestCase(501, "Stopping charge immediately. System fail")]
		[TestCase(5000, "Stopping charge immediately. System fail")]
		
		[TestCase(-1, "Unknown error")]
		[TestCase(-100, "Unknown error")]
        public void handleChargingEvent(double current, string message)
        {
            //Act
            _usbsub.USBChargeChanged += Raise.EventWith(new USBChargeChangedEventArgs { Current = current });

            //Assert
            _displaysub.Received().DisplayMessage(message);
        }
    }
}
