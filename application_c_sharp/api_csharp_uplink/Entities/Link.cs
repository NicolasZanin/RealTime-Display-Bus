namespace api_csharp_uplink.Entities;

public class Link
{
    public string nameStation1 { get; }
    public string nameStation2 { get; }
    public int lineNumber { get; }
    public Orientation orientation { get; }
    public double distance { get; }
    public int seconds { get; }

    public Link(string nameStation1, string nameStation2, int lineNumber, Orientation orientation, double distance,
        int seconds)
    {
        if (lineNumber < 1)
            throw new ArgumentOutOfRangeException(nameof(lineNumber), "The line number must be greater than 0.");

        if (distance < 0)
            throw new ArgumentOutOfRangeException(nameof(distance), "The distance must be greater than or equal to 0.");

        if (seconds < 0)
            throw new ArgumentOutOfRangeException(nameof(seconds), "The seconds must be greater than or equal to 0.");

        this.nameStation1 = nameStation1;
        this.nameStation2 = nameStation2;
        this.lineNumber = lineNumber;
        this.orientation = orientation;
        this.distance = distance;
        this.seconds = seconds;
    }

    public override string ToString()
    {
        return $"Link between {nameStation1} and {nameStation2} on line {lineNumber} " +
               $"with orientation {orientation} and distance {distance} meters.";
    }

    public override bool Equals(object? obj)
    {
        if (obj == null || GetType() != obj.GetType())
            return false;

        Link link = (Link)obj;
        return lineNumber == link.lineNumber &&
               ((nameStation1.Equals(link.nameStation1) && nameStation2.Equals(link.nameStation2))
                || (nameStation1.Equals(link.nameStation2) && nameStation2.Equals(link.nameStation1)));
    }

    public override int GetHashCode()
    {
        int hashNameStation1 = nameStation1.GetHashCode();
        int hashNameStation2 = nameStation2.GetHashCode();
        int hashLineNumber = lineNumber.GetHashCode();

        return hashNameStation1 ^ hashNameStation2 ^ hashLineNumber;
    }
}