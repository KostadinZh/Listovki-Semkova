using ListovkiQuiz.Models;
using ListovkiQuiz.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ListovkiQuiz.Pages;

public class QuizModel : PageModel
{
    private readonly QuestionRepository _repo;
<<<<<<< HEAD

    public QuizModel(QuestionRepository repo)
    {
        _repo = repo;
    }

    // The ids of the questions in this exam. The page sends them back (as hidden fields)
    // when the form is submitted, so we know which questions to grade.
    [BindProperty]
    public List<int> Ids { get; set; } = new List<int>();

    public List<Question> Questions { get; set; } = new List<Question>();

    // Everything below is filled in only after the exam is submitted
    public bool Submitted { get; set; }
    public Dictionary<int, List<int>> Picked { get; } = new Dictionary<int, List<int>>();  // question id -> ticked answers
    public Dictionary<int, bool> IsRight { get; } = new Dictionary<int, bool>();           // question id -> correct?
    public List<int> WrongNumbers { get; } = new List<int>();                              // positions of wrong questions
    public int CorrectCount { get; set; }
    public int PointsEarned { get; set; }

    public bool Passed => PointsEarned >= ExamRules.PassPoints;
    public int ScorePercent => PointsEarned * 100 / ExamRules.TotalPoints;
    public int PassPercent => ExamRules.PassPoints * 100 / ExamRules.TotalPoints;

    // Opening the page: build a new exam
=======
    public QuizModel(QuestionRepository repo) => _repo = repo;

    // Posted back from the form: which questions were shown, and which options were ticked ("questionId:answerIndex")
    [BindProperty] public List<int> QuestionIds { get; set; } = new();
    [BindProperty] public List<string> Picked { get; set; } = new();

    public List<Question> Questions { get; private set; } = new();
    public Dictionary<int, HashSet<int>> PickedMap { get; } = new();
    public Dictionary<int, bool> Results { get; } = new();
    public bool Submitted { get; private set; }
    public int CorrectCount { get; private set; }
    public int PointsEarned { get; private set; }

    public int QuestionCount => ExamRules.QuestionCount;
    public int TotalPoints => ExamRules.TotalPoints;
    public int PassPoints => ExamRules.PassPoints;
    public bool Passed => PointsEarned >= ExamRules.PassPoints;

    /// <summary>Every visit starts a brand-new exam.</summary>
>>>>>>> 79ca3d6f10877abfd5f28b0e205050300759377f
    public void OnGet()
    {
        Questions = _repo.BuildExam();
    }

<<<<<<< HEAD
    // Pressing the "Finish" button: grade the exam
    public IActionResult OnPost()
    {
        Questions = _repo.GetByIds(Ids.Distinct().ToList());

        // If the form is incomplete or was changed, start a new exam instead
        if (Questions.Count != ExamRules.QuestionCount || Questions.Sum(q => q.Points) != ExamRules.TotalPoints)
=======
    public IActionResult OnPost()
    {
        // Only accept a complete, valid exam; anything else starts a fresh one
        var ids = QuestionIds.Distinct().ToList();
        if (ids.Count != ExamRules.QuestionCount) return RedirectToPage();

        Questions = ids.Select(id => _repo.GetById(id))
                       .Where(q => q != null)
                       .Select(q => q!)
                       .ToList();
        if (Questions.Count != ExamRules.QuestionCount
            || Questions.Sum(q => q.Points) != ExamRules.TotalPoints)
>>>>>>> 79ca3d6f10877abfd5f28b0e205050300759377f
            return RedirectToPage();

        Submitted = true;

<<<<<<< HEAD
        for (int i = 0; i < Questions.Count; i++)
        {
            Question question = Questions[i];

            // The ticked answers of a question are sent under the name q<question id>
            var picked = new List<int>();
            foreach (var value in Request.Form["q" + question.Id])
            {
                if (int.TryParse(value, out int answerNumber))
                    picked.Add(answerNumber);
            }

            bool right = question.IsAnsweredCorrectly(picked);
            Picked[question.Id] = picked;
            IsRight[question.Id] = right;

            if (right)
            {
                CorrectCount++;
                PointsEarned += question.Points;
            }
            else
            {
                WrongNumbers.Add(i + 1);
            }
        }

=======
        foreach (var p in Picked)
        {
            var parts = p.Split(':');
            if (parts.Length == 2 && int.TryParse(parts[0], out var qid) && int.TryParse(parts[1], out var idx))
            {
                if (!PickedMap.ContainsKey(qid)) PickedMap[qid] = new HashSet<int>();
                PickedMap[qid].Add(idx);
            }
        }

        foreach (var q in Questions)
        {
            var chosen = PickedMap.TryGetValue(q.Id, out var set) ? set : new HashSet<int>();
            var correct = q.Answers
                .Select((a, i) => (a, i))
                .Where(x => x.a.IsCorrect)
                .Select(x => x.i)
                .ToHashSet();

            // All-or-nothing: you must tick exactly the correct options
            var ok = chosen.SetEquals(correct);
            Results[q.Id] = ok;
            if (ok)
            {
                CorrectCount++;
                PointsEarned += q.Points;
            }
        }
>>>>>>> 79ca3d6f10877abfd5f28b0e205050300759377f
        return Page();
    }
}
