using ListovkiQuiz.Models;
using ListovkiQuiz.Services;

namespace ListovkiQuiz.Tests;

/// <summary>
/// Tests user registration, login and attempt storage.
/// Each test uses a separate temporary folder to avoid sharing data with other tests.
/// </summary>
public class UserStoreTests : IDisposable
{
    private readonly string _folder = Path.Combine(Path.GetTempPath(), "listovki-tests-" + Guid.NewGuid());


    private UserStore NewStore() => new UserStore(new TestEnvironment(_folder));

    public void Dispose()
    {
        // Remove temporary test data after each test.

        if (Directory.Exists(_folder))
            Directory.Delete(_folder, recursive: true);
    }

    [Fact]
    public void Register_ThenLogin_WorksOnlyWithTheRightPassword()
    {
        var store = NewStore();

       
        var created = store.CreateUser("ivan", "parola123");
        var rightPassword = store.CheckLogin("ivan", "parola123");
        var wrongPassword = store.CheckLogin("ivan", "greshna");
        var unknownUser = store.CheckLogin("petar", "parola123");

        
        Assert.NotNull(created);
        Assert.NotNull(rightPassword);
        Assert.Equal(created!.Id, rightPassword!.Id);
        Assert.Null(wrongPassword);
        Assert.Null(unknownUser);

        // The original password must never be stored as plain text.
        Assert.DoesNotContain("parola123", created.PasswordHash);
    }

    [Fact]
    public void Register_RejectsTheSameUsername_EvenWithDifferentLetterCase()
    {
        var store = NewStore();
        store.CreateUser("Ivan", "parola123");

        // Usernames are treated as case-insensitive to prevent duplicate accounts.
        var duplicate = store.CreateUser("IVAN", "drugaparola");

        Assert.Null(duplicate);
    }

    [Fact]
    public void UsersAndAttempts_AreRemembered_AfterTheAppRestarts()
    {
     
        var firstRun = NewStore();
        var user = firstRun.CreateUser("ivan", "parola123")!;
        firstRun.AddAttempt(new Attempt { UserId = user.Id, TakenAt = DateTime.UtcNow, Points = 90, ExamCode = "exam-1" });

        // A new instance reads the data saved by the previous instance.
        var secondRun = NewStore();

      
        Assert.NotNull(secondRun.CheckLogin("ivan", "parola123"));
        var attempts = secondRun.GetAttempts(user.Id);
        Assert.Single(attempts);
        Assert.Equal(90, attempts[0].Points);
    }

    [Fact]
    public void GetAttempts_ReturnsOnlyTheUsersOwnAttempts_NewestFirst()
    {
        var store = NewStore();
        var ivan = store.CreateUser("ivan", "parola123")!;
        var petar = store.CreateUser("petar", "parola123")!;

        
        store.AddAttempt(new Attempt { UserId = ivan.Id, TakenAt = new DateTime(2026, 6, 1, 10, 0, 0, DateTimeKind.Utc), Points = 80, ExamCode = "a" });
        store.AddAttempt(new Attempt { UserId = ivan.Id, TakenAt = new DateTime(2026, 1, 1, 10, 0, 0, DateTimeKind.Utc), Points = 50, ExamCode = "b" });
        store.AddAttempt(new Attempt { UserId = petar.Id, TakenAt = new DateTime(2026, 3, 1, 10, 0, 0, DateTimeKind.Utc), Points = 97, ExamCode = "c" });

        var ivanAttempts = store.GetAttempts(ivan.Id);

         // Results belonging to another user must not appear in Ivan's history.
        Assert.Equal(2, ivanAttempts.Count);
        Assert.Equal(80, ivanAttempts[0].Points);
        Assert.Equal(50, ivanAttempts[1].Points);
    }

    [Fact]
    public void AddAttempt_SavesTheSameExamOnlyOnce()
    {
        var store = NewStore();
        var user = store.CreateUser("ivan", "parola123")!;

        
        
        store.AddAttempt(new Attempt { UserId = user.Id, TakenAt = DateTime.UtcNow, Points = 90, ExamCode = "same-exam" });
        store.AddAttempt(new Attempt { UserId = user.Id, TakenAt = DateTime.UtcNow, Points = 90, ExamCode = "same-exam" });

        // Repeated submissions with the same exam code should not create duplicate history entries.
        Assert.Single(store.GetAttempts(user.Id));
    }

    [Fact]
    public void ShortTestAttempts_KeepTheirDetails_AfterTheAppRestarts()
    {
        var firstRun = NewStore();
        var user = firstRun.CreateUser("ivan", "parola123")!;

        // A short test is saved with its own size and possible points,
        // so My attempts can list it separately from the full exams.
        firstRun.AddAttempt(new Attempt
        {
            UserId = user.Id,
            TakenAt = DateTime.UtcNow,
            Points = 7,
            CorrectCount = 4,
            IsShortTest = true,
            QuestionCount = 5,
            TotalPoints = 11,
            ExamCode = "short-1"
        });

        // A new store reads the same files, like a restarted application.
        var attempt = Assert.Single(NewStore().GetAttempts(user.Id));

        Assert.True(attempt.IsShortTest);
        Assert.Equal(5, attempt.QuestionCount);
        Assert.Equal(11, attempt.TotalPoints);
    }
}
