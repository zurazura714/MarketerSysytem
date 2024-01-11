using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Domain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MarketerSystem.Data.Context
{
    public class MarketerDBContext : DbContext, IUnitOfWork
    {
        public DbSet<Distributor> Distributors { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Sell> Sells { get; set; }
        public DbSet<BonusPayment> BonusPayments { get; set; }
        public MarketerDBContext(DbContextOptions<MarketerDBContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Distributor>()
            .HasIndex(d => d.DistributorGuid)
            .IsUnique();

            modelBuilder.Entity<Address>()
            .HasOne(a => a.Distributor)
            .WithMany(d => d.Addresses)
            .HasForeignKey(a => a.DistributorID)
            .HasPrincipalKey(d => d.DistributorID);

            modelBuilder.Entity<ContactInfo>()
            .HasOne(a => a.Distributor)
            .WithMany(d => d.ContactInfos)
            .HasForeignKey(a => a.DistributorID)
            .HasPrincipalKey(d => d.DistributorID);

        }

        public void Commit()
        {
            SaveChanges();
        }

        public void Rollback()
        {
            Rollback();
        }
    }
}