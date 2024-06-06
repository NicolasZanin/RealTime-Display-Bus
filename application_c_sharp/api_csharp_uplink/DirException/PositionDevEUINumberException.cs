namespace api_csharp_uplink.DirException;

public class PositionDevEuiNumberException(int devEui) : Exception($"Not Position for this devEUI: {devEui}");