using Animals.DAL.Abstract.Repository;
using Animals.DAL.Abstract.Repository.Base;
using Animals.DAL.Impl.Context;

namespace Animals.DAL.Impl.Repository.Base;

public class UnitOfWork : IUnitOfWork
{
    private readonly AnimalsContext _context;

    public IDogRepository DogRepository { get; }
    

    public UnitOfWork(AnimalsContext context, 
        IDogRepository dogRepository)
    {
        DogRepository = dogRepository;
        _context = context;
    }



    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    private bool _disposed = false;

    protected virtual void Dispose(bool disposing)
    {
        if (!this._disposed)
        {
            if (disposing)
            {
                _context.Dispose();
            }

            this._disposed = true;
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }
}
