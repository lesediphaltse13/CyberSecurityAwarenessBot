using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using CyberSecurityAwarenessBot.Data;
using CyberSecurityAwarenessBot.Models;

namespace CyberSecurityAwarenessBot.Services
{
    public class ActivityLogService
    {
        private readonly DatabaseService _databaseService = new DatabaseService();

        public void AddLog(string description)
        {
            using var connection = _databaseService.GetConnection();

            connection.Open();

            string query =
                @"INSERT INTO ActivityLogs
                  (ActionDescription)
                  VALUES
                  (@Description)";

            using var command = new MySqlCommand(query, connection);

            command.Parameters.AddWithValue("@Description", description);

            command.ExecuteNonQuery();
        }

        public List<ActivityLogEntry> GetRecentLogs()
        {
            List<ActivityLogEntry> logs = new();

            using var connection = _databaseService.GetConnection();

            connection.Open();

            string query =
                @"SELECT *
                  FROM ActivityLogs
                  ORDER BY ActionDate DESC
                  LIMIT 10";

            using var command = new MySqlCommand(query, connection);

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                logs.Add(new ActivityLogEntry
                {
                    LogId = Convert.ToInt32(reader["LogId"]),
                    ActionDescription = reader["ActionDescription"].ToString(),
                    ActionDate = Convert.ToDateTime(reader["ActionDate"])
                });
            }

            return logs;
        }
    }
}