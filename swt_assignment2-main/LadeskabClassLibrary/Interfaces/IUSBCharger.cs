namespace Ladeskab.Interfaces
{
    public interface IUSBCharger
    {
        event EventHandler<USBChargeChangedEventArgs>? USBChargeChanged;


        double CurrentValue { get; }


        bool Connected { get; }


        void StartCharge();

        void StopCharge();
    }
}
