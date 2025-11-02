using System;
using System.Collections.Generic;

Console.WriteLine("Start programu Quiz.");

// 1. Stwórz nowy quiz
IQuiz<IQuestion> myQuiz = new Quiz<IQuestion>("Prosty Quiz");

// 2. Stwórz pytania
IQuestion q1 = new Question("Jaka jest stolica Polski?");
q1.AddAnswer("Poznań");
q1.AddAnswer("Kraków");
q1.AddAnswer("Warszawa", true);
q1.AddAnswer("Gdańsk");

IQuestion q2 = new Question("Ile wynosi 2 + 2 * 2?");
q2.AddAnswer("8");
q2.AddAnswer("6", true);
q2.AddAnswer("4");

// 3. Dodaj pytania do quizu
myQuiz.AddQuestion(q1);
myQuiz.AddQuestion(q2);

// 4. Uruchom quiz
myQuiz.Run();

Console.WriteLine("\nNaciśnij Enter, aby zakończyć...");
Console.ReadLine();


// Interfejs dla pojedynczej odpowiedzi
public interface IAnswer
{
    string Text { get; set; }
    bool IsCorrect { get; set; }
}

// Interfejs dla pojedynczego pytania
public interface IQuestion
{
    string Text { get; set; }
    IList<IAnswer> Answers { get; set; } 
    void AddAnswer(string text, bool isCorrect = false);
    void Display();
    bool CheckAnswer(int choiceIndex);
}

// Interfejs dla całego quizu
public interface IQuiz<TQuestion> where TQuestion : IQuestion
{
    string Title { get; set; }
    IList<TQuestion> Questions { get; set; } 
    int Score { get; }

    void AddQuestion(TQuestion question);
        void Run();
}

// Klasa reprezentująca odpowiedź
public class Answer : IAnswer
{
    public string Text { get; set; }
    public bool IsCorrect { get; set; }

    public Answer(string text, bool isCorrect = false)
    {
        Text = text;
        IsCorrect = isCorrect;
    }
}

// ---

// Klasa reprezentująca pytanie
public class Question : IQuestion
{
    public string Text { get; set; }
    public IList<IAnswer> Answers { get; set; }

    public Question(string text)
    {
        Text = text;
        Answers = new List<IAnswer>();
    }

    public void AddAnswer(string text, bool isCorrect = false)
    {
        Answers.Add(new Answer(text, isCorrect));
    }

    public void Display()
    {
        Console.WriteLine(Text);
        for (int i = 0; i < Answers.Count; i++)
        {
            Console.WriteLine($"  {i + 1}. {Answers[i].Text}");
        }
    }

    public bool CheckAnswer(int choiceIndex)
    {
        if (choiceIndex > 0 && choiceIndex <= Answers.Count)
        {
            return Answers[choiceIndex - 1].IsCorrect;
        }
        
        return false;
    }
}

// ---

// Klasa reprezentująca quiz
public class Quiz<TQuestion> : IQuiz<TQuestion> where TQuestion : IQuestion
{
    public string Title { get; set; }
    public IList<TQuestion> Questions { get; set; } // Implementacja używa TQuestion
    public int Score { get; private set; }

    public Quiz(string title)
    {
        Title = title;
        Questions = new List<TQuestion>(); // Tworzymy listę konkretnego typu
        Score = 0;
    }

    public void AddQuestion(TQuestion question) // Metoda przyjmuje TQuestion
    {
        Questions.Add(question);
    }

    public void Run()
    {
        Score = 0; 
        Console.WriteLine($"--- Witaj w quizie: {Title}! ---");
        Console.WriteLine();

        foreach (var question in Questions)
        {
            question.Display(); 
            Console.Write("Twoja odpowiedź (podaj numer 1, 2, 3...): ");
            
            string userInput = Console.ReadLine() ?? ""; 

            if (int.TryParse(userInput, out int userChoice))
            {
                if (question.CheckAnswer(userChoice))
                {
                    Console.WriteLine("Poprawna odpowiedź! 👍");
                    Score++;
                }
                else
                {
                    Console.WriteLine("Niestety, zła odpowiedź. 👎");
                }
            }
            else
            {
                Console.WriteLine("To nie jest poprawny numer. Tracisz punkt.");
            }
            Console.WriteLine(); 
        }
        
        ShowResults();
    }

    // Metoda prywatna do wyświetlania wyników
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