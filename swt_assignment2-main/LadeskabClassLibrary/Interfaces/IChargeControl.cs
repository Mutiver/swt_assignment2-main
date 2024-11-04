namespace Ladeskab.Interfaces
{
    public interface IChargeControl
    {

	    public void startCharge();
        public void stopCharge();
        public bool isConnected();
     
    }
}
