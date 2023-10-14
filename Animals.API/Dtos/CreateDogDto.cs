using System.ComponentModel.DataAnnotations;

namespace Animals.API.Dtos;

public class CreateDogDto

{
    public string Name { get; set; }
    public string Color { get; set; }
    
    [Range(0, int.MaxValue)]
    public int TailLength { get; set; }
    
    [Range(0, int.MaxValue)]
    public int Weight { get; set; }
}
