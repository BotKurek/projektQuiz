namespace QuizWeb.Models
{
    public class Question
    {
        public int Id { get; set; } // Klucz główny bazy
        public string Text { get; set; } = "";
        public List<Answer> Answers { get; set; } = new List<Answer>();
    }
}