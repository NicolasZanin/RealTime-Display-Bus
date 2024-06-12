using api_csharp_uplink.DirException;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

namespace api_csharp_uplink.Composant;

public class PositionComposant(IPositionRepository positionRepository) : IPositionComposant
{
    public PositionBus AddPosition(double latitude, double longitude, string devEuiCard)
    {
        if (latitude < -90 || latitude > 90 || longitude < -180 || longitude > 180)
            throw new ValueNotCorrectException("Latitude or longitude is not correct");
        
        PositionBus positionBus = new(new Position(latitude, longitude), devEuiCard);
        return positionRepository.AddPosition(positionBus);
    }

    public PositionBus GetLastPosition(string devEuiCard)
    {
        PositionBus? positionBus = positionRepository.GetLastPosition(devEuiCard);
        return positionBus ?? throw new PositionDevEuiNumberException(devEuiCard);
    }
}