using System;
using System.Collections.Generic;
using MySql.Data.MySqlClient;
using CyberSecurityAwarenessBot.Data;
using CyberSecurityAwarenessBot.Models;

namespace CyberSecurityAwarenessBot.Services
{
    public class TaskService
    {
        private readonly DatabaseService _databaseService = new DatabaseService();

        public void AddTask(CyberTask task)
        {
            using var connection = _databaseService.GetConnection();
            connection.Open();

            string query = @"INSERT INTO Tasks 
                            (Title, Description, ReminderDate, IsCompleted)
                            VALUES 
                            (@Title, @Description, @ReminderDate, @IsCompleted)";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@Title", task.Title);
            command.Parameters.AddWithValue("@Description", task.Description);
            command.Parameters.AddWithValue("@ReminderDate", task.ReminderDate);
            command.Parameters.AddWithValue("@IsCompleted", task.IsCompleted);

            command.ExecuteNonQuery();
        }

        public List<CyberTask> GetAllTasks()
        {
            List<CyberTask> tasks = new List<CyberTask>();

            using var connection = _databaseService.GetConnection();
            connection.Open();

            string query = "SELECT * FROM Tasks";

            using var command = new MySqlCommand(query, connection);
            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                tasks.Add(new CyberTask
                {
                    TaskId = Convert.ToInt32(reader["TaskId"]),
                    Title = reader["Title"].ToString(),
                    Description = reader["Description"].ToString(),
                    ReminderDate = Convert.ToDateTime(reader["ReminderDate"]),
                    IsCompleted = Convert.ToBoolean(reader["IsCompleted"])
                });
            }

            return tasks;
        }

        public void CompleteTask(int taskId)
        {
            using var connection = _databaseService.GetConnection();
            connection.Open();

            string query = "UPDATE Tasks SET IsCompleted = true WHERE TaskId = @TaskId";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@TaskId", taskId);

            command.ExecuteNonQuery();
        }

        public void DeleteTask(int taskId)
        {
            using var connection = _databaseService.GetConnection();
            connection.Open();

            string query = "DELETE FROM Tasks WHERE TaskId = @TaskId";

            using var command = new MySqlCommand(query, connection);
            command.Parameters.AddWithValue("@TaskId", taskId);

            command.ExecuteNonQuery();
        }
    }
}