using Microsoft.AspNetCore.Mvc.RazorPages;

namespace QuizApp.Pages
{
    public class ThanksModel : PageModel
    {
        public string FilePath { get; set; } = string.Empty;

        public void OnGet()
        {
            FilePath = TempData["ResultPath"] as string ?? string.Empty;
        }
    }
}



