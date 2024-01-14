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
        public DbSet<Passport> Passports { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Picture> Pictures { get; set; }
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

            modelBuilder.Entity<Distributor>()
           .HasOne(d => d.Passport)
           .WithOne(p => p.Distributor)
           .HasForeignKey<Passport>(p => p.DistributorID);

            modelBuilder.Entity<ContactInfo>()
            .HasOne(a => a.Distributor)
            .WithMany(d => d.ContactInfos)
            .HasForeignKey(a => a.DistributorID)
            .HasPrincipalKey(d => d.DistributorID);

            modelBuilder.Entity<Picture>()
            .HasOne(a => a.Distributor)
            .WithMany(d => d.Pictures)
            .HasForeignKey(a => a.DistributorID)
            .HasPrincipalKey(d => d.DistributorID);


            //Seed DB
            modelBuilder.Entity<Product>().HasData(
                new Product()
                {
                    ID = 1,
                    Name = "Pen",
                    Price = 10
                },
                new Product()
                {
                    ID = 2,
                    Name = "Pencil",
                    Price = 9
                },
                new Product()
                {
                    ID = 3,
                    Name = "Book",
                    Price = 20,
                },
                new Product()
                {
                    ID = 4,
                    Name = "Notepad",
                    Price = 14
                });

            modelBuilder.Entity<Distributor>().HasData(
                new Distributor()
                {
                    DistributorID = 1,
                    DistributorGuid = Guid.NewGuid(),
                    Gender = Common.Enums.Gender.Male,
                    FirstName = "Zura",
                    LastName = "Samkharadze",
                    BirthDate = new DateTime(1990, 1, 18),
                    GenerationLinker = null
                },
                new Distributor
                {
                    DistributorID = 2,
                    DistributorGuid = Guid.NewGuid(),
                    Gender = Common.Enums.Gender.Female,
                    FirstName = "Maiko",
                    LastName = "Samkharadze",
                    GenerationLinker = null
                });


            modelBuilder.Entity<Address>().HasData(
                new Address()
                {
                    ID = 1,
                    AddressType = AddressType.Actual,
                    AddressInfo = "საჯაიას 10",
                    DistributorID = 1
                },
                new Address()
                {
                    ID = 2,
                    AddressType = AddressType.Registration,
                    AddressInfo = "ბოხუას 10",
                    DistributorID = 2
                });

            modelBuilder.Entity<Passport>().HasData(
                new Passport()
                {
                    ID = 1,
                    DocumentNumber = "102340",
                    DocumentSerie = "11111",
                    DocumentType = DocumentType.Pasport,
                    ExpirationDate = new DateTime(2028, 1, 1),
                    IssuingAgency = "SA Agency",
                    PersonalNumber = "01008048552",
                    ReleaseDate = DateTime.Now,
                    DistributorID = 1,
                },
                new Passport()
                {
                    ID = 2,
                    DocumentNumber = "102340",
                    DocumentSerie = "11111",
                    DocumentType = DocumentType.Pasport,
                    ExpirationDate = new DateTime(2028, 1, 1),
                    IssuingAgency = "SA Agency",
                    PersonalNumber = "599473377",
                    ReleaseDate = DateTime.Now,
                    DistributorID = 2
                });

            modelBuilder.Entity<ContactInfo>().HasData(
                new ContactInfo()
                {
                    ID = 1,
                    DistributorID = 1,
                    ContactInformationType = ContactInformationType.Mobile,
                    Information = "599473377"
                },
                new ContactInfo()
                {
                    ID = 2,
                    DistributorID = 2,
                    ContactInformationType = ContactInformationType.Email,
                    Information = "MaikoMaiko@Gmail.com"
                });
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