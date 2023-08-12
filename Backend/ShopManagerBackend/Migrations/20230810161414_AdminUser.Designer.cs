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
    [Migration("20230810161414_AdminUser")]
    partial class AdminUser
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

                    b.Property<string>("Sku")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.ToTable("Products", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            ImagePath = "Assets/Hat.svg",
                            Name = "Hat",
                            Price = 16.90m,
                            Quantity = 40,
                            Sku = "HX421RO"
                        },
                        new
                        {
                            Id = 2,
                            ImagePath = "Assets/Hoodie.svg",
                            Name = "Hoodie",
                            Price = 35.90m,
                            Quantity = 80,
                            Sku = "MN632FD"
                        },
                        new
                        {
                            Id = 3,
                            ImagePath = "Assets/Jacket.svg",
                            Name = "Jacket",
                            Price = 49.90m,
                            Quantity = 50,
                            Sku = "DX218ED"
                        },
                        new
                        {
                            Id = 4,
                            ImagePath = "Assets/Pants.svg",
                            Name = "Pants",
                            Price = 27.90m,
                            Quantity = 70,
                            Sku = "SI972YV"
                        },
                        new
                        {
                            Id = 5,
                            ImagePath = "Assets/Shirt.svg",
                            Name = "Shirt",
                            Price = 19.90m,
                            Quantity = 35,
                            Sku = "FL560RX"
                        },
                        new
                        {
                            Id = 6,
                            ImagePath = "Assets/Shoes.svg",
                            Name = "Shoes",
                            Price = 59.90m,
                            Quantity = 90,
                            Sku = "QJ736BG"
                        },
                        new
                        {
                            Id = 7,
                            ImagePath = "Assets/Sweater.svg",
                            Name = "Sweater",
                            Price = 30.90m,
                            Quantity = 25,
                            Sku = "CM214JK"
                        });
                });

            modelBuilder.Entity("ShopManagerBackend.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<byte[]>("PasswordHash")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<string>("Username")
                        .IsRequired()
                        .HasMaxLength(25)
                        .HasColumnType("nvarchar(25)");

                    b.HasKey("Id");

                    b.ToTable("Users", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            PasswordHash = new byte[] { 115, 199, 173, 186, 243, 213, 206, 109, 9, 204, 35, 246, 203, 155, 77, 220, 7, 87, 190, 35, 35, 97, 141, 53, 40, 6, 95, 117, 164, 162, 200, 59, 108, 234, 246, 131, 114, 240, 200, 2, 125, 13, 222, 170, 84, 148, 0, 99, 140, 192, 195, 141, 81, 152, 87, 66, 199, 157, 2, 195, 33, 210, 237, 27 },
                            PasswordSalt = new byte[] { 132, 237, 165, 254, 67, 4, 96, 18, 220, 27, 240, 168, 24, 140, 237, 183, 70, 222, 95, 234, 231, 125, 35, 235, 30, 57, 148, 122, 139, 216, 217, 9, 235, 2, 10, 153, 228, 24, 250, 36, 57, 166, 37, 238, 182, 150, 233, 234, 82, 107, 28, 146, 240, 175, 227, 18, 4, 95, 47, 68, 155, 115, 115, 29, 226, 121, 141, 25, 228, 99, 193, 107, 151, 199, 197, 5, 144, 206, 250, 103, 38, 7, 242, 224, 6, 152, 30, 41, 158, 75, 89, 34, 173, 136, 26, 132, 192, 59, 36, 211, 58, 88, 125, 102, 161, 225, 71, 192, 159, 148, 52, 163, 31, 51, 193, 13, 164, 246, 244, 39, 79, 254, 4, 24, 5, 63, 200, 113 },
                            Role = "Admin",
                            Username = "admin"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}