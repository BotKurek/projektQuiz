using System.Text.Json.Serialization;
[JsonDerivedType(typeof(Answear), typeDiscriminator: "answear")]
public interface IAnswear
{
    string Text { get; set; }
    bool IsCorrect { get; set; }
}