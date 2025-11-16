using System.Collections.Generic;
using Microsoft.Maui.Controls;

namespace QuizApp;

public partial class MainPage : ContentPage
{
    private IQuiz<IQuestion> _quiz;
    private int _currentQuestionIndex;

    public MainPage()
    {
        InitializeComponent();
        LoadQuiz();
    }

    

    private void LoadQuiz()
    {
        // Load the quiz (replace with your actual loading logic)
        _quiz = new Quiz<IQuestion>("Sample Quiz");
        var question1 = new Question("What is 2 + 2?");
        question1.AddAnswer("3", false);
        question1.AddAnswer("4", true);
        question1.AddAnswer("5", false);
        _quiz.AddQuestion(question1);

        var question2 = new Question("What is the capital of France?");
        question2.AddAnswer("Berlin", false);
        question2.AddAnswer("Paris", true);
        question2.AddAnswer("Madrid", false);
        _quiz.AddQuestion(question2);

        DisplayQuestion(0);
    }

    private void DisplayQuestion(int index)
    {
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
        // await label.RelRotateToAsync(360, 1000);
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