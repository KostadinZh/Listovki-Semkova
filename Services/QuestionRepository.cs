using System.Text.Json;
using ListovkiQuiz.Models;

namespace ListovkiQuiz.Services;

<<<<<<< HEAD
/// <summary>Reads Data/questions.json once, and builds exams from it.</summary>
public class QuestionRepository
{
    // All questions that can be used (questions with a video are left out)
    private readonly List<Question> _questions;

    public QuestionRepository(IWebHostEnvironment env)
    {
        string path = Path.Combine(env.ContentRootPath, "Data", "questions.json");
        string json = File.ReadAllText(path);

        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        var all = JsonSerializer.Deserialize<List<Question>>(json, options) ?? new List<Question>();

        _questions = all.Where(q => q.VideoRefs.Count == 0).ToList();

        CheckRecipe();
    }

    /// <summary>How many questions an exam can be drawn from.</summary>
    public int PoolSize => _questions.Count;

    /// <summary>
    /// Builds one new exam by following the recipe in ExamRules.Parts:
    /// for every line, pick random questions of that section and point value.
    /// The result is always 45 questions worth 97 points.
=======
public class QuestionRepository
{
    private readonly List<Question> _questions;   // everything in the JSON
    private readonly List<Question> _pool;        // questions the exam may use (no videos)
    private readonly Dictionary<int, Question> _byId;

    public QuestionRepository(IWebHostEnvironment env, ILogger<QuestionRepository> logger)
    {
        var path = Path.Combine(env.ContentRootPath, "Data", "questions.json");
        using var stream = File.OpenRead(path);
        _questions = JsonSerializer.Deserialize<List<Question>>(
            stream, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new();
        _byId = _questions.ToDictionary(q => q.Id);

        // Video questions are not used
        _pool = _questions.Where(q => q.VideoRefs.Count == 0).ToList();

        ValidateBlueprint();
        logger.LogInformation("Loaded {Total} questions ({Pool} usable, videos excluded) from {Path}",
            _questions.Count, _pool.Count, path);
    }

    public int TotalCount => _questions.Count;
    public int PoolSize => _pool.Count;

    public Question? GetById(int id) => _byId.GetValueOrDefault(id);

    /// <summary>
    /// Builds one exam: for every blueprint cell, random questions of that section and point value.
    /// Always 45 questions worth exactly 97 points, ordered by section.
>>>>>>> 79ca3d6f10877abfd5f28b0e205050300759377f
    /// </summary>
    public List<Question> BuildExam()
    {
        var exam = new List<Question>();
<<<<<<< HEAD

        foreach (var part in ExamRules.Parts)
        {
            // every question of this section with this number of points
            Question[] candidates = _questions
                .Where(q => q.SectionNo == part.Section && q.Points == part.Points)
                .ToArray();

            Random.Shared.Shuffle(candidates);               // mix them like a deck of cards
            exam.AddRange(candidates.Take(part.Count));      // take the first ones
        }

        // Mix the whole exam so the sections are not in a fixed order
        Question[] mixed = exam.ToArray();
        Random.Shared.Shuffle(mixed);
        return mixed.ToList();
    }

    /// <summary>Finds the questions with the given ids, in the same order. Unknown ids are skipped.</summary>
    public List<Question> GetByIds(List<int> ids)
    {
        var result = new List<Question>();
        foreach (int id in ids)
        {
            Question? question = _questions.FirstOrDefault(q => q.Id == id);
            if (question != null)
                result.Add(question);
        }
        return result;
    }

    /// <summary>Stops the program at startup if the recipe cannot make a correct exam.</summary>
    private void CheckRecipe()
    {
        int questions = 0;
        int points = 0;
        foreach (var part in ExamRules.Parts)
        {
            questions += part.Count;
            points += part.Count * part.Points;
        }

        if (questions != ExamRules.QuestionCount || points != ExamRules.TotalPoints)
            throw new InvalidOperationException(
                $"The exam recipe gives {questions} questions and {points} points, " +
                $"but it must give {ExamRules.QuestionCount} and {ExamRules.TotalPoints}.");

        foreach (var part in ExamRules.Parts)
        {
            int available = _questions.Count(q => q.SectionNo == part.Section && q.Points == part.Points);
            if (available < part.Count)
                throw new InvalidOperationException(
                    $"Section {part.Section} has only {available} questions worth {part.Points} point(s), " +
                    $"but the exam needs {part.Count}.");
        }
    }
=======
        foreach (var (section, points, count) in ExamRules.Blueprint)
        {
            exam.AddRange(_pool
                .Where(q => q.SectionNo == section && q.Points == points)
                .OrderBy(_ => System.Random.Shared.Next())
                .Take(count));
        }
        return exam.OrderBy(q => q.SectionNo)
                   .ThenBy(_ => System.Random.Shared.Next())
                   .ToList();
    }

    /// <summary>Fails fast at startup if the blueprint or the data can't produce a valid exam.</summary>
    private void ValidateBlueprint()
    {
        var count = ExamRules.Blueprint.Sum(b => b.Count);
        var points = ExamRules.Blueprint.Sum(b => b.Points * b.Count);
        if (count != ExamRules.QuestionCount || points != ExamRules.TotalPoints)
            throw new InvalidOperationException(
                $"Exam blueprint gives {count} questions / {points} points, expected " +
                $"{ExamRules.QuestionCount} / {ExamRules.TotalPoints}.");

        foreach (var (section, pts, need) in ExamRules.Blueprint)
        {
            var have = _pool.Count(q => q.SectionNo == section && q.Points == pts);
            if (have < need)
                throw new InvalidOperationException(
                    $"Section {section} has only {have} questions worth {pts} point(s); the exam needs {need}.");
        }
    }

    /// <summary>For the development-only API.</summary>
    public IEnumerable<Question> Search(int? section) =>
        section is > 0 ? _questions.Where(q => q.SectionNo == section) : _questions;
>>>>>>> 79ca3d6f10877abfd5f28b0e205050300759377f
}
