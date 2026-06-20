using MySql.Data.MySqlClient;

namespace CyberSecurityAwarenessBot.Data
{
    public class DatabaseService
    {
        private readonly string _connectionString =
            "server=localhost;database=CyberSecurityChatbotDB;uid=root;pwd=#Sedi1412;";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(_connectionString);
        }
    }
}