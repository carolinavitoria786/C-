using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PetsApi.Data;
using PetsApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Pets API", Description = "Uma API que cuida do seu pequeno!", Version = "v1" });
});

builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("petsdb"));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Pets API"));
}

app.UseHttpsRedirection();


app.MapGet("/pets", async (AppDbContext db) => await db.Pets.ToListAsync()); //Fazer uma requisição para pegar os dados no db.
app.MapGet("/pets/{id}", async (AppDbContext db, int id) =>
{
    var pet = await db.Pets.FindAsync(id);
    return pet is not null ? Results.Ok(pet) : Results.NotFound();
});


app.MapPost("/pets", async (AppDbContext db, Animal pet) => //Inserção de pets
{
    await db.Pets.AddAsync(pet);
    await db.SaveChangesAsync();
    return Results.Created($"/pets/{pet.id}", pet);
});


app.MapPut("/pets/{id}", async (AppDbContext db, Animal updateAnimal, int id) => //Edição de pets
{
    var pet = await db.Pets.FindAsync(id);
    if (pet is null) return Results.NotFound();

    // Atualização da classe:
    pet.nome = updateAnimal.nome;
    pet.idade = updateAnimal.idade;
    pet.cor = updateAnimal.cor;
    pet.tipo = updateAnimal.tipo;
    pet.peso_kg = updateAnimal.peso_kg;
    pet.vacinado = updateAnimal.vacinado;

    await db.SaveChangesAsync();
    return Results.NoContent();
});


app.MapDelete("/pets/{id}", async (AppDbContext db, int id) => //Deletar 1 pet.
{
    var pet = await db.Pets.FindAsync(id);
    if (pet is null) return Results.NotFound();

    db.Pets.Remove(pet);
    await db.SaveChangesAsync();
    return Results.Ok();
});

app.Run();