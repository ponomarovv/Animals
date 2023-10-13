namespace Animals.DAL.Abstract.Repository.Base;

public interface IUnitOfWork : IDisposable
{
    IDogRepository DogRepository { get; }
   
    Task SaveChangesAsync();
}
