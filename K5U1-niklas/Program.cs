using Azure.Identity;
using K5U1_niklas.Data;
using K5U1_niklas.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;
// using Azure.Identity; // TODO (Del 4): Kr�vs f�r Key Vault

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddOpenApi();

// 1. Läs in KeyVaultUrl från konfigurationen
var keyVaultUrl = builder.Configuration["KeyVaultUrl"];

// 2. Om KeyVaultUrl är satt, koppla in Azure Key Vault som config provider
if (!string.IsNullOrEmpty(keyVaultUrl))
{
    builder.Configuration.AddAzureKeyVault(
        new Uri(keyVaultUrl),
        new DefaultAzureCredential());
}



// TODO (Del 4 i "Tips och f�rslag"): Konfigurera Azure Key Vault
// Anv�nd Managed Identity f�r att h�mta hemligheter i produktion.
// if (builder.Environment.IsProduction())
// {
//     var keyVaultUrl = new Uri(builder.Configuration["KeyVaultUrl"]!);
//     builder.Configuration.AddAzureKeyVault(keyVaultUrl, new DefaultAzureCredential());
// }

// Vi anv�nder InMemory-databas lokalt
builder.Services.AddHealthChecks();

builder.Services.AddDbContext<InventoryDbContext>(options =>
    options.UseInMemoryDatabase("InventoryDb"));

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseAuthentication();
app.UseAuthorization();
app.MapHealthChecks("/health");
app.MapControllers();

// Seeda data (se till att vi inte dubblar om appen startas om i samma process)
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<InventoryDbContext>();

    if (!db.Products.Any())
    {
        db.Products.Add(new Product { Id = 1, Name = "Laptop", Price = 9999, StockQuantity = 10 });
        db.SaveChanges();
    }
}

app.Run();