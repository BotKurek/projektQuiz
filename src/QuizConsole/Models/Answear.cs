public class Answer : IAnswer
{
    public string Text { get; set; }
    public bool IsCorrect { get; set; }

    public Answer() { Text = ""; } 
    public Answer(string text, bool isCorrect = false)
    {
        Text = text;
        IsCorrect = isCorrect;
    }
}