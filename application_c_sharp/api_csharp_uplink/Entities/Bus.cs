namespace api_csharp_uplink.Entities
{
    public class Bus(int busNumber, string devEuiCard, int lineBus)
    {
        public int BusNumber { get; } = busNumber;
        public string DevEuiCard { get; } = devEuiCard;
        public int LineBus { get; } = lineBus;

        public override string ToString()
        {
            return $"LineBus: {LineBus}, BusNumber: {BusNumber}, DevEuiCard: {DevEuiCard}";
        }


        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var bus = (Bus)obj;
            return LineBus == bus.LineBus &&
                   BusNumber == bus.BusNumber &&
                   DevEuiCard == bus.DevEuiCard;
        }


        public override int GetHashCode()
        {
            int hashLineBus = LineBus.GetHashCode();
            int hashBusNumber = BusNumber.GetHashCode();
            int hashDevEuiCard = DevEuiCard.GetHashCode();

            return hashLineBus ^ hashBusNumber ^ hashDevEuiCard;
        }

    }
}
