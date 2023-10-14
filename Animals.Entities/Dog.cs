using System.ComponentModel.DataAnnotations;

namespace Animals.Entities;

public class Dog
{
    public int Id { get; set; }

    public string? Name { get; set; } = string.Empty;
    public string? Color { get; set; } = string.Empty;
    
    [Range(0, int.MaxValue)]
    public int? TailLength { get; set; }
    [Range(0, int.MaxValue)]
    public int? Weight { get; set; }
}
