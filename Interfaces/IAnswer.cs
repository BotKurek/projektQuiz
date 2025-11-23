using System.Text.Json.Serialization;
[JsonDerivedType(typeof(QuizApp.Answer), typeDiscriminator: "answer")]
public interface IAnswer
{
    string Text { get; set; }
    bool IsCorrect { get; set; }
}