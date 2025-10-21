using System.Collections.Generic;

namespace QuizApp.Services
{
    public static class QuizRepository
    {
        public static readonly List<(string Question, string[] Options)> Questions = new()
        {
            ("Which planet is known as the \u201cRed Planet\u201d?", new[]{"Venus","Mars","Jupiter","Mercury"}),
            ("Who wrote the play Romeo and Juliet?", new[]{"William Wordsworth","Charles Dickens","William Shakespeare","George Orwell"}),
            ("What is the capital city of Australia?", new[]{"Sydney","Melbourne","Canberra","Perth"}),
            ("Which gas do plants absorb from the atmosphere during photosynthesis?", new[]{"Oxygen","Nitrogen","Carbon Dioxide","Hydrogen"}),
            ("What is the largest ocean on Earth?", new[]{"Atlantic Ocean","Indian Ocean","Arctic Ocean","Pacific Ocean"}),
        };
    }
}




