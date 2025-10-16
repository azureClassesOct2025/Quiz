using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizApp.Models;
using QuizApp.Services;
using System.Text;
using System.Text.Json;

namespace QuizApp.Pages
{
    public class QuizModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public int Step { get; set; }

        [BindProperty]
        public string? Selected { get; set; }

        public string CurrentQuestion { get; set; } = string.Empty;
        public string[] Options { get; set; } = Array.Empty<string>();
        public int TotalQuestions => QuizRepository.Questions.Count;
        public bool IsLast => Step >= TotalQuestions - 1;
        public string Error { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            var data = Load();
            if (data == null)
            {
                return RedirectToPage("Index");
            }
            if (Step < 0 || Step >= TotalQuestions)
            {
                return RedirectToPage("Quiz", new { step = 0 });
            }

            PopulateForStep(data);
            return Page();
        }

        public IActionResult OnPost(string action)
        {
            var data = Load();
            if (data == null)
            {
                return RedirectToPage("Index");
            }

            if (Step < 0 || Step >= TotalQuestions)
            {
                Step = 0;
            }

            // Require selection for Next/Submit
            if (action == "next" || action == "submit")
            {
                if (string.IsNullOrWhiteSpace(Selected))
                {
                    PopulateForStep(data);
                    Error = "Please select an option before continuing.";
                    return Page();
                }
            }

            // Save selection for this step
            EnsureSize(data.SelectedOptions, TotalQuestions);
            data.SelectedOptions[Step] = Selected;
            Save(data);

            if (action == "next")
            {
                var nextStep = Math.Min(Step + 1, TotalQuestions - 1);
                return RedirectToPage("Quiz", new { step = nextStep });
            }
            else if (action == "submit")
            {
                var fileContent = BuildResults(data);
                var fileName = SanitizeFileName(data.Username) + "_quiz_results.txt";
                var outputDir = Path.Combine(Directory.GetCurrentDirectory(), "Results");
                Directory.CreateDirectory(outputDir);
                var outputPath = Path.Combine(outputDir, fileName);
                System.IO.File.WriteAllText(outputPath, fileContent, Encoding.UTF8);
                TempData["ResultPath"] = outputPath;
                return RedirectToPage("Thanks");
            }

            // default: redisplay same step
            PopulateForStep(data);
            return Page();
        }

        private void PopulateForStep(QuizData data)
        {
            var (question, options) = QuizRepository.Questions[Step];
            CurrentQuestion = question;
            Options = options;
            if (Step < data.SelectedOptions.Count)
            {
                Selected = data.SelectedOptions[Step];
            }
        }

        private QuizData? Load()
        {
            var json = HttpContext.Session.GetString("quizData");
            if (string.IsNullOrWhiteSpace(json)) return null;
            return JsonSerializer.Deserialize<QuizData>(json);
        }

        private void Save(QuizData data)
        {
            HttpContext.Session.SetString("quizData", JsonSerializer.Serialize(data));
        }

        private static void EnsureSize(List<string?> list, int size)
        {
            while (list.Count < size) list.Add(null);
        }

        private static string BuildResults(QuizData data)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"User: {data.Username}");
            sb.AppendLine();
            for (int i = 0; i < QuizRepository.Questions.Count; i++)
            {
                var (q, opts) = QuizRepository.Questions[i];
                var selected = data.SelectedOptions.ElementAtOrDefault(i) ?? "";
                var selectedText = OptionTextFromLetter(opts, selected);
                sb.AppendLine($"Q{i + 1}: {q}");
                sb.AppendLine($"Selected: {selected} {selectedText}");
                sb.AppendLine();
            }
            return sb.ToString();
        }

        private static string OptionTextFromLetter(string[] options, string letter)
        {
            if (string.IsNullOrWhiteSpace(letter)) return string.Empty;
            int idx = char.ToUpperInvariant(letter[0]) - 'A';
            if (idx >= 0 && idx < options.Length) return options[idx];
            return string.Empty;
        }

        private static string SanitizeFileName(string name)
        {
            foreach (var c in Path.GetInvalidFileNameChars())
            {
                name = name.Replace(c, '_');
            }
            return string.IsNullOrWhiteSpace(name) ? "user" : name.Trim();
        }
    }
}


