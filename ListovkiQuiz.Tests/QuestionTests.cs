using ListovkiQuiz.Models;

namespace ListovkiQuiz.Tests;

/// <summary>
/// Tests how questions are graded when users select multiple answers.
/// </summary>

public class QuestionTests
{
/// Creates a question with two correct and two incorrect answers
/// for testing different answer combinations.
    private static Question QuestionWithTwoCorrectAnswers() => new Question
    {
        Id = 1,
        Points = 3,
        Text = "Примерен въпрос",
        Answers = new List<Answer>
        {
            new Answer { Text = "Верен 1", IsCorrect = true },
            new Answer { Text = "Верен 2", IsCorrect = true },
            new Answer { Text = "Грешен 1", IsCorrect = false },
            new Answer { Text = "Грешен 2", IsCorrect = false },
        }
    };

    
    [Theory]
    [InlineData(new[] { 0, 1 }, true)]       
    [InlineData(new[] { 1, 0 }, true)]       
    [InlineData(new[] { 0 }, false)]          
    [InlineData(new[] { 0, 1, 3 }, false)]   
    [InlineData(new int[0], false)]          
    public void IsAnsweredCorrectly_RequiresExactlyTheCorrectAnswers(int[] picked, bool expected)
    {
        var question = QuestionWithTwoCorrectAnswers();

        bool result = question.IsAnsweredCorrectly(picked.ToList());

        Assert.Equal(expected, result);
    }
}
