namespace api_csharp_uplink.Entities;

public class Position(double latitude, double longitude)
{
    public double Latitude { get; } = latitude;
    public double Longitude { get; } = longitude;
    
    public override bool Equals(object? obj)
    {
        if (obj == this)
            return true;
        if (obj == null || obj.GetType() != GetType())
            return false;
        Position position = (Position) obj;
        return Math.Abs(position.Latitude - Latitude) < 0.001 && Math.Abs(position.Longitude - Longitude) < 0.001;
    }
    
    public override int GetHashCode()
    {
        return HashCode.Combine(Latitude, Longitude);
    }

    public override string ToString()
    {
        return $"Latitude: {Latitude}, Longitude: {Longitude}";
    }
}