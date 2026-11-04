namespace ListovkiQuiz.Models;

<<<<<<< HEAD
/// <summary>One line of the exam recipe: "take 5 questions from section 2, each worth 2 points".</summary>
public class ExamPart
{
    public int Section { get; }
    public int Points { get; }
    public int Count { get; }

    public ExamPart(int section, int points, int count)
    {
        Section = section;
        Points = points;
        Count = count;
    }
}

/// <summary>The fixed rules of the exam. If a rule changes, change it here.</summary>
=======
/// <summary>The fixed rules of the exam. Change them here, in one place.</summary>
>>>>>>> 79ca3d6f10877abfd5f28b0e205050300759377f
public static class ExamRules
{
    public const int QuestionCount = 45;
    public const int TotalPoints = 97;
    public const int PassPoints = 87;

<<<<<<< HEAD
    // The recipe for one exam. The lines must add up to 45 questions and 97 points
    // (the program checks this when it starts).
    public static readonly List<ExamPart> Parts = new List<ExamPart>
    {
        new ExamPart(1, 1, 3),    //  3 questions x 1 point  =  3 points
        new ExamPart(2, 2, 5),    //  5 questions x 2 points = 10 points
        new ExamPart(3, 2, 2),    //  2 questions x 2 points =  4 points
        new ExamPart(3, 3, 2),    //  2 questions x 3 points =  6 points
        new ExamPart(4, 1, 4),    //  4 questions x 1 point  =  4 points
        new ExamPart(4, 2, 1),    //  1 question  x 2 points =  2 points
        new ExamPart(4, 3, 20),   // 20 questions x 3 points = 60 points
        new ExamPart(5, 1, 4),    //  4 questions x 1 point  =  4 points
        new ExamPart(6, 1, 4),    //  4 questions x 1 point  =  4 points
    };                            // total: 45 questions, 97 points
=======
    /// <summary>
    /// How many questions of each (section, point value) every exam contains.
    /// The counts must add up to exactly 45 questions worth exactly 97 points
    /// (checked when the app starts).
    /// </summary>
    public static readonly (int Section, int Points, int Count)[] Blueprint =
    {
        (1, 1, 3),   //  3 q ×1 =  3
        (2, 2, 5),   //  5 q ×2 = 10
        (3, 2, 2),   //  2 q ×2 =  4
        (3, 3, 2),   //  2 q ×3 =  6
        (4, 1, 4),   //  4 q ×1 =  4
        (4, 2, 1),   //  1 q ×2 =  2
        (4, 3, 20),  // 20 q ×3 = 60
        (5, 1, 4),   //  4 q ×1 =  4
        (6, 1, 4),   //  4 q ×1 =  4
    };                // total: 45 questions, 97 points
>>>>>>> 79ca3d6f10877abfd5f28b0e205050300759377f
}
