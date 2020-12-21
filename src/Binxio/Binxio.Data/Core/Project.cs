namespace Binxio.Data
{
    public class Project : CodedObject
    {
        public long Id { get; set; }
        public string UrlPart { get; set; }
        public string Title { get; set; }
        public long? ClientId { get; set; }
        public Client Client { get; set; }
        public string ProjectDatabaseConnectionString { get; set; }
    }   
}