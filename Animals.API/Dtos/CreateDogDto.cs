using System.ComponentModel.DataAnnotations;

namespace Animals.API.Dtos;

public class CreateDogDto

{
    public string Name { get; set; }
    public string Color { get; set; }
    public int TailLength { get; set; }
    public int Weight { get; set; }
}
