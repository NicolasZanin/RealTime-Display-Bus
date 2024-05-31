namespace api_csharp_uplink.DirException
{
    public class BusAlreadyCreateException(int busNumber) : Exception($"Bus with number {busNumber} is already exist")
    {
    }
}
