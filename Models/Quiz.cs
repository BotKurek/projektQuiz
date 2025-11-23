using System.ComponentModel.DataAnnotations;

namespace QuizApp;

public class Quiz : IQuiz<Question>
{
    [Key]
    public int Id { get; set; }
    public string Title { get; set; }
    public int Score { get; set; }
    
    public virtual List<Question> Questions { get; set; } = new();

    public Quiz(string title) { Title = title; }

    public void AddQuestion(Question question)
    {
        Questions.Add(question);
    }

    public void Run() { /* ... */ }
}