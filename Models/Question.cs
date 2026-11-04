using System.Text.Json.Serialization;
<<<<<<< HEAD
=======
using System.Text.RegularExpressions;
>>>>>>> 79ca3d6f10877abfd5f28b0e205050300759377f

namespace ListovkiQuiz.Models;

public class Answer
{
    public string Text { get; set; } = "";
    public bool IsCorrect { get; set; }
}

<<<<<<< HEAD
/// <summary>One question, with the same fields as in Data/questions.json.</summary>
public class Question
{
    public int Id { get; set; }
    public string Section { get; set; } = "";   // for example "Раздел 4. ПРАВИЛА ЗА ДВИЖЕНИЕТО ПО ПЪТИЩАТА"
    public int Points { get; set; }

    [JsonPropertyName("question")]              // in the JSON file this field is called "question"
    public string Text { get; set; } = "";

    public List<Answer> Answers { get; set; } = new List<Answer>();
    public List<string> ImageRefs { get; set; } = new List<string>();   // for example "./assets/30115.png"
    public List<string> VideoRefs { get; set; } = new List<string>();

    // ----- Small helpers used by the pages -----

    /// <summary>The section number. The text always starts with "Раздел " (7 characters), then the digit.</summary>
=======
/// <summary>
/// Mirrors one entry of Data/questions.json:
/// id, section, points, question, answers[{text,isCorrect}], imageRefs[], videoRefs[]
/// </summary>
public class Question
{
    public int Id { get; set; }
    public string Section { get; set; } = "";
    public int Points { get; set; }

    // The JSON property is called "question"; a member can't share its class's name in C#.
    [JsonPropertyName("question")]
    public string Text { get; set; } = "";

    public List<Answer> Answers { get; set; } = new();
    public List<string> ImageRefs { get; set; } = new();
    public List<string> VideoRefs { get; set; } = new();

    // ---- Derived helpers (not part of the JSON) ----

    /// <summary>Section number parsed from "Раздел 3. ...".</summary>
    [JsonIgnore]
>>>>>>> 79ca3d6f10877abfd5f28b0e205050300759377f
    public int SectionNo
    {
        get
        {
<<<<<<< HEAD
            if (Section.Length < 8)
                return 0;
            string digit = Section.Substring(7, 1);
            return int.TryParse(digit, out int number) ? number : 0;
        }
    }

    /// <summary>The short section name shown on the page: "Раздел 4".</summary>
    public string SectionLabel => Section.Split('.')[0];

    /// <summary>
    /// True when the answers are pictures: no answer has text, and there is
    /// exactly one picture for every answer (first picture = first answer, and so on).
    /// </summary>
    public bool HasImageAnswers
    {
        get
        {
            if (Answers.Count < 2 || ImageRefs.Count != Answers.Count)
                return false;
            foreach (var answer in Answers)
            {
                if (!string.IsNullOrWhiteSpace(answer.Text))
                    return false;
            }
            return true;
        }
    }

    /// <summary>The pictures shown under the question text. Empty when the answers are the pictures.</summary>
    public List<string> QuestionImages
    {
        get
        {
            if (HasImageAnswers)
                return new List<string>();
            return ImageRefs;
        }
    }

    /// <summary>The picture of answer number i, or null when that answer is only text.</summary>
    public string? AnswerImageUrl(int i)
    {
        if (!HasImageAnswers)
            return null;
        return AssetUrl(ImageRefs[i]);
    }

    /// <summary>Turns "./assets/30115.png" into "/assets/30115.png" (the file is in wwwroot/assets).</summary>
    public static string AssetUrl(string reference)
    {
        if (reference.StartsWith("./"))
            return "/" + reference.Substring(2);
        return reference;
    }

    /// <summary>
    /// A question is correct only when every correct answer is ticked and no wrong answer is ticked.
    /// "picked" holds the numbers of the answers the person ticked.
    /// </summary>
    public bool IsAnsweredCorrectly(List<int> picked)
    {
        for (int i = 0; i < Answers.Count; i++)
        {
            bool wasTicked = picked.Contains(i);
            if (Answers[i].IsCorrect != wasTicked)
                return false;
        }
        return true;
    }
=======
            var m = Regex.Match(Section, @"^\s*Раздел\s+(\d+)");
            return m.Success ? int.Parse(m.Groups[1].Value) : 0;
        }
    }

    /// <summary>
    /// True when the answer options are pictures: every answer has empty text and
    /// there is exactly one image per answer (imageRefs[i] belongs to answers[i]).
    /// </summary>
    [JsonIgnore]
    public bool HasImageAnswers =>
        Answers.Count > 1
        && ImageRefs.Count == Answers.Count
        && Answers.All(a => string.IsNullOrWhiteSpace(a.Text));

    /// <summary>Pictures shown with the question itself (empty for picture-answer questions).</summary>
    [JsonIgnore]
    public IReadOnlyList<string> QuestionImages =>
        HasImageAnswers ? Array.Empty<string>() : ImageRefs;

    /// <summary>URL of the picture for answer i, or null if this answer is text only.</summary>
    public string? AnswerImageUrl(int i) =>
        HasImageAnswers && i < ImageRefs.Count ? AssetUrl(ImageRefs[i]) : null;

    /// <summary>"./assets/123.png" -> "/assets/123.png" (served from wwwroot/assets).</summary>
    public static string AssetUrl(string reference) =>
        reference.StartsWith("./") ? "/" + reference[2..] : reference;
>>>>>>> 79ca3d6f10877abfd5f28b0e205050300759377f
}
