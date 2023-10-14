namespace Animals.Dtos.Tests;

public class CreateDogDtoTests
{
    [Fact]
    public void TailLength_Getter_ReturnsSetValue()
    {
        // Arrange
        var createDogDto = new CreateDogDto();
        const int expectedTailLength = 5; // Set within the [Range] attribute

        // Act
        createDogDto.TailLength = expectedTailLength;
        var actualTailLength = createDogDto.TailLength;

        // Assert
        Assert.Equal(expectedTailLength, actualTailLength);
    }

    [Fact]
    public void Weight_Getter_ReturnsSetValue()
    {
        // Arrange
        var createDogDto = new CreateDogDto();
        const int expectedWeight = -1; // Set outside the [Range] attribute

        // Act
        createDogDto.Weight = expectedWeight;
        var actualWeight = createDogDto.Weight;

        // Assert
        Assert.Equal(expectedWeight, actualWeight);
    }
    
    [Fact]
    public void Name_Getter_ReturnsSetValue()
    {
        // Arrange
        var createDogDto = new CreateDogDto();
        const string expectedName = "Fido";

        // Act
        createDogDto.Name = expectedName;
        var actualName = createDogDto.Name;

        // Assert
        Assert.Equal(expectedName, actualName);
    }

    [Fact]
    public void Color_Getter_ReturnsSetValue()
    {
        // Arrange
        var createDogDto = new CreateDogDto();
        const string expectedColor = "Brown";

        // Act
        createDogDto.Color = expectedColor;
        var actualColor = createDogDto.Color;

        // Assert
        Assert.Equal(expectedColor, actualColor);
    }
    
}
