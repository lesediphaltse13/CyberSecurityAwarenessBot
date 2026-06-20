using System;

namespace CyberSecurityAwarenessBot.Services
{
    public class NlpService
    {
        public bool IsTaskCommand(string input)
        {
            input = input.ToLower();

            return input.Contains("add task")
                || input.Contains("create task")
                || input.Contains("set task")
                || input.Contains("remind me")
                || input.Contains("set reminder");
        }

        public bool IsActivityLogCommand(string input)
        {
            input = input.ToLower();

            return input.Contains("show activity log")
                || input.Contains("what have you done")
                || input.Contains("recent actions")
                || input.Contains("activity summary");
        }

        public bool IsQuizCommand(string input)
        {
            input = input.ToLower();

            return input.Contains("start quiz")
                || input.Contains("play quiz")
                || input.Contains("cybersecurity quiz");
        }

        public string ExtractTaskTitle(string input)
        {
            string task = input;

            task = task.Replace("add task to", "", System.StringComparison.OrdinalIgnoreCase);
            task = task.Replace("add a task to", "", System.StringComparison.OrdinalIgnoreCase);
            task = task.Replace("create task to", "", System.StringComparison.OrdinalIgnoreCase);
            task = task.Replace("remind me to", "", System.StringComparison.OrdinalIgnoreCase);
            task = task.Replace("set reminder to", "", System.StringComparison.OrdinalIgnoreCase);

            return task.Trim();
        }

        public DateTime ExtractReminderDate(string input)
        {
            input = input.ToLower();

            if (input.Contains("tomorrow"))
            {
                return DateTime.Now.AddDays(1);
            }

            if (input.Contains("next week"))
            {
                return DateTime.Now.AddDays(7);
            }

            if (input.Contains("in 3 days"))
            {
                return DateTime.Now.AddDays(3);
            }

            if (input.Contains("in 7 days"))
            {
                return DateTime.Now.AddDays(7);
            }

            return DateTime.Now;
        }
    }
}