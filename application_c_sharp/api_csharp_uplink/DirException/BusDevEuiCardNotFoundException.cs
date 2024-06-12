namespace api_csharp_uplink.DirException;

public class BusDevEuiCardNotFoundException(string devEuiCard) : Exception($"Bus with card {devEuiCard} not found");