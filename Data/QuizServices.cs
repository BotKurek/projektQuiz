using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Maui.Storage;
using QuizApp; 

namespace QuizApp;
public class QuizService
{
    private QuizDbContext _context;

    public QuizService()
    {
        _context = new QuizDbContext();
    }


    // 1. Dodawanie quizu
    public async Task AddQuizAsync(Quiz quiz)
    {
        _context.Quizzes.Add(quiz);
        await _context.SaveChangesAsync();
    }

    // 2. Pobieranie wszystkich quizów
    public async Task<List<Quiz>> GetAllQuizzesAsync()
    {
        return await _context.Quizzes
                             .Include(q => q.Questions)
                             .ToListAsync();
    }

    // 3. Pobieranie szczegółów quizu
    public async Task<Quiz?> GetQuizWithDetailsAsync(int id)
    {
        return await _context.Quizzes
                             .Include(q => q.Questions)
                             .ThenInclude(que => que.Answers)
                             .FirstOrDefaultAsync(q => q.Id == id);
    }

    // 4. Aktualizacja
    public async Task UpdateQuizAsync(Quiz quiz)
    {
        _context.Quizzes.Update(quiz);
        await _context.SaveChangesAsync();
    }

    // 5. Usuwanie
    public async Task DeleteQuizAsync(Quiz quiz)
    {
        _context.Quizzes.Remove(quiz);
        await _context.SaveChangesAsync();
    }

    // --- SEEDING (Wczytywanie danych) ---
    // Ta metoda musi być wewnątrz klasy QuizService, ale POZA innymi metodami
    public async Task SeedDataAsync()
    {
        // Sprawdzamy, czy baza ma już dane. Jeśli tak, przerywamy.
        if (await _context.Quizzes.AnyAsync())
        {
            return;
        }

        try 
        {
            // Odczyt pliku JSON z zasobów
            using var stream = await FileSystem.OpenAppPackageFileAsync("gaming_quiz.json");
            using var reader = new StreamReader(stream);
            string jsonString = await reader.ReadToEndAsync();

            var options = new JsonSerializerOptions 
            { 
                PropertyNameCaseInsensitive = true 
            };
            
            var quizFromJson = JsonSerializer.Deserialize<Quiz>(jsonString, options);

            if (quizFromJson != null)
            {
                // Dodajemy quiz do bazy
                await AddQuizAsync(quizFromJson);
            }
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Błąd seedowania: {ex.Message}");
        }
    }

    public async Task<List<Quiz>> SearchQuizzesAsync(string searchTerm)
    {
  
        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            return await GetAllQuizzesAsync();
        }

        
        return await _context.Quizzes
                             .Include(q => q.Questions)
                             .Where(q => q.Title.Contains(searchTerm))
                             .ToListAsync();
    }
}