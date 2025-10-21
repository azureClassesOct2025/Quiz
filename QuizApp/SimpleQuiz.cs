using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace QuizApp
{
    public class SimpleQuiz
    {
        private static readonly List<(string Question, string[] Options, string CorrectAnswer)> Questions = new()
        {
            ("Which planet is known as the \"Red Planet\"?", new[]{"Venus","Mars","Jupiter","Mercury"}, "B"),
            ("Who wrote the play Romeo and Juliet?", new[]{"William Wordsworth","Charles Dickens","William Shakespeare","George Orwell"}, "C"),
            ("What is the capital city of Australia?", new[]{"Sydney","Melbourne","Canberra","Perth"}, "C"),
            ("Which gas do plants absorb from the atmosphere during photosynthesis?", new[]{"Oxygen","Nitrogen","Carbon Dioxide","Hydrogen"}, "C"),
            ("What is the largest ocean on Earth?", new[]{"Atlantic Ocean","Indian Ocean","Arctic Ocean","Pacific Ocean"}, "D"),
        };

        public static void Main(string[] args)
        {
            Console.WriteLine("=== Welcome to the Quiz ===");
            Console.WriteLine();
            
            // Get user name
            Console.Write("Please enter your name: ");
            string? userName = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(userName))
            {
                Console.WriteLine("Name is required. Exiting...");
                return;
            }

            Console.WriteLine($"\nHello {userName}! Let's start the quiz.\n");
            Console.WriteLine("Press any key to begin...");
            Console.ReadKey();

            var answers = new List<string?>();
            int score = 0;

            // Ask each question
            for (int i = 0; i < Questions.Count; i++)
            {
                var (question, options, correctAnswer) = Questions[i];
                
                Console.Clear();
                Console.WriteLine($"=== Question {i + 1} of {Questions.Count} ===");
                Console.WriteLine($"\n{question}\n");

                // Display options
                for (int j = 0; j < options.Length; j++)
                {
                    char letter = (char)('A' + j);
                    Console.WriteLine($"{letter}) {options[j]}");
                }

                Console.WriteLine();
                Console.Write("Enter your answer (A, B, C, or D): ");
                string? answer = Console.ReadLine()?.ToUpper();

                while (answer != "A" && answer != "B" && answer != "C" && answer != "D")
                {
                    Console.Write("Invalid input. Please enter A, B, C, or D: ");
                    answer = Console.ReadLine()?.ToUpper();
                }

                answers.Add(answer);

                // Check if correct
                if (answer == correctAnswer)
                {
                    Console.WriteLine("✓ Correct!");
                    score++;
                }
                else
                {
                    Console.WriteLine($"✗ Incorrect. The correct answer is {correctAnswer}) {options[correctAnswer[0] - 'A']}");
                }

                Console.WriteLine("\nPress any key to continue...");
                Console.ReadKey();
            }

            // Show results
            Console.Clear();
            Console.WriteLine("=== Quiz Complete! ===");
            Console.WriteLine($"\n{userName}, your score: {score}/{Questions.Count}");
            Console.WriteLine($"Percentage: {(score * 100.0 / Questions.Count):F1}%");

            // Save results to file
            SaveResults(userName, answers, score);

            Console.WriteLine("\nResults saved to file!");
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }

        private static void SaveResults(string userName, List<string?> answers, int score)
        {
            var resultsDir = Path.Combine(Directory.GetCurrentDirectory(), "Results");
            Directory.CreateDirectory(resultsDir);

            var fileName = SanitizeFileName(userName) + "_quiz_results.txt";
            var filePath = Path.Combine(resultsDir, fileName);

            var content = new StringBuilder();
            content.AppendLine($"User: {userName}");
            content.AppendLine($"Score: {score}/{Questions.Count}");
            content.AppendLine($"Percentage: {(score * 100.0 / Questions.Count):F1}%");
            content.AppendLine();

            for (int i = 0; i < Questions.Count; i++)
            {
                var (question, options, correctAnswer) = Questions[i];
                var userAnswer = answers[i] ?? "No answer";
                var isCorrect = userAnswer == correctAnswer ? "✓" : "✗";
                
                content.AppendLine($"Q{i + 1}: {question}");
                content.AppendLine($"Your Answer: {userAnswer}) {(userAnswer != "No answer" ? options[userAnswer[0] - 'A'] : "No answer")} {isCorrect}");
                content.AppendLine($"Correct Answer: {correctAnswer}) {options[correctAnswer[0] - 'A']}");
                content.AppendLine();
            }

            File.WriteAllText(filePath, content.ToString(), Encoding.UTF8);
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

