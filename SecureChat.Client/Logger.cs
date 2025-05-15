using static SecureChat.Library.ScConstants;

namespace SecureChat.Client
{
    public class Logger
    {
        public List<LogEntry> Entries { get; private set; } = new();

        public void Clear()
        {
            lock (Entries)
            {
                Entries.Clear();
            }
        }

        private void RemoveStaleEntries()
        {
            Entries.RemoveAll(entry => (DateTime.UtcNow - entry.TimestampUTC).TotalMinutes > 60);
        }

        public void Error(string message)
        {
            lock (Entries)
            {
                RemoveStaleEntries();
                Entries.Add(new LogEntry(ScErrorLevel.Error, message));
            }
        }

        public void Error(Exception ex)
        {
            lock (Entries)
            {
                RemoveStaleEntries();
                Entries.Add(new LogEntry(ScErrorLevel.Error, ex.GetBaseException().Message));
            }
        }

        public void Error(string message, Exception ex)
        {
            lock (Entries)
            {
                RemoveStaleEntries();
                Entries.Add(new LogEntry(ScErrorLevel.Error, $"{message}\r\n{ex.GetBaseException().Message}"));
            }
        }

        public void Fatal(string message)
        {
            lock (Entries)
            {
                RemoveStaleEntries();
                Entries.Add(new LogEntry(ScErrorLevel.Fatal, message));
            }
        }

        public void Fatal(Exception ex)
        {
            lock (Entries)
            {
                RemoveStaleEntries();
                Entries.Add(new LogEntry(ScErrorLevel.Fatal, ex.GetBaseException().Message));
            }
        }

        public void Fatal(string message, Exception ex)
        {
            lock (Entries)
            {
                RemoveStaleEntries();
                Entries.Add(new LogEntry(ScErrorLevel.Fatal, $"{message}\r\n{ex.GetBaseException().Message}"));
            }
        }

        public void Warning(string message)
        {
            lock (Entries)
            {
                RemoveStaleEntries();
                Entries.Add(new LogEntry(ScErrorLevel.Warning, message));
            }
        }

        public void Warning(Exception ex)
        {
            lock (Entries)
            {
                RemoveStaleEntries();
                Entries.Add(new LogEntry(ScErrorLevel.Warning, ex.GetBaseException().Message));
            }
        }

        public void Warning(string message, Exception ex)
        {
            lock (Entries)
            {
                RemoveStaleEntries();
                Entries.Add(new LogEntry(ScErrorLevel.Warning, $"{message}\r\n{ex.GetBaseException().Message}"));
            }
        }

        public void Verbose(string message)
        {
            lock (Entries)
            {
                RemoveStaleEntries();
                Entries.Add(new LogEntry(ScErrorLevel.Verbose, message));
            }
        }

        public void Information(string message)
        {
            lock (Entries)
            {
                RemoveStaleEntries();
                Entries.Add(new LogEntry(ScErrorLevel.Information, message));
            }
        }
    }
}
