using System.Collections.Generic;
using CyberSecurityAwarenessBot.Models;

namespace CyberSecurityAwarenessBot.Services
{
    public class QuizService
    {
        public List<QuizQuestion> GetQuestions()
        {
            return new List<QuizQuestion>
            {
                new QuizQuestion
                {
                    Question = "What should you do if you receive an email asking for your password?",
                    Answers = new List<string>
                    {
                        "Reply with your password",
                        "Delete the email",
                        "Report as phishing",
                        "Ignore it"
                    },
                    CorrectAnswer = 2,
                    Explanation = "Reporting phishing emails helps prevent scams."
                },

                new QuizQuestion
                {
                    Question = "True or False: Using the same password everywhere is safe.",
                    Answers = new List<string>
                    {
                        "True",
                        "False"
                    },
                    CorrectAnswer = 1,
                    Explanation = "Every account should have a unique password."
                },

                new QuizQuestion
                {
                    Question = "What does HTTPS indicate?",
                    Answers = new List<string>
                    {
                        "Secure connection",
                        "Free internet",
                        "Antivirus protection",
                        "Password manager"
                    },
                    CorrectAnswer = 0,
                    Explanation = "HTTPS encrypts communication between browser and website."
                },

                new QuizQuestion
                {
                    Question = "Which is the strongest password?",
                    Answers = new List<string>
                    {
                        "123456",
                        "password",
                        "Lesedi123",
                        "L#9vT!2xQ@7"
                    },
                    CorrectAnswer = 3,
                    Explanation = "Strong passwords contain mixed characters and are difficult to guess."
                },

                new QuizQuestion
                {
                    Question = "What is phishing?",
                    Answers = new List<string>
                    {
                        "Fishing at sea",
                        "Attempt to steal information",
                        "Computer repair",
                        "Internet speed test"
                    },
                    CorrectAnswer = 1,
                    Explanation = "Phishing tricks users into revealing sensitive information."
                },

                new QuizQuestion
                {
                    Question = "True or False: Two-factor authentication improves security.",
                    Answers = new List<string>
                    {
                        "True",
                        "False"
                    },
                    CorrectAnswer = 0,
                    Explanation = "2FA adds an extra layer of protection."
                },

                new QuizQuestion
                {
                    Question = "Which information should not be shared publicly?",
                    Answers = new List<string>
                    {
                        "Favourite food",
                        "ID number",
                        "Movie preferences",
                        "Sports team"
                    },
                    CorrectAnswer = 1,
                    Explanation = "Identity information should be kept private."
                },

                new QuizQuestion
                {
                    Question = "What should you do before clicking a link?",
                    Answers = new List<string>
                    {
                        "Check the URL",
                        "Click immediately",
                        "Forward it",
                        "Ignore it"
                    },
                    CorrectAnswer = 0,
                    Explanation = "Always verify links before clicking."
                },

                new QuizQuestion
                {
                    Question = "True or False: Antivirus software is unnecessary.",
                    Answers = new List<string>
                    {
                        "True",
                        "False"
                    },
                    CorrectAnswer = 1,
                    Explanation = "Antivirus software helps detect threats."
                },

                new QuizQuestion
                {
                    Question = "Which is a common scam tactic?",
                    Answers = new List<string>
                    {
                        "Creating urgency",
                        "Giving free money",
                        "Both",
                        "Neither"
                    },
                    CorrectAnswer = 2,
                    Explanation = "Scammers often create urgency and unrealistic offers."
                }
            };
        }
    }
}