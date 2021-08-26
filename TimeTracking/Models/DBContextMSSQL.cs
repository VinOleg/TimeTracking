using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TimeTracking.Models
{
    public class DBContextMSSQL : DbContext
    {   
        public DbSet<Users> Users { get; set; }
        public DbSet<DailyWork> DailyWork { get; set; }
        public DBContextMSSQL(DbContextOptions<DBContextMSSQL> options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasAlternateKey(u => u.Email);
        }
    }
}
