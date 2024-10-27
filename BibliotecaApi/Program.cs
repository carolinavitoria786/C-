using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using BibliotecaApi.Data;
using BibliotecaApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(
    c =>
    {
      c.SwaggerDoc("v1", new OpenApiInfo { Title = "Libraries API", Description = "Desbravando as barreiras do conhecimento", Version = "v1" });
    }
);

var connectionString= builder.Configuration.GetConnectionString("Bibliotecas") ?? "Data Source=Biblioteca.db";
builder.Services.AddSqlite<AppDbContext>(connectionString);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
  app.UseSwagger();
  app.UseSwaggerUI(
    c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Biblioteca")
  );
}

app.UseHttpsRedirection();

app.MapGet("/bibliotecas", async (AppDbContext db) => await db.Bibliotecas.ToListAsync());

app.MapGet("/biblioteca/{id}", async (AppDbContext db, int id) => await db.Bibliotecas.FindAsync(id));

app.MapPut("/biblioteca/{id}", async (AppDbContext
db, Biblioteca updatebiblioteca, int id) =>
{
  var biblioteca = await db.Bibliotecas.FindAsync(id);
  if (biblioteca is null) return
  Results.NotFound();
  biblioteca.nome = updatebiblioteca.nome;
  biblioteca.inicio_funcionamento = updatebiblioteca.inicio_funcionamento;
  biblioteca.fim_funcionamento = updatebiblioteca.fim_funcionamento;
  biblioteca.inauguracao = updatebiblioteca.inauguracao;
  biblioteca.contato = updatebiblioteca.contato;
  await db.SaveChangesAsync();
  return Results.NoContent();
});

app.MapDelete("/biblioteca/{id}", async
(AppDbContext db, int id) =>
{
  var biblioteca = await db.Bibliotecas.FindAsync(id);
  if (biblioteca is null)
  {
    return Results.NotFound();
  }
  db.Bibliotecas.Remove(biblioteca);
  await db.SaveChangesAsync();
  return Results.Ok();
});

app.MapPost("/biblioteca", async
(AppDbContext db, Biblioteca biblioteca) =>
{

  await db.Bibliotecas.AddAsync(biblioteca);
  await db.SaveChangesAsync();
  return

  Results.Created($"/biblioteca/{biblioteca.id}",
  biblioteca);
});

app.Run();