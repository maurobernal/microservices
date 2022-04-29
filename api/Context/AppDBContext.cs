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
            @"Server=mssql;Database=API;user id=sa;password=M1st2rPassw0rd!;");
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
    {
        return base.SaveChangesAsync(cancellationToken);
        
    }

}
