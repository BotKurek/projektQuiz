using System;
using System.IO;
using System.Text.Json;
using System.Reflection;

Console.WriteLine("Start programu Quiz.");

string exePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
string filename = Path.Combine(exePath, "gaming_quiz.json");
IQuiz<IQuestion>? loadedQuiz = null;

try
{
    if (File.Exists(filename))
    {
        string jsonString = File.ReadAllText(filename);
        loadedQuiz = JsonSerializer.Deserialize<Quiz<IQuestion>>(jsonString);
        Console.WriteLine($"[INFO] Quiz został pomyślnie wczytany z pliku: {filename}\n");
    }
    else
    {
        Console.WriteLine($"[BŁĄD] Nie znaleziono pliku quizu '{filename}'.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"[BŁĄD] Nie udało się wczytać lub przetworzyć quizu: {ex.Message}");
}

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