using MarketerSystem.Abstractions.Repository;
using MarketerSystem.Common.Enums;
using MarketerSystem.Domain.Model;
using Microsoft.EntityFrameworkCore;

namespace MarketerSystem.Data.Context;

public class MarketerDBContext(DbContextOptions<MarketerDBContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<Distributor> Distributors => Set<Distributor>();
    public DbSet<Passport> Passports => Set<Passport>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<Picture> Pictures => Set<Picture>();
    public DbSet<Sell> Sells => Set<Sell>();
    public DbSet<BonusPayment> BonusPayments => Set<BonusPayment>();

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

        SeedReferenceData(modelBuilder);
    }

    private static void SeedReferenceData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(
            new Product { ID = 1, Name = "Pen", Price = 10 },
            new Product { ID = 2, Name = "Pencil", Price = 9 },
            new Product { ID = 3, Name = "Book", Price = 20 },
            new Product { ID = 4, Name = "Notepad", Price = 14 });

        modelBuilder.Entity<Distributor>().HasData(
            new Distributor
            {
                DistributorID = 1,
                DistributorGuid = Guid.NewGuid(),
                Gender = Gender.Male,
                FirstName = "Zura",
                LastName = "Samkharadze",
                BirthDate = new DateTime(1990, 1, 18),
                GenerationLinker = null,
                PassportID = 1
            },
            new Distributor
            {
                DistributorID = 2,
                DistributorGuid = Guid.NewGuid(),
                Gender = Gender.Female,
                FirstName = "Maiko",
                LastName = "Samkharadze",
                GenerationLinker = null,
                PassportID = 2
            });

        modelBuilder.Entity<Address>().HasData(
            new Address { ID = 1, AddressType = AddressType.Actual, AddressInfo = "საჯაიას 10", DistributorID = 1 },
            new Address { ID = 2, AddressType = AddressType.Registration, AddressInfo = "ბოხუას 10", DistributorID = 2 });

        modelBuilder.Entity<Passport>().HasData(
            new Passport
            {
                ID = 1,
                DocumentNumber = "102340",
                DocumentSerie = "11111",
                DocumentType = DocumentType.Pasport,
                ExpirationDate = new DateTime(2028, 1, 1),
                IssuingAgency = "SA Agency",
                PersonalNumber = "01008048552",
                ReleaseDate = DateTime.Now,
                DistributorID = 1
            },
            new Passport
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
            new ContactInfo { ID = 1, DistributorID = 1, ContactInformationType = ContactInformationType.Mobile, Information = "599473377" },
            new ContactInfo { ID = 2, DistributorID = 2, ContactInformationType = ContactInformationType.Email, Information = "MaikoMaiko@Gmail.com" });

        modelBuilder.Entity<Sell>().HasData(
            new Sell { ID = 1, ProductID = 1, DistributorID = 1, ProductPrice = 10, ProductTotalPrice = 10, ProductUnitPrice = 10, SoldDate = DateTime.Now });

        modelBuilder.Entity<BonusPayment>().HasData(
            new BonusPayment { ID = 1, BonusPay = 10, DistributorID = 1, FromDate = DateTime.Now.AddHours(-5), ToDate = DateTime.Now });
    }

    public Task CommitAsync() => SaveChangesAsync();

    public Task RollbackAsync()
    {
        ChangeTracker.Clear();
        return Task.CompletedTask;
    }
}
