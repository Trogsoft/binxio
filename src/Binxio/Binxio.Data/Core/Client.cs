namespace Binxio.Data
{
    public class Client : ICodedObject
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string UrlPart { get; set; }
        public long? ProviderId { get; set; }
        public Provider ServiceProvider { get; set; }
    }
}