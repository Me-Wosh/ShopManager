﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ShopManagerBackend.Entities;

#nullable disable

namespace ShopManagerBackend.Migrations
{
    [DbContext(typeof(ShopManagerDbContext))]
    [Migration("20230614201853_Init")]
    partial class Init
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("ShopManagerBackend.Entities.Product", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<decimal>("Price")
                        .HasPrecision(14, 2)
                        .HasColumnType("decimal(14,2)");

                    b.Property<int>("Quantity")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(0);

                    b.HasKey("Id");

                    b.ToTable("Products", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ImagePath = "Assets/Hat.svg",
                            Name = "Hat",
                            Price = 16.90m,
                            Quantity = 40
                        },
                        new
                        {
                            Id = 2,
                            ImagePath = "Assets/Hoodie.svg",
                            Name = "Hoodie",
                            Price = 35.90m,
                            Quantity = 80
                        },
                        new
                        {
                            Id = 3,
                            ImagePath = "Assets/Jacket.svg",
                            Name = "Jacket",
                            Price = 49.90m,
                            Quantity = 50
                        },
                        new
                        {
                            Id = 4,
                            ImagePath = "Assets/Pants.svg",
                            Name = "Pants",
                            Price = 27.90m,
                            Quantity = 70
                        },
                        new
                        {
                            Id = 5,
                            ImagePath = "Assets/Shirt.svg",
                            Name = "Shirt",
                            Price = 19.90m,
                            Quantity = 35
                        },
                        new
                        {
                            Id = 6,
                            ImagePath = "Assets/Shoes.svg",
                            Name = "Shoes",
                            Price = 59.90m,
                            Quantity = 90
                        },
                        new
                        {
                            Id = 7,
                            ImagePath = "Assets/Sweater.svg",
                            Name = "Sweater",
                            Price = 30.90m,
                            Quantity = 25
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
