using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using PetsApi.Models;

namespace PetsApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) {}
    public DbSet<Animal> Pets { get; set; } = null!;

}