using System.Reflection;
using Animals.BLL.Abstract.Services;
using Animals.Entities;

namespace Animals.BLL.Impl;

public class DogService : IDogService
{
    public static List<Dog> SortDogs(List<Dog> dogs, string attribute, string order)
    {
        if (string.IsNullOrWhiteSpace(attribute) || string.IsNullOrWhiteSpace(order))
        {
            // Handle invalid input
            throw new ArgumentException("Attribute and order must be provided.");
        }

        var propertyInfo = typeof(Dog).GetProperty(attribute, BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

        if (propertyInfo == null)
        {
            // Handle non-existing property
            // todo return it as action result. badrequest: there is not such property
            throw new ArgumentException($"Property '{attribute}' does not exist on Dog.");
        }

        var sortedDogs = order.Equals("desc", StringComparison.OrdinalIgnoreCase)
            ? dogs.OrderByDescending(dog => propertyInfo.GetValue(dog, null)).ToList()
            : dogs.OrderBy(dog => propertyInfo.GetValue(dog, null)).ToList();

        return sortedDogs;
    }

    public static List<Dog> Pagination(List<Dog> dogs, int? pageNumber, int? pageSize)
    {
        if (pageNumber.HasValue && pageSize.HasValue)
        {
            dogs = dogs.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
        }

        return dogs;
    }
}
