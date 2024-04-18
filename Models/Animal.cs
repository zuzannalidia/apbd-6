namespace RestApiApbdPjatkCw4.Models;

public class Animal
{
    public int ID { get; set; }
    public string Name { get; set; } = string.Empty;
    /*? -> wartość jest null'owalna */
    public string? Description { get; set; } 
    public string Category { get; set; } = string.Empty;
    public string Area { get; set; } = string.Empty;
}