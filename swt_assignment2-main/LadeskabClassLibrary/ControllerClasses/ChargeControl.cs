using Ladeskab.Interfaces;

namespace Ladeskab
{
    public class ChargeControl : IChargeControl
    {
        private IUSBCharger _usbCharger;
        private IDisplay _display;

        public double CurrentCharge { get; set; }

        public ChargeControl(IUSBCharger usbCharger, IDisplay display)
        {
            _usbCharger = usbCharger;
            _display = display;
            _usbCharger.USBChargeChanged += HandleUSBChargeEvent;
        }

        public void startCharge()
        {
            _usbCharger.StartCharge();
        }

        public void stopCharge()
        {
            _usbCharger.StopCharge();
        }

        public bool isConnected()
        {

            return _usbCharger.Connected;
        }

        private void HandleUSBChargeEvent(object sender, USBChargeChangedEventArgs e)
        {
            CurrentCharge = e.Current;

            switch (CurrentCharge)
            {
                case 0:
                    _display.DisplayMessage("No connection");
                    break;
                case > 0 and <= 5:
                    _display.DisplayMessage("Phone is fully charged");
                    break;
                case > 5 and <= 500:
                    _display.DisplayMessage("Charge is in progress");
                    break;
                case > 500:
                    _usbCharger.StopCharge();
                    _display.DisplayMessage("Stopping charge immediately. System fail");
                    break;
                default:
                    _display.DisplayMessage("Unknown error");
                    break;
            }
        }
      
    }
}
