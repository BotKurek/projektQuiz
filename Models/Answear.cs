public class Answear : IAnswear
{
    public string Text { get; set; }
    public bool IsCorrect { get; set; }

    public Answear() { Text = ""; } 
    public Answear(string text, bool isCorrect = false)
    {
        Text = text;
        IsCorrect = isCorrect;
    }
}