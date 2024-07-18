namespace api_csharp_uplink.Entities
{
    public class Bus
    {
        public int BusNumber { get; set; }
        public int DevEUICard { get; set; }
        public int LineBus { get; set; }

        public override string ToString()
        { 
            return $"LineBus: {LineBus}, BusNumber: {BusNumber}, DevEUICard: {DevEUICard}";
        }


        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var bus = (Bus)obj;
            return LineBus == bus.LineBus &&
                   BusNumber == bus.BusNumber &&
                   DevEUICard == bus.DevEUICard;
        }


        public override int GetHashCode()
        {
            int hashLineBus = LineBus.GetHashCode();
            int hashBusNumber = BusNumber.GetHashCode();
            int hashDevEUICard = DevEUICard.GetHashCode();

            return hashLineBus ^ hashBusNumber ^ hashDevEUICard;
        }

    }
}
