using Microsoft.EntityFrameworkCore;
using System.IO;

namespace QuizApp;

public class QuizDbContext : DbContext
{
    public DbSet<Quiz> Quizzes { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<Answer> Answers { get; set; }

    public QuizDbContext()
    {
        // Tworzy bazę, jeśli nie istnieje
        Database.EnsureCreated();
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Ścieżka do bazy danych na urządzeniu
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "quiz_database.db");
        
        optionsBuilder.UseSqlite($"Filename={dbPath}");
    }
}