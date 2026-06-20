using System;
using System.Collections.Generic;

namespace CyberSecurityAwarenessBot.Services
{
    public class ChatbotService
    {
        private readonly Random _random = new Random();
        private readonly MemoryService _memory = new MemoryService();
        private string _lastTopic = "";

        private readonly Dictionary<string, List<string>> _responses = new Dictionary<string, List<string>>
        {
            {
                "password", new List<string>
                {
                    "Use strong, unique passwords for every account.",
                    "Avoid using your name, birthday, or simple words as passwords.",
                    "A good password should include uppercase letters, lowercase letters, numbers, and symbols."
                }
            },
            {
                "phishing", new List<string>
                {
                    "Phishing is when attackers trick you into giving away personal information.",
                    "Always check the sender's email address before clicking links.",
                    "Do not enter your password on websites opened from suspicious emails."
                }
            },
            {
                "scam", new List<string>
                {
                    "Online scams often create urgency, like saying your account will be closed.",
                    "Never send money or personal details to someone you do not trust.",
                    "If an offer sounds too good to be true, it is probably a scam."
                }
            },
            {
                "privacy", new List<string>
                {
                    "Review your social media privacy settings regularly.",
                    "Do not share personal details like your ID number or address online.",
                    "Use two-factor authentication to protect private accounts."
                }
            },
            {
                "browsing", new List<string>
                {
                    "Only use secure websites that start with https.",
                    "Avoid downloading files from unknown websites.",
                    "Do not click pop-up ads that claim your device is infected."
                }
            }
        };

        public string GetResponse(string userInput)
        {
            if (string.IsNullOrWhiteSpace(userInput))
            {
                return "Please type something so I can help you.";
            }

            string input = userInput.ToLower();

            // Memory: user name
            if (input.StartsWith("my name is"))
            {
                string name = userInput.Replace("My name is", "", StringComparison.OrdinalIgnoreCase).Trim();

                _memory.UserName = name;

                return $"Nice to meet you, {name}. I'll remember your name.";
            }

            // Memory: favourite topic
            if (input.Contains("interested in privacy"))
            {
                _memory.FavouriteTopic = "privacy";

                return "Great! I'll remember that you're interested in privacy.";
            }

            if (input.Contains("interested in password"))
            {
                _memory.FavouriteTopic = "password safety";

                return "Great! I'll remember that you're interested in password safety.";
            }

            if (input.Contains("interested in phishing"))
            {
                _memory.FavouriteTopic = "phishing";

                return "Great! I'll remember that you're interested in phishing awareness.";
            }

            if (input.Contains("interested in scams") || input.Contains("interested in scam"))
            {
                _memory.FavouriteTopic = "scams";

                return "Great! I'll remember that you're interested in learning about scams.";
            }

            // Memory recall
            if (input.Contains("remember") || input.Contains("what do you know about me"))
            {
                if (!string.IsNullOrEmpty(_memory.UserName) && !string.IsNullOrEmpty(_memory.FavouriteTopic))
                {
                    return $"I remember that your name is {_memory.UserName} and you're interested in {_memory.FavouriteTopic}.";
                }

                if (!string.IsNullOrEmpty(_memory.UserName))
                {
                    return $"I remember that your name is {_memory.UserName}.";
                }

                if (!string.IsNullOrEmpty(_memory.FavouriteTopic))
                {
                    return $"I remember that you're interested in {_memory.FavouriteTopic}.";
                }

                return "I don't have anything saved in memory yet.";
            }

            // Sentiment detection
            if (input.Contains("worried"))
            {
                return "It's understandable to feel worried about cybersecurity threats. Let me help you stay safe. A good first step is enabling two-factor authentication.";
            }

            if (input.Contains("frustrated"))
            {
                return "Cybersecurity can feel overwhelming at first, but you're already taking the right step by learning about it.";
            }

            if (input.Contains("curious"))
            {
                return "That's great! Curiosity is one of the best ways to improve your cybersecurity knowledge.";
            }

            // Basic questions
            if (input.Contains("how are you"))
            {
                return "I'm running well and ready to help you stay safe online.";
            }

            if (input.Contains("purpose") || input.Contains("what do you do"))
            {
                return "My purpose is to teach you about cybersecurity threats such as phishing, scams, weak passwords, and unsafe browsing.";
            }

            if (input.Contains("what can i ask"))
            {
                return "You can ask me about password safety, phishing, scams, privacy, and safe browsing.";
            }

            // Follow-up conversation flow
            if (input.Contains("tell me more") || input.Contains("another tip") || input.Contains("explain more"))
            {
                if (!string.IsNullOrEmpty(_lastTopic))
                {
                    return GetRandomResponse(_lastTopic);
                }

                return "Tell me which cybersecurity topic you want to learn more about.";
            }

            // Keyword recognition
            foreach (var topic in _responses.Keys)
            {
                if (input.Contains(topic))
                {
                    _lastTopic = topic;
                    return GetRandomResponse(topic);
                }
            }

            return "I'm not sure I understand. Can you try rephrasing your cybersecurity question?";
        }

        private string GetRandomResponse(string topic)
        {
            var topicResponses = _responses[topic];
            int index = _random.Next(topicResponses.Count);
            return topicResponses[index];
        }
    }
}