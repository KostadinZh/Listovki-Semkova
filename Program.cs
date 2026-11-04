using ListovkiQuiz.Services;

var builder = WebApplication.CreateBuilder(args);

<<<<<<< HEAD
// Razor Pages = the website. QuestionRepository = loads the JSON once and keeps it in memory.
=======
>>>>>>> 79ca3d6f10877abfd5f28b0e205050300759377f
builder.Services.AddRazorPages();
builder.Services.AddSingleton<QuestionRepository>();

var app = builder.Build();

<<<<<<< HEAD
app.UseStaticFiles();   // serves wwwroot (css and the question pictures)
app.MapRazorPages();    // serves the pages in the Pages folder
=======
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();

// Raw JSON endpoints for checking the data. They expose the correct answers,
// so they only exist when running in Development.
if (app.Environment.IsDevelopment())
{
    app.MapGet("/api/questions", (QuestionRepository repo, int? section, int? take) =>
        Results.Ok(repo.Search(section).Take(take ?? 50)));

    app.MapGet("/api/questions/{id:int}", (QuestionRepository repo, int id) =>
        repo.GetById(id) is { } q ? Results.Ok(q) : Results.NotFound());
}
>>>>>>> 79ca3d6f10877abfd5f28b0e205050300759377f

app.Run();
