using Hotels.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hotels.Infrastructure.Context
{
    public class HotelsDbContext : DbContext
    {
        public HotelsDbContext(DbContextOptions options) : base(options) { }

        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Hotel>()
                .HasKey(h => h.Id);

            modelBuilder.Entity<Hotel>()
                .Property(h => h.Name)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
