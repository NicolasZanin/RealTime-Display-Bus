using System;

namespace api_csharp_uplink.DirException
{
    public class BusNotFoundException(int busNumber) : Exception($"Bus with number {busNumber} not found")
    {
    }
}
