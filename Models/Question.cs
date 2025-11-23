using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace QuizApp;

public class Question : IQuestion
{
    [Key]
    public int Id { get; set; }

    public string Text { get; set; }
    
    // To jest główna lista, którą ładuje EF Core i JSON
    public virtual List<Answer> Answers { get; set; } = new();

    public int QuizId { get; set; }

    [JsonIgnore] 
    [System.ComponentModel.DataAnnotations.Schema.NotMapped]
    public List<IAnswer> Answears
    { 
        get => Answers.Cast<IAnswer>().ToList(); 
        set => Answers = value.Cast<Answer>().ToList(); 
    }
    List<IAnswer> IQuestion.Answers { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public Question() { }
    public Question(string text)
    {
        Text = text;
    }

    public void AddAnswer(string text, bool isCorrect = false)
    {
        Answers.Add(new Answer(text, isCorrect));
    }

    public void Display() { } 
    
    public bool CheckAnswer(int choiceIndex)
    {
        if (choiceIndex > 0 && choiceIndex <= Answers.Count)
        {
            return Answers[choiceIndex - 1].IsCorrect;
        }
        return false;
    }
}