using System.Text.Json;
using ListovkiQuiz.Models;
using ListovkiQuiz.Services;

namespace ListovkiQuiz.Tests;

/// <summary>
/// Tests exam generation using a separate set of questions created specifically for testing.
/// </summary>
public class ExamBuilderTests : IDisposable
{
    private readonly string _folder = Path.Combine(Path.GetTempPath(), "listovki-tests-" + Guid.NewGuid());

    public ExamBuilderTests()
    {
        // Create a temporary question database so the tests do not depend on the actual application data.
        Directory.CreateDirectory(Path.Combine(_folder, "Data"));
        string json = JsonSerializer.Serialize(MakeTestQuestions());
        File.WriteAllText(Path.Combine(_folder, "Data", "questions.json"), json);
    }

    public void Dispose()
    {    // Delete the temporary database after the tests have finished.
        if (Directory.Exists(_folder))
            Directory.Delete(_folder, recursive: true);
    }

    
    private static List<object> MakeTestQuestions()
    {/// Creates enough questions for every exam section, including some that should be excluded because they contain video.
        var questions = new List<object>();
        int id = 1;

        foreach (ExamPart part in ExamRules.Parts)
        { // Add video questions to check that the repository filters them out.
            for (int i = 0; i < part.Count + 5; i++)
                questions.Add(MakeQuestion(id++, part.Section, part.Points, hasVideo: false));
        }

        
        for (int i = 0; i < 10; i++)
            questions.Add(MakeQuestion(id++, 4, 3, hasVideo: true));

        return questions;
    }

    private static object MakeQuestion(int id, int section, int points, bool hasVideo) => new
    {
        id,
        section = $"Раздел {section}. Тест",
        points,
        question = $"Въпрос {id}",
        answers = new[]
        {
            new { text = "Верен", isCorrect = true },
            new { text = "Грешен", isCorrect = false },
        },
        imageRefs = new string[0],
        videoRefs = hasVideo ? new[] { "./assets/video.mp4" } : new string[0],
    };

    [Fact]
    public void BuildExam_Gives45DifferentQuestions_WorthExactly97Points()
    {
        var repository = new QuestionRepository(new TestEnvironment(_folder));

       // Repeat generation to check that the exam rules are respected across multiple runs.
        for (int run = 0; run < 20; run++)
        {
            var exam = repository.BuildExam();

            Assert.Equal(ExamRules.QuestionCount, exam.Count);                             
            Assert.Equal(ExamRules.QuestionCount, exam.Select(q => q.Id).Distinct().Count()); 
            Assert.Equal(ExamRules.TotalPoints, exam.Sum(q => q.Points));                 
        }
    }

    [Fact]
    public void BuildExam_NeverContainsQuestionsWithVideo()
    {
        var repository = new QuestionRepository(new TestEnvironment(_folder));

       
        for (int run = 0; run < 20; run++)
        {
            var exam = repository.BuildExam();

            Assert.DoesNotContain(exam, q => q.VideoRefs.Count > 0);
        }
    }
}
