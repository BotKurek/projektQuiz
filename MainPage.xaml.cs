using System;
using System.Collections.Generic;
using System.IO; // <-- Potrzebne do wczytania pliku
using System.Text.Json; // <-- Potrzebne do wczytania pliku
using System.Threading.Tasks; // <-- Potrzebne do wczytania pliku
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage; // <-- Potrzebne do wczytania pliku

namespace QuizApp;

public partial class MainPage : ContentPage
{
    // ZMIANA 1: (Konieczna dla Deserializacji JSON)
    // Zmieniamy interfejs IQuestion na konkretną klasę Question.
    // Deserializator musi wiedzieć, jaką klasę stworzyć.
    private IQuiz<Question> _quiz;
    private int _currentQuestionIndex;

    public MainPage()
    {
        InitializeComponent();

        // ZMIANA 2: (Konieczna, bo wczytywanie jest 'async')
        // Usuwamy stąd wywołanie LoadQuiz(). Nie można używać 'await' w konstruktorze.
    }

    // DODANE: (Konieczne, bo ZMIANA 2)
    // To jest właściwe miejsce na ładowanie danych, które się wczytują.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_quiz == null) // Upewnij się, że ładujesz tylko raz
        {
            await LoadQuizAsync(); // Wywołujemy naszą nową, asynchroniczną metodę
        }
    }

    // ZMIANA 3: (To jest rdzeń Twojej prośby)
    // Zastępujemy Twoje 'LoadQuiz()' tą wersją 'async Task'.
    private async Task LoadQuizAsync()
    {
        string filename = "gaming_quiz.json"; // Nazwa pliku w Resources/Raw

        try
        {
            // Otwórz plik (jako MauiAsset)
            using var stream = await FileSystem.OpenAppPackageFileAsync(filename);
            if (stream == null)
            {
                await DisplayAlert("Błąd", $"Nie znaleziono pliku quizu: '{filename}'.", "OK");
                return;
            }

            // Wczytaj go
            using var reader = new StreamReader(stream);
            string jsonString = await reader.ReadToEndAsync();

            // Przetwórz JSON. Zauważ, że używamy Quiz<Question>
            _quiz = JsonSerializer.Deserialize<Quiz<Question>>(jsonString);

            // Jeśli wszystko poszło OK, wyświetl pierwsze pytanie
            DisplayQuestion(0);
        }
        catch (Exception ex)
        {
            // Jeśli plik JSON jest zły lub coś poszło nie tak
            await DisplayAlert("Błąd krytyczny", $"Nie udało się wczytać quizu: {ex.Message}", "OK");
            QuestionLabel.Text = "Błąd ładowania quizu.";
        }
    }

    // --- PONIŻSZE METODY SĄ NIEMAL NIETKNIĘTE ---

    private void DisplayQuestion(int index)
    {
        // Sprawdzenie, czy quiz się w ogóle wczytał
        if (_quiz == null)
        {
            return; // Błąd został już pokazany w LoadQuizAsync
        }

        if (index >= _quiz.Questions.Count)
        {
            ShowResults();
            return;
        }

        _currentQuestionIndex = index;
        var question = _quiz.Questions[index];

        QuizTitleLabel.Text = _quiz.Title;
        QuestionLabel.Text = question.Text;
        AnswersStack.Children.Clear();

        for (int i = 0; i < question.Answers.Count; i++)
        {
            var answerButton = new Button
            {
                Text = question.Answers[i].Text,
                CommandParameter = i
            };
            answerButton.Clicked += OnAnswerSelected;
            AnswersStack.Children.Add(answerButton);
        }
    }

     void OnNextButtonClicked(object sender, EventArgs args)
    {
        // ... bez zmian
    }

    private void OnAnswerSelected(object? sender, EventArgs e)
    {
        var button = sender as Button;
        if (button == null) return;

        var selectedAnswerIndex = (int)button.CommandParameter;
        var question = _quiz.Questions[_currentQuestionIndex];

        if (question.CheckAnswer(selectedAnswerIndex + 1))
        {
            ResultLabel.Text = "Correct!";
            ResultLabel.TextColor = Colors.Green;
            _quiz.Score++;
        }
        else
        {
            ResultLabel.Text = "Wrong!";
            ResultLabel.TextColor = Colors.Red;
        }

        ResultLabel.IsVisible = true;
        DisplayQuestion(_currentQuestionIndex + 1);
    }

    private void ShowResults()
    {
        QuizTitleLabel.Text = "Quiz Completed!";
        QuestionLabel.Text = $"Your score: {_quiz.Score}/{_quiz.Questions.Count}";
        AnswersStack.Children.Clear();
        ResultLabel.IsVisible = false;
    }
}