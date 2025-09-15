namespace ActiveDirectoryWebFrontend.Models;

public class ApiResponse
{
    public HealthData Data { get; set; }
}

public class HealthData
{
    public bool Health { get; set; }
    public string Version { get; set; }
}