using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizWeb.Models;
using QuizWeb.Services;

namespace QuizWeb.Pages
{
    public class IndexModel : PageModel
    {
        private readonly QuizService _quizService;

        public IndexModel(QuizService quizService)
        {
            _quizService = quizService;
        }

        [BindProperty(SupportsGet = true)]
        public int CurrentQuestionIndex { get; set; } = 0;

        [BindProperty(SupportsGet = true)]
        public int Score { get; set; } = 0;

        public Question? CurrentQuestion { get; set; }
        public bool IsFinished { get; set; } = false;
        public int TotalQuestions { get; set; }

        public void OnGet()
        {
            var questions = _quizService.GetQuestions();
            TotalQuestions = questions.Count;

            if (questions.Any() && CurrentQuestionIndex < TotalQuestions)
            {
                CurrentQuestion = questions[CurrentQuestionIndex];
            }
            else
            {
                IsFinished = true;
            }
        }

        public IActionResult OnPost(int selectedAnswerIndex)
        {
            var questions = _quizService.GetQuestions();
            
            if (CurrentQuestionIndex < questions.Count)
            {
                var question = questions[CurrentQuestionIndex];
                if (selectedAnswerIndex >= 0 && selectedAnswerIndex < question.Answers.Count)
                {
                    if (question.Answers[selectedAnswerIndex].IsCorrect)
                    {
                        Score++;
                    }
                }
            }

            return RedirectToPage("Index", new { CurrentQuestionIndex = CurrentQuestionIndex + 1, Score = Score });
        }
    }
}