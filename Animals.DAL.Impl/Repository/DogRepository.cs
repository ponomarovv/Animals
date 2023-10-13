using Animals.DAL.Abstract.Repository;
using Animals.DAL.Impl.Context;
using Animals.Entities;
using StoreCS.DAL.Impl.Repository.Base;

namespace Animals.DAL.Impl.Repository;

public class DogRepository : GenericRepository<int, Dog>, IDogRepository
{
    private readonly AnimalsContext _dbContext;

    public DogRepository(AnimalsContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
