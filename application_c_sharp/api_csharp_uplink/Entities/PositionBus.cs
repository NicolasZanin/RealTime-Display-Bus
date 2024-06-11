namespace api_csharp_uplink.Entities;

public class PositionBus(Position position, int devEuiNumber)
{
    public Position Position { get; } = position;
    public int DevEuiNumber { get; } = devEuiNumber;

    public PositionBus() : this(new Position(), 0)
    {
    }
    public override bool Equals(object? obj)
    {
        if (obj == this)
            return true;
        if (obj == null || obj.GetType() != GetType())
            return false;
        PositionBus positionBus = (PositionBus)obj;
        return positionBus.Position.Equals(Position)  && positionBus.DevEuiNumber == DevEuiNumber;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Position, DevEuiNumber);
    }

    public override string ToString()
    {
        return $"Position: {Position}, DevEuiNumber: {DevEuiNumber}";
    }
}