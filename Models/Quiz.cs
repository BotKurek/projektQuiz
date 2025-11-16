public class Quiz<TQuestion> : IQuiz<TQuestion> where TQuestion : IQuestion
{
    public string Title { get; set; }
    public List<TQuestion> Questions { get; set; }
    public int Score { get; set; }

    public Quiz()
    {
        Title = "";
        Questions = new List<TQuestion>();
        Score = 0;
    }

    public Quiz(string title)
    {
        Title = title;
        Questions = new List<TQuestion>();
        Score = 0;
    }

    public void AddQuestion(TQuestion question)
    {
        Questions.Add(question);
    }

    public void Run()
    {
        Score = 0; 
        Console.WriteLine($"--- Witaj w quizie: {Title}! ---");
        Console.WriteLine();

        if (Questions.Count == 0)
        {
            Console.WriteLine("Wczytany quiz nie zawiera żadnych pytań.");
            return;
        }

        foreach (var question in Questions)
        {
            question.Display(); 
            
            int userChoice = 0;
            bool isValidInput = false;

            while (!isValidInput)
            {
                Console.Write($"Twoja odpowiedź (podaj numer 1-{question.Answears.Count}): ");
                string userInput = Console.ReadLine() ?? ""; 

                if (int.TryParse(userInput, out userChoice))
                {
                    if (userChoice > 0 && userChoice <= question.Answears.Count)
                    {
                        isValidInput = true;
                    }
                    else
                    {
                        Console.WriteLine($"Błędny numer! Proszę podać liczbę od 1 do {question.Answears.Count}.");
                    }
                }
                else
                {
                    Console.WriteLine("To nie jest poprawny numer. Spróbuj jeszcze raz.");
                }
            }

            if (question.CheckAnswer(userChoice))
            {
                Console.WriteLine("Poprawna odpowiedź! 👍");
                Score++;
            }
            else
            {
                Console.WriteLine("Niestety, zła odpowiedź. 👎");
            }
            Console.WriteLine(); 
        }
        
        ShowResults();
    }

    private void ShowResults()
    {
        Console.WriteLine("--- Koniec quizu! ---");
        Console.WriteLine($"Twój ostateczny wynik: {Score} na {Questions.Count} pytań.");
        
        if (Questions.Count > 0)
        {
            double percentage = (double)Score / Questions.Count * 100;
            Console.WriteLine($"Uzyskałeś: {percentage:F2}%"); 
        }
    }
}