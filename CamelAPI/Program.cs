using CamelAPI;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CamelDb>(options => 
    options.UseSqlite("Data Source=camels.db"));

builder.Services.AddEndpointsApiExplorer(); 
builder.Services.AddSwaggerGen();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();
var app = builder.Build();

using (var scope = app.Services.CreateScope()) 
{
    var db = scope.ServiceProvider.GetRequiredService<CamelDb>(); 
    db.Database.EnsureCreated(); 
}

app.UseSwagger(); 
app.UseSwaggerUI();

app.MapPost("/camels", async (Camel camel, CamelDb db) => { 
    if (camel.HumpCount is not 1 and not 2)
        return Results.BadRequest("HumpCount must be 1 or 2."); 
    db.Camels.Add(camel); 
    await db.SaveChangesAsync(); 
    return Results.Created($"/camels/{camel.Id}", camel); 
});

app.MapGet("/camels", async (CamelDb db) =>
    await db.Camels.ToListAsync()
);

app.MapGet("/camels/{id}", async (int id, CamelDb db) =>
{
    var camel = await db.Camels.FindAsync(id);
    return camel is not null ? Results.Ok(camel) : Results.NotFound();
});          

app.MapPut("/camels/{id}", async (int id, Camel updatedCamel, CamelDb db) =>
{
    if (updatedCamel.HumpCount is not 1 and not 2)
        return Results.BadRequest("HumpCount must be 1 or 2.");
    var camel = await db.Camels.FindAsync(id);
    if (camel is null)
        return Results.NotFound();
    camel.Name = updatedCamel.Name;
    camel.Color = updatedCamel.Color;
    camel.HumpCount = updatedCamel.HumpCount;
    camel.LastFed = updatedCamel.LastFed;
    await db.SaveChangesAsync();
    return Results.Ok(camel);
});

app.MapDelete("/camels/{id}", async (int id, CamelDb db) =>
{
    var camel = await db.Camels.FindAsync(id);
    if (camel is null)
        return Results.NotFound();
    db.Camels.Remove(camel);
    await db.SaveChangesAsync();
    return Results.NoContent();
});

app.Run();


