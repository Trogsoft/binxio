using Microsoft.EntityFrameworkCore;
using System;

namespace Binxio.Data
{
    public class BinxioDb : DbContext
    {
        public DbSet<Provider> ServiceProviders { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Project> Projects { get; set; }
    }
}
