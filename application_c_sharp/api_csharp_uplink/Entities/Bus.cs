namespace api_csharp_uplink.Entities
{
    public class Bus
    {
        public int BusNumber { get; set; }
        public int DevEUICard { get; set; }
        public int LineBus { get; set; }

        // Override Equals method
        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var bus = (Bus)obj;
            return LineBus == bus.LineBus &&
                   BusNumber == bus.BusNumber &&
                   DevEUICard == bus.DevEUICard;
        }

        // Override GetHashCode method
        public override int GetHashCode()
        {
            int hashLineBus = LineBus.GetHashCode();
            int hashBusNumber = BusNumber.GetHashCode();
            int hashDevEUICard = DevEUICard.GetHashCode();

            return hashLineBus ^ hashBusNumber ^ hashDevEUICard;
        }

    }
}
