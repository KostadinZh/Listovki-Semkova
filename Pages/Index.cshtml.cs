<<<<<<< HEAD
=======
using ListovkiQuiz.Models;
>>>>>>> 79ca3d6f10877abfd5f28b0e205050300759377f
using ListovkiQuiz.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ListovkiQuiz.Pages;

public class IndexModel : PageModel
{
    private readonly QuestionRepository _repo;
<<<<<<< HEAD

    public IndexModel(QuestionRepository repo)
    {
        _repo = repo;
    }

    public int PoolSize => _repo.PoolSize;

    public void OnGet()
    {
    }
=======
    public IndexModel(QuestionRepository repo) => _repo = repo;

    public int PoolSize => _repo.PoolSize;
    public int QuestionCount => ExamRules.QuestionCount;
    public int TotalPoints => ExamRules.TotalPoints;
    public int PassPoints => ExamRules.PassPoints;

    public void OnGet() { }
>>>>>>> 79ca3d6f10877abfd5f28b0e205050300759377f
}
