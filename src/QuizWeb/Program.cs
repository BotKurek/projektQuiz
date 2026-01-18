using Microsoft.EntityFrameworkCore;
using QuizWeb.Components;
using QuizWeb.Data;
using QuizWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Konfiguracja bazy danych (SQLite)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<QuizContext>(options =>
    options.UseSqlite(connectionString));

// 2. Rejestracja serwisów
builder.Services.AddRazorPages();
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

    builder.Services.AddScoped<QuizService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles(); 
app.UseAntiforgery();


app.MapRazorPages();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<QuizService>();
    var context = scope.ServiceProvider.GetRequiredService<QuizContext>();
    
  
    context.Database.EnsureCreated();
    
    await service.SeedDataAsync();
}

app.Run();