using api_csharp_uplink.DirException;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

namespace api_csharp_uplink.Composant;

public class PositionComposant(IPositionRepository positionRepository) : IPositionComposant
{
    public PositionBus AddPosition(double latitude, double longitude, int devEuiNumber)
    {
        PositionBus positionBus = new(new Position(latitude, longitude), devEuiNumber);
        return positionRepository.AddPosition(positionBus);
    }

    public PositionBus GetLastPosition(int devEuiNumber)
    {
        PositionBus? positionBus = positionRepository.GetLastPosition(devEuiNumber);
        return positionBus ?? throw new PositionDevEuiNumberException(devEuiNumber);
    }
}