using Animals.Entities;
using Microsoft.EntityFrameworkCore;

namespace Animals.DAL.Impl.Context;

public class AnimalsContext: DbContext
{
    public AnimalsContext(DbContextOptions<AnimalsContext> options) : base(options)
    {
    }

    public DbSet<Dog> Dogs { get; set; } = null!;
}
