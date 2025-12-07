namespace QuizWeb.Models
{
    public class Answer
    {
        public int Id { get; set; } // Klucz główny bazy
        public string Text { get; set; } = "";
        public bool IsCorrect { get; set; }
        
        // Relacja do pytania (Klucz obcy)
        public int QuestionId { get; set; }
    }
}