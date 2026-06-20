namespace CyberSecurityAwarenessBot.Models
{
    public class ActivityLogEntry
    {
        public int LogId { get; set; }

        public string ActionDescription { get; set; }

        public DateTime ActionDate { get; set; }
    }
}