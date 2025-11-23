using System;
using System.Collections.Generic;
using System.IO; 
using System.Text.Json; 
using System.Threading.Tasks; 
using Microsoft.Maui.Controls;
using Microsoft.Maui.Storage; 

namespace QuizApp;

public partial class MainPage : ContentPage
{
    private IQuiz<Question> _quiz; 
    private int _currentQuestionIndex;
    private readonly QuizService _quizService; // Serwis bazy danych

    public MainPage()
    {
        InitializeComponent();
        _quizService = new QuizService();
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        if (_quiz == null)
        {
            await LoadQuizFromDbAsync();
        }
    }

    private async Task LoadQuizFromDbAsync()
    {
        try
        {
            // 1. Załaduj przykładowe dane, jeśli baza jest pusta
            await _quizService.SeedDataAsync();

            // 2. Pobierz dostępny quiz
            var quizzes = await _quizService.GetAllQuizzesAsync();
            var firstQuiz = quizzes.FirstOrDefault();

            if (firstQuiz != null)
            {
                // 3. Pobierz pełne dane (pytania i odpowiedzi)
                _quiz = await _quizService.GetQuizWithDetailsAsync(firstQuiz.Id);
                
                if (_quiz != null && _quiz.Questions.Count > 0)
                {
                    DisplayQuestion(0);
                    return;
                }
            }
            
            QuestionLabel.Text = "Brak quizów w bazie danych.";
        }
        catch (Exception ex)
        {
            await DisplayAlert("Błąd Bazy", ex.Message, "OK");
        }
    }

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

    private async void OnQuizSearchPressed(object sender, EventArgs e)
    {
        string searchTerm = QuizSearch.Text;

        try
        {
            // Wywołujemy naszą nową metodę z LINQ
            var foundQuizzes = await _quizService.SearchQuizzesAsync(searchTerm);

            if (foundQuizzes.Count > 0)
            {
                // Jeśli znaleziono, załaduj pierwszy pasujący quiz
                var quizSummary = foundQuizzes.First();
                
                // Pobierz szczegóły (odpowiedzi) dla tego quizu
                _quiz = await _quizService.GetQuizWithDetailsAsync(quizSummary.Id);
                
                // Zresetuj grę i pokaż pierwsze pytanie znalezionego quizu
                _currentQuestionIndex = 0;
                DisplayQuestion(0);
            }
            else
            {
                await DisplayAlert("Wynik", "Nie znaleziono quizu o takiej nazwie.", "OK");
            }
        }
        catch (Exception ex)
        {
            await DisplayAlert("Błąd", ex.Message, "OK");
        }
    }
}