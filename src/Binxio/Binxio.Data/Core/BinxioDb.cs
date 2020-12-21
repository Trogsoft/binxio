﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Binxio.Data
{
    public class BinxioDb : DbContext
    {
        public BinxioDb(DbContextOptions options) : base(options)
        {
        }

        protected BinxioDb()
        {
        }

        public DbSet<Provider> ServiceProviders { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Provider>(p =>
            {
                p.HasMany(x => x.Clients)
                .WithOne(x => x.ServiceProvider)
                .HasForeignKey(x => x.ProviderId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<Client>(c =>
            {
                c.HasMany(x => x.Users)
                .WithOne(x => x.Client)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

                c.HasMany(x => x.Projects)
                .WithOne(x => x.Client)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
            });

            base.OnModelCreating(modelBuilder);
        }

        public string GetUrlPart<T>(string displayName) where T : CodedObject
        {
            Regex rgx = new Regex("[^a-zA-Z0-9-]");
            var basePart = rgx.Replace(displayName.ToLower().Replace(' ', '-'), "");
            var part = basePart;
            var i = 1;
            while (this.Set<T>().Where(x => x.UrlPart == part).Any())
            {
                part = basePart + "-" + i;
                i++;
            }
            return part;
        }
    }
}
