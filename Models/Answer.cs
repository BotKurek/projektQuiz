using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace QuizApp;

public class Answer : IAnswer 
{
    [Key]
    public int Id { get; set; }

    public string Text { get; set; }
    public bool IsCorrect { get; set; }

    public int QuestionId { get; set; }

    public Answer() { Text = ""; }
    
    public Answer(string text, bool isCorrect = false)
    {
        Text = text;
        IsCorrect = isCorrect;
    }
}