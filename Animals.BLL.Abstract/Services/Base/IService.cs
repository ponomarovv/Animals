namespace Animals.BLL.Abstract.Services.Base;

public interface IService<TModel> where TModel : class
{
    // Create
    Task<TModel> CreateAsync(TModel model);

    // Read
    Task<List<TModel>> GetAllAsync();
    Task<TModel> GetByIdAsync(int id);

    // Update
    Task<bool> UpdateAsync(TModel model);

    // Delete
    Task<bool> DeleteAsync(int id);
}
