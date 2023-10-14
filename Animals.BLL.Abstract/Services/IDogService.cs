using Animals.BLL.Abstract.Services.Base;
using Animals.Entities;
using Animals.Models;

namespace Animals.BLL.Abstract.Services;

public interface IDogService : IService<DogModel>
{
    List<DogModel> SortDogs(List<DogModel> dogs, string? attribute, bool? isAscendingOrder = true);
    List<DogModel> Pagination(List<DogModel> dogs, int? pageNumber, int? pageSize);
}
