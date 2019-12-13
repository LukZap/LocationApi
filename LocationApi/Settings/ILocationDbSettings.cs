namespace LocationApi.Settings
{
    public interface ILocationDbSettings
    {
        string GeolocationsCollection { get; set; }
        string ConnectionString { get; set; }
        string DatabaseName { get; set; }
    }
}