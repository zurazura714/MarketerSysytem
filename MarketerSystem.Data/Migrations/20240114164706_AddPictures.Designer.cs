﻿// <auto-generated />
using System;
using MarketerSystem.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MarketerSystem.Data.Migrations
{
    [DbContext(typeof(MarketerDBContext))]
    [Migration("20240114164706_AddPictures")]
    partial class AddPictures
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("MarketerSystem.Domain.Model.Address", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("AddressInfo")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("AddressType")
                        .HasColumnType("int");

                    b.Property<int>("DistributorID")
                        .HasColumnType("int");

                    b.HasKey("ID");

                    b.HasIndex("DistributorID");

                    b.ToTable("Addresses");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            AddressInfo = "საჯაიას 10",
                            AddressType = 1,
                            DistributorID = 1
                        },
                        new
                        {
                            ID = 2,
                            AddressInfo = "ბოხუას 10",
                            AddressType = 2,
                            DistributorID = 2
                        });
                });

            modelBuilder.Entity("MarketerSystem.Domain.Model.BonusPayment", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<decimal>("BonusPay")
                        .HasColumnType("decimal(18,2)");

                    b.Property<int>("DistributorID")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("FromDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("ToDate")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.HasIndex("DistributorID");

                    b.ToTable("BonusPayments");
                });

            modelBuilder.Entity("MarketerSystem.Domain.Model.ContactInfo", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("ContactInformationType")
                        .HasColumnType("int");

                    b.Property<int>("DistributorID")
                        .HasColumnType("int");

                    b.Property<string>("Information")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("ID");

                    b.HasIndex("DistributorID");

                    b.ToTable("ContactInfo");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            ContactInformationType = 2,
                            DistributorID = 1,
                            Information = "599473377"
                        },
                        new
                        {
                            ID = 2,
                            ContactInformationType = 3,
                            DistributorID = 2,
                            Information = "MaikoMaiko@Gmail.com"
                        });
                });

            modelBuilder.Entity("MarketerSystem.Domain.Model.Distributor", b =>
                {
                    b.Property<int>("DistributorID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("DistributorID"));

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("datetime2");

                    b.Property<Guid>("DistributorGuid")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Gender")
                        .HasColumnType("int");

                    b.Property<string>("GenerationLinker")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("PassportID")
                        .HasColumnType("int");

                    b.Property<int?>("RecomendatorID")
                        .HasColumnType("int");

                    b.HasKey("DistributorID");

                    b.HasIndex("DistributorGuid")
                        .IsUnique();

                    b.HasIndex("RecomendatorID");

                    b.ToTable("Distributors");

                    b.HasData(
                        new
                        {
                            DistributorID = 1,
                            BirthDate = new DateTime(1990, 1, 18, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DistributorGuid = new Guid("20be1849-eb96-4d11-8016-a4da2f8658cc"),
                            FirstName = "Zura",
                            Gender = 1,
                            LastName = "Samkharadze",
                            PassportID = 0
                        },
                        new
                        {
                            DistributorID = 2,
                            BirthDate = new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                            DistributorGuid = new Guid("0f794ec4-612d-4311-98b4-5b3d82cff52a"),
                            FirstName = "Maiko",
                            Gender = 2,
                            LastName = "Samkharadze",
                            PassportID = 0
                        });
                });

            modelBuilder.Entity("MarketerSystem.Domain.Model.Passport", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("DistributorID")
                        .HasColumnType("int");

                    b.Property<string>("DocumentNumber")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<string>("DocumentSerie")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.Property<int>("DocumentType")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("ExpirationDate")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("IssuingAgency")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PersonalNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTimeOffset>("ReleaseDate")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.HasIndex("DistributorID")
                        .IsUnique();

                    b.ToTable("Passports");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            DistributorID = 1,
                            DocumentNumber = "102340",
                            DocumentSerie = "11111",
                            DocumentType = 1,
                            ExpirationDate = new DateTimeOffset(new DateTime(2028, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)),
                            IssuingAgency = "SA Agency",
                            PersonalNumber = "01008048552",
                            ReleaseDate = new DateTimeOffset(new DateTime(2024, 1, 14, 20, 47, 6, 29, DateTimeKind.Unspecified).AddTicks(6750), new TimeSpan(0, 4, 0, 0, 0))
                        },
                        new
                        {
                            ID = 2,
                            DistributorID = 2,
                            DocumentNumber = "102340",
                            DocumentSerie = "11111",
                            DocumentType = 1,
                            ExpirationDate = new DateTimeOffset(new DateTime(2028, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 4, 0, 0, 0)),
                            IssuingAgency = "SA Agency",
                            PersonalNumber = "599473377",
                            ReleaseDate = new DateTimeOffset(new DateTime(2024, 1, 14, 20, 47, 6, 29, DateTimeKind.Unspecified).AddTicks(6762), new TimeSpan(0, 4, 0, 0, 0))
                        });
                });

            modelBuilder.Entity("MarketerSystem.Domain.Model.Picture", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<byte[]>("Content")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<int>("DistributorID")
                        .HasColumnType("int");

                    b.Property<string>("FileName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("UploadTime")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.HasIndex("DistributorID");

                    b.ToTable("Picture");
                });

            modelBuilder.Entity("MarketerSystem.Domain.Model.Product", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("Price")
                        .HasColumnType("decimal(18,2)");

                    b.HasKey("ID");

                    b.ToTable("Products");

                    b.HasData(
                        new
                        {
                            ID = 1,
                            Name = "Pen",
                            Price = 10m
                        },
                        new
                        {
                            ID = 2,
                            Name = "Pencil",
                            Price = 9m
                        },
                        new
                        {
                            ID = 3,
                            Name = "Book",
                            Price = 20m
                        },
                        new
                        {
                            ID = 4,
                            Name = "Notepad",
                            Price = 14m
                        });
                });

            modelBuilder.Entity("MarketerSystem.Domain.Model.Sell", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("ID"));

                    b.Property<int>("DistributorID")
                        .HasColumnType("int");

                    b.Property<int>("ProductID")
                        .HasColumnType("int");

                    b.Property<decimal>("ProductPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ProductTotalPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("ProductUnitPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTimeOffset>("SoldDate")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("ID");

                    b.HasIndex("DistributorID");

                    b.HasIndex("ProductID");

                    b.ToTable("Sells");
                });

            modelBuilder.Entity("MarketerSystem.Domain.Model.Address", b =>
                {
                    b.HasOne("MarketerSystem.Domain.Model.Distributor", "Distributor")
                        .WithMany("Addresses")
                        .HasForeignKey("DistributorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Distributor");
                });

            modelBuilder.Entity("MarketerSystem.Domain.Model.BonusPayment", b =>
                {
                    b.HasOne("MarketerSystem.Domain.Model.Distributor", "Distributor")
                        .WithMany("BonusPayments")
                        .HasForeignKey("DistributorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Distributor");
                });

            modelBuilder.Entity("MarketerSystem.Domain.Model.ContactInfo", b =>
                {
                    b.HasOne("MarketerSystem.Domain.Model.Distributor", "Distributor")
                        .WithMany("ContactInfos")
                        .HasForeignKey("DistributorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Distributor");
                });

            modelBuilder.Entity("MarketerSystem.Domain.Model.Distributor", b =>
                {
                    b.HasOne("MarketerSystem.Domain.Model.Distributor", "Recomendator")
                        .WithMany()
                        .HasForeignKey("RecomendatorID");

                    b.Navigation("Recomendator");
                });

            modelBuilder.Entity("MarketerSystem.Domain.Model.Passport", b =>
                {
                    b.HasOne("MarketerSystem.Domain.Model.Distributor", "Distributor")
                        .WithOne("Passport")
                        .HasForeignKey("MarketerSystem.Domain.Model.Passport", "DistributorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Distributor");
                });

            modelBuilder.Entity("MarketerSystem.Domain.Model.Picture", b =>
                {
                    b.HasOne("MarketerSystem.Domain.Model.Distributor", "Distributor")
                        .WithMany("Pictures")
                        .HasForeignKey("DistributorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Distributor");
                });

            modelBuilder.Entity("MarketerSystem.Domain.Model.Sell", b =>
                {
                    b.HasOne("MarketerSystem.Domain.Model.Distributor", "Distributor")
                        .WithMany()
                        .HasForeignKey("DistributorID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MarketerSystem.Domain.Model.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Distributor");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("MarketerSystem.Domain.Model.Distributor", b =>
                {
                    b.Navigation("Addresses");

                    b.Navigation("BonusPayments");

                    b.Navigation("ContactInfos");

                    b.Navigation("Passport")
                        .IsRequired();

                    b.Navigation("Pictures");
                });
#pragma warning restore 612, 618
        }
    }
}
