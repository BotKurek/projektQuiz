using System.Text.Json.Serialization;

[JsonDerivedType(typeof(Question), typeDiscriminator: "question")]
public interface IQuestion
{
    List<IAnswer> Answers { get; set; }
    string Text { get; set; }
    void AddAnswer(string text, bool isCorrect = false);
    void Display();
    bool CheckAnswer(int choiceIndex);
}