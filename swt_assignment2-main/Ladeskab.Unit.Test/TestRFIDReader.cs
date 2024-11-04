namespace Ladeskab.Unit.Test;
[TestFixture]
public class TestRFIDReader
{   
    private RFID_Reader _uut;
    private RFIDReaderChangedEventArgs _eventArgs;

    [SetUp]
    public void Setup()
    {
        _uut = new RFID_Reader();
        _uut.RFIDChangedEvent += (sender, args) => _eventArgs = args;
    }

    [Test]
    public void SetRFID_NewValue_EventTriggered()
    {
        //Arrange
        int newRFID = 123;

        //Act
        _uut.setRFID(newRFID);

        //Assert
        Assert.IsNotNull(_eventArgs);
        Assert.AreEqual(newRFID, _eventArgs.RFID);

    }
}
