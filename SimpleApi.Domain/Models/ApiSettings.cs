
public class SimpleApiSettings
{
    public ApiSettings WeatherApiSettings { get; set; }
    public ApiSettings LocationApiSettings { get; set; }
    public SpotifyApiSettings SpotifyApiSettings { get; set; }
}


public class SpotifyApiSettings
{
    public string ClientId { get; set; }
    public string Secret { get; set; }
}
public class ApiSettings
{
    public string Key { get; set; }
    public string Host { get; set; }
}