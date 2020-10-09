using System;
using HyperMarket.Data.SqlServer.Maps;
using Microsoft.EntityFrameworkCore;
using HyperMarket.DomainObjects;
using System.Collections.Generic;
using System.Text;

namespace HyperMarket.Data.SqlServer
{
    public class MainContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public MainContext() { }
        public MainContext(DbContextOptions<MainContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserMap());
        }
    }
}
