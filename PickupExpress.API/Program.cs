using Microsoft.EntityFrameworkCore;
using PickupExpress.Core.Interfaces;
using PickupExpress.Infrastructure.Data;
using PickupExpress.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddOpenApi();

// Add services to the container.
builder.Services.AddControllers().AddNewtonsoftJson(); // Required for JSON Patch support

// Configure SQLite Database
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseRouting();
app.UseAuthorization();
app.MapControllers();

// Ensure that you are running the in the API folder
app.Run();

