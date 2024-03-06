using API.Entities;
using Microsoft.EntityFrameworkCore;

//The C# Dev Kit does not add the Namespace within the right folder correctly
namespace API.Data;
public class DataContext : DbContext
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<AppUser> Users { get; set; }
}