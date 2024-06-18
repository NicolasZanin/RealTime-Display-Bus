namespace api_csharp_uplink.Entities
{
    public class Schedule
    {
        public List<string> schedules { get; set; }
        public string name { get; set; }
        public double latitude { get; set; }
        public double longitude { get; set; }
        public override string ToString()
        {
            return $"Station: {name}, position: ({latitude},{longitude}), schedules: {schedules}";
        }


        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
                return false;

            var schedule = (Schedule)obj;
            return name == schedule.name &&
                   latitude == schedule.latitude &&
                   longitude == schedule.longitude &&
                   schedules == schedule.schedules;
        }


        public override int GetHashCode()
        {
            int hashName = name.GetHashCode();
            int hashLatitude = latitude.GetHashCode();
            int hashLongitude = longitude.GetHashCode();
            int hashSchedules = schedules.GetHashCode();
            return hashName ^ hashLatitude ^ hashLongitude ^ hashSchedules;
        }

    }
}
