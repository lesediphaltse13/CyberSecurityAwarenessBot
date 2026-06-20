namespace CyberSecurityAwarenessBot.Models;

public class QuizQuestion
{
    public string Question { get; set; }

    public List<string> Answers { get; set; }

    public int CorrectAnswer { get; set; }

    public string Explanation { get; set; }
}