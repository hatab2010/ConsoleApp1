using ConsoleApp1.Data;
using Microsoft.EntityFrameworkCore;

public class DomainDbContext : DbContext
{
    public DbSet<Item> Items { get; set; }
    public DbSet<Category> Categories { get; set; }

    public DomainDbContext()
    {

    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Filename=C:\\Users\\cowor\\source\\repos\\ConsoleApp1\\ConsoleApp1\\Mobile.db");
    }

    public DomainDbContext(DbContextOptions<DomainDbContext> options)
        : base(options)
    {
        Database.EnsureCreated();
    }
}
