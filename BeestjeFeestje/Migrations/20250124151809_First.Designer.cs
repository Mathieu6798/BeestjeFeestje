﻿// <auto-generated />
using System;
using BeestjeFeestje.DbAccess;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BeestjeFeestje.Migrations
{
    [DbContext(typeof(MyAnimalDbContext))]
    [Migration("20250124151809_First")]
    partial class First
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("MyDomain.Animal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Image")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id");

                    b.ToTable("Animals");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Image = "https://img.freepik.com/free-photo/cute-long-tailed-macaque-eating-fruits-mauritius_181624-47645.jpg?semt=ais_hybrid",
                            Name = "Aap",
                            Price = 1000,
                            Type = "Jungle"
                        },
                        new
                        {
                            Id = 2,
                            Image = "https://img.freepik.com/free-photo/beautiful-shot-african-elephant-savanna-field_181624-13908.jpg?semt=ais_hybrid",
                            Name = "Olifant",
                            Price = 1500,
                            Type = "Jungle"
                        },
                        new
                        {
                            Id = 3,
                            Image = "https://img.freepik.com/free-photo/beautiful-baby-zebra-sitting-ground-captured-african-jungle_181624-35400.jpg?semt=ais_hybrid",
                            Name = "Zebra",
                            Price = 2000,
                            Type = "Jungle"
                        },
                        new
                        {
                            Id = 4,
                            Image = "https://img.freepik.com/free-photo/female-lion-animal-orphanage-kenya_181624-47473.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid",
                            Name = "Leeuw",
                            Price = 2500,
                            Type = "Jungle"
                        },
                        new
                        {
                            Id = 5,
                            Image = "https://img.freepik.com/free-photo/closeup-shot-black-labrador-playing-grass-surrounded-by-greenery_181624-24383.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid",
                            Name = "Hond",
                            Price = 500,
                            Type = "Boerderij"
                        },
                        new
                        {
                            Id = 6,
                            Image = "https://img.freepik.com/free-photo/very-sweet-solitary-burro-standing-grass-meadow_493961-1064.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid",
                            Name = "Ezel",
                            Price = 750,
                            Type = "Boerderij"
                        },
                        new
                        {
                            Id = 7,
                            Image = "https://img.freepik.com/free-photo/cows-standing-green-field-front-fuji-mountain-japan_335224-197.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid",
                            Name = "Koe",
                            Price = 1000,
                            Type = "Boerderij"
                        },
                        new
                        {
                            Id = 8,
                            Image = "https://img.freepik.com/premium-photo/duck-drake-green-grass_167689-1415.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid",
                            Name = "Eend",
                            Price = 1250,
                            Type = "Boerderij"
                        },
                        new
                        {
                            Id = 9,
                            Image = "https://img.freepik.com/premium-photo/newborn-little-cute-yellow-duckling-isolated-white_174533-1086.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid",
                            Name = "Kuiken",
                            Price = 1500,
                            Type = "Boerderij"
                        },
                        new
                        {
                            Id = 10,
                            Image = "https://img.freepik.com/free-photo/penguin-walking-frozen-beach_181624-50490.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid",
                            Name = "Pinguïn",
                            Price = 750,
                            Type = "Sneeuw"
                        },
                        new
                        {
                            Id = 11,
                            Image = "https://img.freepik.com/premium-photo/polar-bear-ursus-maritimus-cub-pack-ice_160321-4130.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid",
                            Name = "IJsbeer",
                            Price = 1000,
                            Type = "Sneeuw"
                        },
                        new
                        {
                            Id = 12,
                            Image = "https://img.freepik.com/free-photo/seal-beach-dune-island-near-helgoland_475641-186.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid",
                            Name = "Zeehond",
                            Price = 1250,
                            Type = "Sneeuw"
                        },
                        new
                        {
                            Id = 13,
                            Image = "https://img.freepik.com/free-photo/dromedary-portrait_463209-91.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid",
                            Name = "Kameel",
                            Price = 750,
                            Type = "Woestijn"
                        },
                        new
                        {
                            Id = 14,
                            Image = "https://img.freepik.com/free-photo/green-viper-snake-branch_488145-411.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid",
                            Name = "Slang",
                            Price = 1000,
                            Type = "Woestijn"
                        },
                        new
                        {
                            Id = 15,
                            Image = "https://img.freepik.com/premium-photo/tyrannosaurus-t-rex-dinosaur-smoke-background_44074-5290.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid",
                            Name = "T-Rex",
                            Price = 5000,
                            Type = "VIP"
                        },
                        new
                        {
                            Id = 16,
                            Image = "https://img.freepik.com/free-photo/photorealistic-unicorn-creature_23-2151581099.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid",
                            Name = "Unicorn",
                            Price = 10000,
                            Type = "VIP"
                        });
                });

            modelBuilder.Entity("MyDomain.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CustomerCard")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("MyDomain.Booking", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("BookedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Bookings");
                });

            modelBuilder.Entity("MyDomain.BookingAnimal", b =>
                {
                    b.Property<int>("BookingId")
                        .HasColumnType("int");

                    b.Property<int>("AnimalId")
                        .HasColumnType("int");

                    b.HasKey("BookingId", "AnimalId");

                    b.HasIndex("AnimalId");

                    b.ToTable("BookingAnimals");
                });

            modelBuilder.Entity("MyDomain.ContactInformation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("BookingId")
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("BookingId")
                        .IsUnique();

                    b.ToTable("Contacts");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("MyDomain.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("MyDomain.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyDomain.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("MyDomain.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyDomain.Booking", b =>
                {
                    b.HasOne("MyDomain.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("MyDomain.BookingAnimal", b =>
                {
                    b.HasOne("MyDomain.Animal", "Animal")
                        .WithMany("BookingAnimals")
                        .HasForeignKey("AnimalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MyDomain.Booking", "Booking")
                        .WithMany("BookingAnimals")
                        .HasForeignKey("BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Animal");

                    b.Navigation("Booking");
                });

            modelBuilder.Entity("MyDomain.ContactInformation", b =>
                {
                    b.HasOne("MyDomain.Booking", null)
                        .WithOne("ContactInformation")
                        .HasForeignKey("MyDomain.ContactInformation", "BookingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("MyDomain.Animal", b =>
                {
                    b.Navigation("BookingAnimals");
                });

            modelBuilder.Entity("MyDomain.Booking", b =>
                {
                    b.Navigation("BookingAnimals");

                    b.Navigation("ContactInformation")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
