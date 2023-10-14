using Animals.DAL.Abstract.Repository;
using Animals.DAL.Impl.Context;
using Animals.DAL.Impl.Repository.Base;
using Animals.Entities;

namespace Animals.DAL.Impl.Repository;

public class DogRepository : GenericRepository<int, Dog>, IDogRepository
{
    private readonly AnimalsContext _dbContext;

    public DogRepository(AnimalsContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}
