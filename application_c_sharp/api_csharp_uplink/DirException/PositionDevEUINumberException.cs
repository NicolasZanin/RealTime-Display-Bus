namespace api_csharp_uplink.DirException;

public class PositionDevEuiNumberException(string devEuiCard) : Exception($"Not Position for this devEui of the Card: {devEuiCard}");