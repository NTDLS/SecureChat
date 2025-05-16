using static Talkster.Library.ScConstants;

namespace Talkster.Client
{
    public class LogEntry
    {
        public string Message { get; set; }
        public DateTime TimestampUTC { get; set; } = DateTime.UtcNow;

        public ScErrorLevel Severity { get; set; }

        public LogEntry(ScErrorLevel severity, string message)
        {
            Severity = severity;
            Message = message;
        }
    }
}
