using Microsoft.EntityFrameworkCore;
using QuizWeb.Data;
using QuizWeb.Services;

var builder = WebApplication.CreateBuilder(args);

// 1. Konfiguracja bazy danych (SQLite)
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<QuizContext>(options =>
    options.UseSqlite(connectionString));

// 2. Rejestracja serwisów
builder.Services.AddRazorPages();
builder.Services.AddScoped<QuizService>(); // To zostaje, ale zmienimy jego kod

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

// Utwórz zakres (scope), pobierz serwis i uruchom seedowanie
using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<QuizService>();
    
    // Najpierw upewnij się, że baza jest utworzona
    var context = scope.ServiceProvider.GetRequiredService<QuizContext>();
    context.Database.EnsureCreated();
    
    // Wypełnij danymi z JSON
    service.SeedData();
}

app.Run();
