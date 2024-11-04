﻿namespace Ladeskab.Unit.Test
{

    [TestFixture]
    public class TestUSBCharger
    {
        private USBCharger _uut;
        [SetUp]
        public void Setup()
        {
            _uut = new USBCharger();
        }

        [Test]
        public void ctor_IsConnected()
        {
            //Assert
            Assert.That(_uut.Connected, Is.True);
        }

        [Test]
        public void ctor_CurentValueIsZero()
        {
            //Assert
            Assert.That(_uut.CurrentValue, Is.Zero);
        }

        [Test]
        public void SimulateDisconnected_ReturnsDisconnected()
        {
            //Arrange
            _uut.SimulateConnected(false);

            //Assert
            Assert.That(_uut.Connected, Is.False);
        }

        [Test]
        public void Started_WaitSomeTime_ReceivedSeveralValues()
        {
            //Arrange
            int numValues = 0;
            _uut.USBChargeChanged += (o, args) => numValues++;

            //Act
            _uut.StartCharge();

            System.Threading.Thread.Sleep(1100);

            //Assert
            Assert.That(numValues, Is.GreaterThan(4));
        }

        [Test]
        public void Started_WaitSomeTime_ReceivedChangedValue()
        {
            //Arrange
            double lastValue = 1000;
            _uut.USBChargeChanged += (o, args) => lastValue = args.Current;

            //Act
            _uut.StartCharge();

            System.Threading.Thread.Sleep(300);

            //Assert
            Assert.That(lastValue, Is.LessThan(500.0));
        }

        [Test]
        public void StartedNoEventReceiver_WaitSomeTime_PropertyChangedValue()
        {
            //Act
            _uut.StartCharge();

            System.Threading.Thread.Sleep(300);

            //Assert
            Assert.That(_uut.CurrentValue, Is.LessThan(500.0));
        }

        [Test]
        public void Started_WaitSomeTime_PropertyMatchesReceivedValue()
        {
            //Arrange
            double lastValue = 1000;
            _uut.USBChargeChanged += (o, args) => lastValue = args.Current;

            //Act
            _uut.StartCharge();

            System.Threading.Thread.Sleep(1100);

            //Assert
            Assert.That(lastValue, Is.EqualTo(_uut.CurrentValue));
        }


        [Test]
        public void Started_SimulateOverload_ReceivesHighValue()
        {
            //Arrange
            ManualResetEvent pause = new ManualResetEvent(false);
            double lastValue = 0;

            _uut.USBChargeChanged += (o, args) =>
            {
                lastValue = args.Current;

                pause.Set();
            };

            //Act
            // Start
            _uut.StartCharge();

            // Next value should be high
            _uut.SimulateOverload(true);

            // Reset event
            pause.Reset();

            // Wait for next tick, should send overloaded value
            pause.WaitOne(300);

            //Assert
            Assert.That(lastValue, Is.GreaterThan(500.0));
        }

        [Test]
        public void Started_SimulateDisconnected_ReceivesZero()
        {
            //Arrange
            ManualResetEvent pause = new ManualResetEvent(false);
            double lastValue = 1000;

            _uut.USBChargeChanged += (o, args) =>
            {
                lastValue = args.Current;
                pause.Set();
            };

            //Act
            // Start
            _uut.StartCharge();

            // Next value should be zero
            _uut.SimulateConnected(false);

            // Reset event
            pause.Reset();

            // Wait for next tick, should send disconnected value
            pause.WaitOne(300);

            //Assert
            Assert.That(lastValue, Is.Zero);
        }

        [Test]
        public void SimulateOverload_Start_ReceivesHighValueImmediately()
        {
            //Arrange
            double lastValue = 0;

            _uut.USBChargeChanged += (o, args) =>
            {
                lastValue = args.Current;
            };

            // First value should be high
            _uut.SimulateOverload(true);

            //Act
            // Start
            _uut.StartCharge();

            // Should not wait for first tick, should send overload immediately

            //Assert
            Assert.That(lastValue, Is.GreaterThan(500.0));
        }

        [Test]
        public void SimulateDisconnected_Start_ReceivesZeroValueImmediately()
        {
            //Arrange
            double lastValue = 1000;

            _uut.USBChargeChanged += (o, args) =>
            {
                lastValue = args.Current;
            };

            // First value should be high
            _uut.SimulateConnected(false);

            //Act
            // Start
            _uut.StartCharge();

            // Should not wait for first tick, should send zero immediately

            //Assert
            Assert.That(lastValue, Is.Zero);
        }

        [Test]
        public void StopCharge_IsCharging_ReceivesZeroValue()
        {
            //Arrange
            double lastValue = 1000;
            _uut.USBChargeChanged += (o, args) => lastValue = args.Current;

            //Act
            _uut.StartCharge();

            System.Threading.Thread.Sleep(300);

            _uut.StopCharge();

            //Assert
            Assert.That(lastValue, Is.EqualTo(0.0));
        }

        [Test]
        public void StopCharge_IsCharging_PropertyIsZero()
        {
            //Act
            _uut.StartCharge();

            System.Threading.Thread.Sleep(300);

            _uut.StopCharge();

            //Assert
            Assert.That(_uut.CurrentValue, Is.EqualTo(0.0));
        }

        [Test]
        public void StopCharge_IsCharging_ReceivesNoMoreValues()
        {
            //Arrange
            double lastValue = 1000;
            _uut.USBChargeChanged += (o, args) => lastValue = args.Current;

            //Act
            _uut.StartCharge();

            System.Threading.Thread.Sleep(300);

            _uut.StopCharge();
            lastValue = 1000;

            // Wait for a tick
            System.Threading.Thread.Sleep(300);

            //Assert
            // No new value received
            Assert.That(lastValue, Is.EqualTo(1000.0));
        }



    }
}
