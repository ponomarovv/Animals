using System.Reflection;
using Animals.BLL.Abstract.Services;
using Animals.Entities;

namespace Animals.BLL.Impl.Services;

public class DogService : IDogService
{
    public static List<Dog> SortDogs(List<Dog> dogs, string? attribute, bool? isAscendingOrder = true)
    {
        PropertyInfo? propertyInfo;

        if (string.IsNullOrEmpty(attribute))
        {
            propertyInfo = typeof(Dog).GetProperty("Id",
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        }
        else
        {
            propertyInfo = typeof(Dog).GetProperty(attribute,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        }

        var sortedDogs = isAscendingOrder == true
            ? dogs.OrderBy(dog => propertyInfo.GetValue(dog, null)).ToList()
            : dogs.OrderByDescending(dog => propertyInfo.GetValue(dog, null)).ToList();

        return sortedDogs;
    }

    public static List<Dog> Pagination(List<Dog> dogs, int? pageNumber, int? pageSize)
    {
        List<Dog> result = new();
        if (pageNumber.HasValue && pageSize.HasValue)
        {
            result = dogs.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
        }

        return result;
    }
}
