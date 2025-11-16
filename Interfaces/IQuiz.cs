public interface IQuiz<TQuestion> where TQuestion : IQuestion
{
    string Title { get; set; }
    List<TQuestion> Questions { get; set; }
    int Score { get; set; }
    void AddQuestion(TQuestion question);
    void Run();
}