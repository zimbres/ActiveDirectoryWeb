namespace ActiveDirectoryWebApi.Models;

public class Health
{
    public HealthData Data { get; set; } = new();
}

public class HealthData
{
    public bool Health { get; set; } = true;
    public string Version { get; set; } = Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "Unknown";
}