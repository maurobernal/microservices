using api.Entities;
using api.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;

namespace api.Context;
public class AppDBContext : DbContext, IAppDBContext
{
    public AppDBContext(DbContextOptions<AppDBContext> options) : base(options)
    {

    }
    public DbSet<Post> Post { get; set; }
    public DbSet<Categorias> Categorias { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(
            @"Server=127.0.0.1,14333;Database=API;user id=sa;password=M1sterPassw0rd!;");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        return base.SaveChangesAsync(cancellationToken);
        
    }

}
