namespace accountSystem.Domain.Entities
{
    public class TransactionLog
    {
        public int TransactionLogId { get; set; } // Primary Key
        public string LogType { get; set; } // Type of log (e.g., create, update, delete)
        public string OldValue { get; set; } // Previous value of the modified field
        public string NewValue { get; set; } // New value of the modified field
        public DateTime LogDate { get; set; } // Date of the log entry

        public DateTime CreatedAt { get; set; }
    }
}
