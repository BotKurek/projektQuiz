using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizWeb.Models;
using QuizWeb.Services;

namespace QuizWeb.Pages
{
    public class ManageModel : PageModel
    {
        private readonly QuizService _quizService;

        public ManageModel(QuizService quizService)
        {
            _quizService = quizService;
        }

        public List<Question> Questions { get; set; } = new List<Question>();

        // Dane z formularza dodawania
        [BindProperty]
        public string NewQuestionText { get; set; } = "";
        
        [BindProperty]
        public List<string> NewAnswers { get; set; } = new List<string> { "", "", "", "" };
        
        [BindProperty]
        public int CorrectAnswerIndex { get; set; } = 0;

    
        public void OnGet()
        {
            Questions = _quizService.GetQuestions();
        }

        // CREATE - Dodaj pytanie
        public IActionResult OnPostAdd()
        {
            var newQuestion = new Question
            {
                Text = NewQuestionText,
                Answers = new List<Answer>()
            };

            // Zamieniamy wpisane teksty na obiekty Answer
            for (int i = 0; i < 4; i++)
            {
                newQuestion.Answers.Add(new Answer
                {
                    Text = NewAnswers[i],
                    IsCorrect = (i == CorrectAnswerIndex)
                });
            }

            _quizService.AddQuestion(newQuestion);

            return RedirectToPage();
        }

        // DELETE - Usuń pytanie
        public IActionResult OnPostDelete(int id)
        {
        
            _quizService.DeleteQuestion(id);
            
            return RedirectToPage();
        }
    }
}