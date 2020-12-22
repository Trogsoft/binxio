namespace Binxio.Data
{
    public class LogContext
    {
        public long Id { get; set; }
        public long? LogId { get; set; }
        public Log Log { get; set; }
        public string Key { get; set; }
        public string Value { get; set; }
    }
}