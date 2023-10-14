using System.Reflection;
using Animals.BLL.Abstract.Services;
using Animals.DAL.Abstract.Repository.Base;
using Animals.Entities;
using Animals.Models;
using AutoMapper;

namespace Animals.BLL.Impl.Services;



public class DogService : IDogService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public DogService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }
    
    public async Task<DogModel> CreateAsync(DogModel model)
    {
        var entity = _mapper.Map<Dog>(model);
        var newEntity = await _unitOfWork.DogRepository.AddAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return _mapper.Map<DogModel>(newEntity);
    }

    public async Task<List<DogModel>> GetAllAsync()
    {
        var entities = await _unitOfWork.DogRepository.GetAllAsync(x => true);
        var result = entities.Select(_mapper.Map<DogModel>).ToList();

        return result;
    }

    public async Task<DogModel> GetByIdAsync(int id)
    {
        var result = _mapper.Map<DogModel>(await _unitOfWork.DogRepository.GetByIdAsync(id));

        return result;
    }

    public async Task<bool> UpdateAsync(DogModel model)
    {
        if (model == null)
        {
            return false;
        }

        var entity = _mapper.Map<Dog>(model);

        var result = await _unitOfWork.DogRepository.UpdateAsync(entity);
        await _unitOfWork.SaveChangesAsync();

        return result;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var result = await _unitOfWork.DogRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();

        return result;
    }
    
    
    // special methods
    public List<DogModel> SortDogs(List<DogModel> dogs, string? attribute, bool? isAscendingOrder = true)
    {
        PropertyInfo? propertyInfo;

        if (string.IsNullOrEmpty(attribute))
        {
            propertyInfo = typeof(DogModel).GetProperty("Id",
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        }
        else
        {
            propertyInfo = typeof(DogModel).GetProperty(attribute,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
        }

        var sortedDogs = isAscendingOrder == true
            ? dogs.OrderBy(dog => propertyInfo.GetValue(dog, null)).ToList()
            : dogs.OrderByDescending(dog => propertyInfo.GetValue(dog, null)).ToList();

        var result = _mapper.Map<List<DogModel>>(sortedDogs);

        return result;
    }

    public List<DogModel> Pagination(List<DogModel> dogs, int? pageNumber, int? pageSize)
    {
        List<DogModel> result = new(dogs);
       
        if (pageNumber.HasValue && pageSize.HasValue)
        {
            result = dogs.Skip((pageNumber.Value - 1) * pageSize.Value).Take(pageSize.Value).ToList();
        }

        return result;
    }
}
