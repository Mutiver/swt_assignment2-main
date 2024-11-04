using Ladeskab.Interfaces;

namespace Ladeskab
{
    public class RFID_Reader : IRFID_Reader
    {
        private int _oldRFID;
        public event EventHandler<RFIDReaderChangedEventArgs>? RFIDChangedEvent;

        public void setRFID(int newRFID)
        {
            if (newRFID != _oldRFID)
            {
                OnRFIDChanged(new RFIDReaderChangedEventArgs { RFID = newRFID });
            }
        }

        protected virtual void OnRFIDChanged(RFIDReaderChangedEventArgs e)
        {
            RFIDChangedEvent?.Invoke(this, e);
        }
    }
}
