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

        // Metoda inicjalizująca (Seeding) - przenosi dane z JSON do Bazy
        public void SeedData()
        {
            // Jeśli baza już coś zawiera, nie robimy nic
            if (_context.Questions.Any()) 
            {
                return;
            }

            var path = Path.Combine(_environment.ContentRootPath, "gaming_quiz.json");
            if (File.Exists(path))
            {
                var jsonString = File.ReadAllText(path);
                var options = new JsonSerializerOptions 
                { 
                    PropertyNameCaseInsensitive = true 
                };
                
                // Tutaj używamy klasy QuizRoot, która jest zdefiniowana poniżej
                var quizData = JsonSerializer.Deserialize<QuizRoot>(jsonString, options);

                if (quizData?.Questions != null)
                {
                    _context.Questions.AddRange(quizData.Questions);
                    _context.SaveChanges();
                }
            }
        }

        // --- CRUD (Metody do obsługi bazy danych) ---

        public List<Question> GetQuestions()
        {
            // Pobieramy pytania wraz z odpowiedziami (Include)
            return _context.Questions
                           .Include(q => q.Answers)
                           .ToList();
        }

        public void AddQuestion(Question question)
        {
            _context.Questions.Add(question);
            _context.SaveChanges();
        }

        public void DeleteQuestion(int id)
        {
            var question = _context.Questions.Find(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
                _context.SaveChanges();
            }
        }
    }

    // 👇 TEJ KLASY BRAKOWAŁO - DODAJ JĄ NA KOŃCU PLIKU 👇
    public class QuizRoot
    {
        public string Title { get; set; } = "";
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}