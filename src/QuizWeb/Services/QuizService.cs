using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using QuizWeb.Data;
using QuizWeb.Models;

namespace QuizWeb.Services
{
    public class QuizService
    {
        private readonly QuizContext _context;
        private readonly IWebHostEnvironment _environment;

        public QuizService(QuizContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // ASYNC: Wczytywanie z pliku JSON (zewnętrzne źródło)
        public async Task SeedDataAsync()
        {
            if (await _context.Questions.AnyAsync()) 
            {
                return;
            }

            var path = Path.Combine(_environment.ContentRootPath, "gaming_quiz.json");
            if (File.Exists(path))
            {
                // Zmiana na ReadAllTextAsync
                var jsonString = await File.ReadAllTextAsync(path);
                var options = new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                };
                
                var quizData = JsonSerializer.Deserialize<QuizRoot>(jsonString, options);

                if (quizData?.Questions != null)
                {
                    await _context.Questions.AddRangeAsync(quizData.Questions);
                    await _context.SaveChangesAsync();
                }
            }
        }

        // --- CRUD ASYNC ---

        public async Task<List<Question>> GetQuestionsAsync()
        {
            // Zmiana na ToListAsync
            return await _context.Questions
                           .Include(q => q.Answers)
                           .ToListAsync();
        }

        public async Task AddQuestionAsync(Question question)
        {
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteQuestionAsync(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();
            }
        }
    }

    public class QuizRoot
    {
        public string Title { get; set; } = "";
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}