using Animals.DAL.Abstract.Repository.Base;
using Animals.Entities;

namespace Animals.DAL.Abstract.Repository;

public interface IDogRepository: IGenericRepository<int, Dog>
{
    
}

