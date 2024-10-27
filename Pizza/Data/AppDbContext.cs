using System.Data.Common;
using Microsoft.EntityFrameworkCore;
using PizzaStoreApi.Models;

namespace PizzaStoreApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options) {}
    public DbSet<Pizza> Pizzas { get; set; } = null!;

}