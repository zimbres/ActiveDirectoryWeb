namespace ActiveDirectoryWebFrontend.Models;

public class GroupModel
{
    public string Name { get; set; }
    public string Type { get; set; }
    public string DistinguishedName { get; set; }
    public List<string> Members { get; set; } = new();
}
