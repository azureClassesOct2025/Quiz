using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using QuizApp.Models;
using QuizApp.Services;
using System.Text.Json;

namespace QuizApp.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string Username { get; set; } = string.Empty;

        public string Error { get; set; } = string.Empty;

        public void OnGet()
        {
            // no-op
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrWhiteSpace(Username))
            {
                Error = "Name is required.";
                return Page();
            }

            var quizData = new QuizData { Username = Username };
            for (int i = 0; i < QuizRepository.Questions.Count; i++)
            {
                quizData.SelectedOptions.Add(null);
            }

            HttpContext.Session.SetString("quizData", JsonSerializer.Serialize(quizData));
            return RedirectToPage("Quiz", new { step = 0 });
        }
    }
}



