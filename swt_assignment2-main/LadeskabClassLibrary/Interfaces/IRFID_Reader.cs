namespace Ladeskab.Interfaces
{
    public interface IRFID_Reader
    {
        event EventHandler<RFIDReaderChangedEventArgs> RFIDChangedEvent;
    }
}
