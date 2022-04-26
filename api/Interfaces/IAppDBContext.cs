using api.Entities;
using Microsoft.EntityFrameworkCore;

namespace api.Interfaces
{
    public interface IAppDBContext
    {
        DbSet<Categorias> Categorias { get; set; }
        DbSet<Post> Post { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}