namespace Binxio.Data
{
    public class Provider : ICodedObject
    {
        public long Id { get; set; }
        public string UrlPart { get; set; }
        public string Title { get; set; }
    }
}