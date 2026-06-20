using System.Windows;
using CyberSecurityAwarenessBot.Services;

namespace CyberSecurityAwarenessBot
{
    public partial class MainWindow : Window
    {
        private readonly ChatbotService _chatbotService;

        public MainWindow()
        {
            InitializeComponent();

            _chatbotService = new ChatbotService();

            ChatHistory.Items.Add("Bot: Welcome to the Cybersecurity Awareness Bot.");
            ChatHistory.Items.Add("Bot: Ask me about password safety, phishing, scams, privacy, or safe browsing.");
        }

        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            string userMessage = MessageTextBox.Text;

            ChatHistory.Items.Add("You: " + userMessage);

            string botResponse = _chatbotService.GetResponse(userMessage);

            ChatHistory.Items.Add("Bot: " + botResponse);

            MessageTextBox.Clear();
        }
    }
}