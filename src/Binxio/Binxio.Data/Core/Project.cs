using Binxio.Common.Manage;
using Binxio.Common.Projects;

namespace Binxio.Data
{
    [XioList(typeof(ProjectModel))]
    public class Project : CodedObject
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public long? ClientId { get; set; }
        public Client Client { get; set; }
        public string ProjectDatabaseConnectionString { get; set; }
    }   
}