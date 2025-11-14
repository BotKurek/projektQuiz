using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Reflection;

// === KOD STARTOWY ===

Console.WriteLine("Start programu Quiz.");

string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;

// Łączy tę ścieżkę z nazwą pliku
string filename = Path.Combine(exePath, "gaming_quiz.json");
IQuiz<IQuestion>? loadedQuiz = null; 

// === WCZYTANIE Z PLIKU ===
try
{
    if (File.Exists(filename))
    {
        string jsonString = File.ReadAllText(filename);
        
        // Teraz to zadziała, bo interfejsy mają atrybuty
        loadedQuiz = JsonSerializer.Deserialize<Quiz<IQuestion>>(jsonString);
        
        Console.WriteLine($"[INFO] Quiz został pomyślnie wczytany z pliku: {filename}\n");
    }
    else
    {
        Console.WriteLine($"[BŁĄD] Nie znaleziono pliku quizu '{filename}'.");
        Console.WriteLine("Upewnij się, że plik JSON z quizem znajduje się w tym samym folderze co program.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"[BŁĄD] Nie udało się wczytać lub przetworzyć quizu: {ex.Message}");
}

// === URUCHOMIENIE QUIZU ===
if (loadedQuiz != null)
{
    loadedQuiz.Run();
}
else
{
    Console.WriteLine("Nie udało się uruchomić quizu, ponieważ nie został poprawnie wczytany.");
}


Console.WriteLine("\nNaciśnij Enter, aby zakończyć...");
Console.ReadLine();


// === INTERFEJSY ===

// Interfejs dla pojedynczej odpowiedzi
// POPRAWKA: Dodajemy atrybuty mówiące, że IAnswer jest implementowane przez klasę Answer
[JsonDerivedType(typeof(Answer), typeDiscriminator: "answer")]
public interface IAnswer
{
    string Text { get; set; }
    bool IsCorrect { get; set; }
}

// Interfejs dla pojedynczego pytania
// POPRAWKA: Dodajemy atrybuty mówiące, że IQuestion jest implementowane przez klasę Question
[JsonDerivedType(typeof(Question), typeDiscriminator: "question")]
public interface IQuestion
{
    List<IAnswer> Answers { get; set; } // Zmienione z IList na List dla serializacji
    string Text { get; set; }
    void AddAnswer(string text, bool isCorrect = false);
    void Display();
    bool CheckAnswer(int choiceIndex);
}

// Interfejs dla całego quizu
public interface IQuiz<TQuestion> where TQuestion : IQuestion
{
    string Title { get; set; }
    List<TQuestion> Questions { get; set; } // Zmienione z IList na List dla serializacji
    int Score { get; }
    void AddQuestion(TQuestion question);
    void Run();
}

// === KLASY ===

// Klasa reprezentująca odpowiedź
public class Answer : IAnswer
{
    public string Text { get; set; }
    public bool IsCorrect { get; set; }

    public Answer() { Text = ""; } 
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
    public List<IAnswer> Answers { get; set; }

    public Question() 
    {
        Text = "";
        Answers = new List<IAnswer>();
    }

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
    public List<TQuestion> Questions { get; set; }
    public int Score { get; private set; }

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
            
            int userChoice = 0; // Przechowa poprawny wybór użytkownika
            bool isValidInput = false; // Flaga do kontrolowania pętli

            // Ta pętla będzie działać, dopóki użytkownik nie poda poprawnej liczby
            while (!isValidInput)
            {
                // Pytamy o odpowiedź, podając zakres (np. 1-4)
                Console.Write($"Twoja odpowiedź (podaj numer 1-{question.Answers.Count}): ");
                string userInput = Console.ReadLine() ?? ""; 

                // 1. Sprawdzamy, czy to w ogóle jest liczba
                if (int.TryParse(userInput, out userChoice))
                {
                    // 2. Jeśli tak, sprawdzamy, czy jest w poprawnym zakresie
                    if (userChoice > 0 && userChoice <= question.Answers.Count)
                    {
                        // Sukces! To jest poprawna odpowiedź (np. 1, 2, 3 lub 4)
                        isValidInput = true; // To zakończy pętlę 'while'
                    }
                    else
                    {
                        // To jest liczba, ale zła (np. 0, 5, 6)
                        Console.WriteLine($"Błędny numer! Proszę podać liczbę od 1 do {question.Answers.Count}.");
                    }
                }
                else
                {
                    // To w ogóle nie jest liczba (np. "abc")
                    Console.WriteLine("To nie jest poprawny numer. Spróbuj jeszcze raz.");
                }
            }

            // Po wyjściu z pętli 'while' mamy pewność, że 'userChoice' jest poprawną liczbą.
            // Teraz dopiero sprawdzamy, czy jest to dobra odpowiedź.

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