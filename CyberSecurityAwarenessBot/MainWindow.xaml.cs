using System;
using System.Collections.Generic;
using System.Windows;
using CyberSecurityAwarenessBot.Models;
using CyberSecurityAwarenessBot.Services;
using System.Windows.Controls;

namespace CyberSecurityAwarenessBot
{
    public partial class MainWindow : Window
    {
        private readonly ChatbotService _chatbotService;
        private readonly TaskService _taskService;
        private readonly ActivityLogService _activityLogService;
        private readonly QuizService _quizService;
        private readonly NlpService _nlpService;

        private List<QuizQuestion> _quizQuestions;
        private int _currentQuestionIndex;
        private int _score;
        private bool _answerSelected;

        public MainWindow()
        {
            InitializeComponent();

            _chatbotService = new ChatbotService();
            _taskService = new TaskService();
            _activityLogService = new ActivityLogService();
            _quizService = new QuizService();
            _nlpService = new NlpService();
            _quizQuestions = new List<QuizQuestion>();

            ChatHistory.Items.Add("Bot: Welcome to the Cybersecurity Awareness Bot.");
            ChatHistory.Items.Add("Bot: Ask me about password safety, phishing, scams, privacy, or safe browsing.");
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userMessage = MessageTextBox.Text;

            ChatHistory.Items.Add("You: " + userMessage);

            // NLP Task Detection
            if (_nlpService.IsTaskCommand(userMessage))
            {
                string taskTitle = _nlpService.ExtractTaskTitle(userMessage);

                CyberTask task = new CyberTask
                {
                    Title = taskTitle,
                    Description = "Created through NLP command",
                    ReminderDate = _nlpService.ExtractReminderDate(userMessage),
                    IsCompleted = false
                };

                _taskService.AddTask(task);

                _activityLogService.AddLog($"NLP Task Added: {taskTitle}");

                ChatHistory.Items.Add($"Bot: Task added successfully: {taskTitle}");

                MessageTextBox.Clear();
                return;
            }

            // NLP Activity Log Detection
            if (_nlpService.IsActivityLogCommand(userMessage))
            {
                var logs = _activityLogService.GetRecentLogs();

                ChatHistory.Items.Add("Bot: Here are my recent actions:");

                foreach (var log in logs)
                {
                    ChatHistory.Items.Add($"• {log.ActionDescription}");
                }

                MessageTextBox.Clear();
                return;
            }

            // NLP Quiz Detection
            if (_nlpService.IsQuizCommand(userMessage))
            {
                ChatHistory.Items.Add("Bot: Opening the Cybersecurity Quiz.");

                ChatPanel.Visibility = Visibility.Collapsed;
                TaskPanel.Visibility = Visibility.Collapsed;
                ActivityLogPanel.Visibility = Visibility.Collapsed;
                QuizPanel.Visibility = Visibility.Visible;

                MessageTextBox.Clear();
                return;
            }

            string botResponse = _chatbotService.GetResponse(userMessage);

            ChatHistory.Items.Add("Bot: " + botResponse);

            MessageTextBox.Clear();
        }

        private void ChatButton_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Visibility = Visibility.Visible;
            TaskPanel.Visibility = Visibility.Collapsed;
            QuizPanel.Visibility = Visibility.Collapsed;
            ActivityLogPanel.Visibility = Visibility.Collapsed;
        }

        private void TasksButton_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Visibility = Visibility.Collapsed;
            TaskPanel.Visibility = Visibility.Visible;
            QuizPanel.Visibility = Visibility.Collapsed;
            ActivityLogPanel.Visibility = Visibility.Collapsed;

            LoadTasks();
        }

        private void QuizButton_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Visibility = Visibility.Collapsed;
            TaskPanel.Visibility = Visibility.Collapsed;
            QuizPanel.Visibility = Visibility.Visible;
            ActivityLogPanel.Visibility = Visibility.Collapsed;
        }

        private void LogButton_Click(object sender, RoutedEventArgs e)
        {
            ChatPanel.Visibility = Visibility.Collapsed;
            TaskPanel.Visibility = Visibility.Collapsed;
            QuizPanel.Visibility = Visibility.Collapsed;
            ActivityLogPanel.Visibility = Visibility.Visible;

            LoadActivityLogs();
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TaskTitleTextBox.Text))
            {
                MessageBox.Show("Please enter a task title.");
                return;
            }

            CyberTask task = new CyberTask
            {
                Title = TaskTitleTextBox.Text,
                Description = TaskDescriptionTextBox.Text,
                ReminderDate = ReminderDatePicker.SelectedDate ?? DateTime.Now,
                IsCompleted = false
            };

            _taskService.AddTask(task);
            _activityLogService.AddLog($"Task added: {task.Title}");

            TaskTitleTextBox.Clear();
            TaskDescriptionTextBox.Clear();
            ReminderDatePicker.SelectedDate = null;

            LoadTasks();

            MessageBox.Show("Task added successfully.");
        }

        private void RefreshTasksButton_Click(object sender, RoutedEventArgs e)
        {
            LoadTasks();
        }

        private void CompleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TasksDataGrid.SelectedItem is CyberTask selectedTask)
            {
                _taskService.CompleteTask(selectedTask.TaskId);
                _activityLogService.AddLog($"Task completed: {selectedTask.Title}");

                LoadTasks();

                MessageBox.Show("Task marked as completed.");
            }
            else
            {
                MessageBox.Show("Please select a task first.");
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TasksDataGrid.SelectedItem is CyberTask selectedTask)
            {
                _taskService.DeleteTask(selectedTask.TaskId);
                _activityLogService.AddLog($"Task deleted: {selectedTask.Title}");

                LoadTasks();

                MessageBox.Show("Task deleted successfully.");
            }
            else
            {
                MessageBox.Show("Please select a task first.");
            }
        }

        private void RefreshLogButton_Click(object sender, RoutedEventArgs e)
        {
            LoadActivityLogs();
        }

        private void StartQuizButton_Click(object sender, RoutedEventArgs e)
        {
            _quizQuestions = _quizService.GetQuestions();
            _currentQuestionIndex = 0;
            _score = 0;
            _answerSelected = false;

            _activityLogService.AddLog("Quiz started");

            ShowQuestion();
        }

        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {
            if (_quizQuestions.Count == 0)
            {
                MessageBox.Show("Please start the quiz first.");
                return;
            }

            if (_answerSelected)
            {
                MessageBox.Show("You already answered this question. Click Next Question.");
                return;
            }

            Button selectedButton = sender as Button;
            int selectedAnswer = Convert.ToInt32(selectedButton.Tag);

            QuizQuestion currentQuestion = _quizQuestions[_currentQuestionIndex];

            if (selectedAnswer == currentQuestion.CorrectAnswer)
            {
                _score++;
                QuizFeedbackTextBlock.Text = "Correct! " + currentQuestion.Explanation;
            }
            else
            {
                QuizFeedbackTextBlock.Text = "Incorrect. " + currentQuestion.Explanation;
            }

            _answerSelected = true;
            QuizStatusTextBlock.Text = $"Question {_currentQuestionIndex + 1} of {_quizQuestions.Count} | Score: {_score}";
        }

        private void NextQuestionButton_Click(object sender, RoutedEventArgs e)
        {
            if (_quizQuestions.Count == 0)
            {
                MessageBox.Show("Please start the quiz first.");
                return;
            }

            if (!_answerSelected)
            {
                MessageBox.Show("Please answer the current question first.");
                return;
            }

            _currentQuestionIndex++;

            if (_currentQuestionIndex >= _quizQuestions.Count)
            {
                EndQuiz();
                return;
            }

            _answerSelected = false;
            ShowQuestion();
        }

        private void ShowQuestion()
        {
            QuizQuestion currentQuestion = _quizQuestions[_currentQuestionIndex];

            QuestionTextBlock.Text = currentQuestion.Question;
            QuizFeedbackTextBlock.Text = "";

            AnswerButton0.Content = currentQuestion.Answers.Count > 0 ? currentQuestion.Answers[0] : "";
            AnswerButton1.Content = currentQuestion.Answers.Count > 1 ? currentQuestion.Answers[1] : "";
            AnswerButton2.Content = currentQuestion.Answers.Count > 2 ? currentQuestion.Answers[2] : "";
            AnswerButton3.Content = currentQuestion.Answers.Count > 3 ? currentQuestion.Answers[3] : "";

            AnswerButton0.Tag = 0;
            AnswerButton1.Tag = 1;
            AnswerButton2.Tag = 2;
            AnswerButton3.Tag = 3;

            AnswerButton0.Visibility = currentQuestion.Answers.Count > 0 ? Visibility.Visible : Visibility.Collapsed;
            AnswerButton1.Visibility = currentQuestion.Answers.Count > 1 ? Visibility.Visible : Visibility.Collapsed;
            AnswerButton2.Visibility = currentQuestion.Answers.Count > 2 ? Visibility.Visible : Visibility.Collapsed;
            AnswerButton3.Visibility = currentQuestion.Answers.Count > 3 ? Visibility.Visible : Visibility.Collapsed;

            QuizStatusTextBlock.Text = $"Question {_currentQuestionIndex + 1} of {_quizQuestions.Count} | Score: {_score}";
        }

        private void EndQuiz()
        {
            string finalMessage;

            if (_score >= 8)
            {
                finalMessage = $"Great job! You're a cybersecurity pro. Final score: {_score}/{_quizQuestions.Count}";
            }
            else if (_score >= 5)
            {
                finalMessage = $"Good effort. Keep learning to stay safe online. Final score: {_score}/{_quizQuestions.Count}";
            }
            else
            {
                finalMessage = $"Keep practising. Cybersecurity awareness improves with learning. Final score: {_score}/{_quizQuestions.Count}";
            }

            QuestionTextBlock.Text = finalMessage;
            QuizFeedbackTextBlock.Text = "Quiz completed.";

            AnswerButton0.Visibility = Visibility.Collapsed;
            AnswerButton1.Visibility = Visibility.Collapsed;
            AnswerButton2.Visibility = Visibility.Collapsed;
            AnswerButton3.Visibility = Visibility.Collapsed;

            QuizStatusTextBlock.Text = "Quiz finished.";

            _activityLogService.AddLog($"Quiz completed with score {_score}/{_quizQuestions.Count}");
        }

        private void LoadTasks()
        {
            TasksDataGrid.ItemsSource = _taskService.GetAllTasks();
        }

        private void LoadActivityLogs()
        {
            ActivityLogDataGrid.ItemsSource = _activityLogService.GetRecentLogs();
        }
    }
}