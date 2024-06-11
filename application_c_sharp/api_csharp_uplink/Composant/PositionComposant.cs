using api_csharp_uplink.DirException;
using api_csharp_uplink.Entities;
using api_csharp_uplink.Interface;

namespace api_csharp_uplink.Composant;

public class PositionComposant(IPositionRepository positionRepository) : IPositionComposant
{
    public PositionBus AddPosition(double latitude, double longitude, int devEuiNumber)
    {
        if (latitude < -90 || latitude > 90 || longitude < -180 || longitude > 180)
            throw new ValueNotCorrectException("Latitude or longitude is not correct");
        if(devEuiNumber < 0)
            throw new ValueNotCorrectException("DevEuiNumber is not correct");
        
        PositionBus positionBus = new(new Position(latitude, longitude), devEuiNumber);
        return positionRepository.AddPosition(positionBus);
    }

    public PositionBus GetLastPosition(int devEuiNumber)
    {
        if(devEuiNumber < 0)
            throw new ValueNotCorrectException("DevEuiNumber is not correct");
        
        PositionBus? positionBus = positionRepository.GetLastPosition(devEuiNumber);
        return positionBus ?? throw new PositionDevEuiNumberException(devEuiNumber);
    }
}