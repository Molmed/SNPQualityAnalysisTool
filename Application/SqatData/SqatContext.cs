using System;
using System.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SqatData.Entities;

namespace SqatData
{
    public partial class SqatContext : DbContext
    {
        private string _dbname;
        public SqatContext(string dbname)
        {
            _dbname = dbname;
        }

        public SqatContext(DbContextOptions<SqatContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Plate> Plates { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connStr = ConfigurationManager.ConnectionStrings["DevDb"].ConnectionString;
                connStr = connStr.Replace("db_name", _dbname);
                optionsBuilder.UseSqlServer(connStr);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Query<Plate>(entity =>
            {
                entity.Property(e => e.Identifier)
                    .HasColumnName("identifier");
            });
        }
    }
}
