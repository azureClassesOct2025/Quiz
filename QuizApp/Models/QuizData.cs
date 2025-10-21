using System.Collections.Generic;

namespace QuizApp.Models
{
    public class QuizData
    {
        public string Username { get; set; } = string.Empty;
        // Store selected option letter (e.g., "A", "B", etc.) per question index
        public List<string?> SelectedOptions { get; set; } = new List<string?>();
    }
}




