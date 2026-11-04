# Listovki Quiz (ASP.NET Core, Razor Pages)

<<<<<<< HEAD
A practice driving-theory exam built from `Data/questions.json`.

## Run
    dotnet run
Then open http://localhost:5000

## The rules
- 45 questions, 97 points in total, 87 points to pass (`Models/ExamRules.cs`)
- Questions come from all sections - the user cannot choose the number or the topics
- Questions with a video are never used
- A question gives its points only if exactly the correct answers are ticked

## What each file does
| File | Job |
|------|-----|
| `Program.cs` | Starts the website |
| `Data/questions.json` | The questions |
| `Models/Question.cs` | One question + small helpers (grading, picture paths) |
| `Models/ExamRules.cs` | 45 / 97 / 87 and the recipe: how many questions from each section |
| `Services/QuestionRepository.cs` | Reads the JSON, builds an exam, checks the recipe at startup |
| `Pages/Index.cshtml` | Home page |
| `Pages/Quiz.cshtml` | The exam page (questions, then the result) |
| `Pages/Quiz.cshtml.cs` | Builds the exam and grades the answers |
| `Pages/Shared/_Layout.cshtml` | Header and footer around every page |
| `wwwroot/css/site.css` | All colours and styling |

## Pictures
Put the question pictures in `wwwroot/assets/` (for example `wwwroot/assets/30115.png`).
`tools/download-assets.ps1` can download them.
=======
A fixed practice exam built from `Data/questions.json`.

## Run
```
dotnet run
```
Then open http://localhost:5000

## The exam
- 45 questions, worth exactly 97 points, pass mark 87 (see `Models/ExamRules.cs`)
- Drawn at random from all sections; the user cannot choose the number of questions or the topics
- Video questions (58) are never used
- A question only scores if exactly the correct options are ticked

The mix of questions per section and point value is the `Blueprint` table in `Models/ExamRules.cs`.
The app checks at startup that it adds up to 45 questions / 97 points.

## Pictures
The JSON only references pictures (`./assets/30115.png`). Put them in `wwwroot/assets/`.
`tools/download-assets.ps1` downloads the 863 that are needed.

## Checking the data (Development only)
With `ASPNETCORE_ENVIRONMENT=Development`: `/api/questions?take=5` and `/api/questions/5300` return raw JSON.
>>>>>>> 79ca3d6f10877abfd5f28b0e205050300759377f
