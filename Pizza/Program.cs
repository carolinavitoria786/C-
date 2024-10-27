using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PizzaStoreApi.Data;
using PizzaStoreApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pizza Store API", Description = "Fazendo Pizzas por amor", Version = "v1" });
    }
);

var connectionString= builder.Configuration.GetConnectionString("Massas") ?? "Data Source=Pizza.db";
builder.Services.AddSqlite<AppDbContext>(connectionString);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(
    c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pizza")
  );
}

app.UseHttpsRedirection();

app.MapGet("/pizzas", async (AppDbContext db) => await db.Pizzas.ToListAsync());

app.MapGet("/pizza/{id}", async (AppDbContext db, int id) => await db.Pizzas.FindAsync(id));

app.MapPut("/pizza/{id}", async (AppDbContext
db, Pizza updatepizza, int id) =>
{
  var pizza = await db.Pizzas.FindAsync(id);
  if (pizza is null) return
  Results.NotFound();
  pizza.Nome = updatepizza.Nome;
  pizza.Tamanho = updatepizza.Tamanho;
  pizza.Preco = updatepizza.Preco;
  await db.SaveChangesAsync();
  return Results.NoContent();
});

app.MapDelete("/pizza/{id}", async
(AppDbContext db, int id) =>
{
  var pizza = await db.Pizzas.FindAsync(id);
  if (pizza is null)
  {
    return Results.NotFound();
  }
  db.Pizzas.Remove(pizza);
  await db.SaveChangesAsync();
  return Results.Ok();
});

app.MapPost("/pizza", async
(AppDbContext db, Pizza pizza) =>
{

  await db.Pizzas.AddAsync(pizza);
  await db.SaveChangesAsync();
  return

  Results.Created($"/pizza/{pizza.Id}",
  pizza);
});

app.Run();
