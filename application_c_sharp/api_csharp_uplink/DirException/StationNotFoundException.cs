using System;

namespace api_csharp_uplink.DirException
{
    public class ScheduleNotFoundException(string station) : Exception($"Station {station} not found")
    {
    }
}
