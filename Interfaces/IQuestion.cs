using System.Text.Json.Serialization;

[JsonDerivedType(typeof(Question), typeDiscriminator: "question")]
public interface IQuestion
{
    List<IAnswear> Answears { get; set; }
    string Text { get; set; }
    void AddAnswer(string text, bool isCorrect = false);
    void Display();
    bool CheckAnswer(int choiceIndex);
}