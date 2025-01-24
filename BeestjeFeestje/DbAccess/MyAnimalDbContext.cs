using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MyDomain;

namespace BeestjeFeestje.DbAccess
{
    public class MyAnimalDbContext : IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public MyAnimalDbContext(DbContextOptions<MyAnimalDbContext> options)
            : base(options)
        {
        }
        public MyAnimalDbContext() { }

        public virtual DbSet<Animal> Animals { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public DbSet<ContactInformation> Contacts { get; set; }
        public DbSet<BookingAnimal> BookingAnimals { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Animal>()
                .Property(a => a.Name)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Animal>()
                .Property(a => a.Type)
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<Animal>()
                .Property(a => a.Price)
                .IsRequired();

            modelBuilder.Entity<Booking>()
                .Property(b => b.UserId);

            modelBuilder.Entity<ContactInformation>()
                .Property(c => c.BookingId)
                .IsRequired();

            modelBuilder.Entity<BookingAnimal>()
                .HasKey(ba => new { ba.BookingId, ba.AnimalId });

            modelBuilder.Entity<BookingAnimal>()
                .HasOne(ba => ba.Booking)
                .WithMany(b => b.BookingAnimals)
                .HasForeignKey(ba => ba.BookingId);

            modelBuilder.Entity<BookingAnimal>()
                .HasOne(ba => ba.Animal)
                .WithMany(a => a.BookingAnimals)
                .HasForeignKey(ba => ba.AnimalId);

            // Seed data for animals
            modelBuilder.Entity<Animal>().HasData(
                new Animal { Id = 1, Name = "Aap", Type = "Jungle", Price = 1000, Image = "https://image.parool.nl/131071330/width/2480/de-makaak-naruto-nam-honderden-foto-s-van-zichzelf-en-zijn-g" },
                new Animal { Id = 2, Name = "Olifant", Type = "Jungle", Price = 1500, Image = "link" },
                new Animal { Id = 3, Name = "Zebra", Type = "Jungle", Price = 2000, Image = "link" },
                new Animal { Id = 4, Name = "Leeuw", Type = "Jungle", Price = 2500, Image = "link" },
                new Animal { Id = 5, Name = "Hond", Type = "Boerderij", Price = 500, Image = "link" },
                new Animal { Id = 6, Name = "Ezel", Type = "Boerderij", Price = 750, Image = "link" },
                new Animal { Id = 7, Name = "Koe", Type = "Boerderij", Price = 1000, Image = "link" },
                new Animal { Id = 8, Name = "Eend", Type = "Boerderij", Price = 1250, Image = "link" },
                new Animal { Id = 9, Name = "Kuiken", Type = "Boerderij", Price = 1500, Image = "link" },
                new Animal { Id = 10, Name = "Pinguïn", Type = "Sneeuw", Price = 750, Image = "link" },
                new Animal { Id = 11, Name = "IJsbeer", Type = "Sneeuw", Price = 1000, Image = "link" },
                new Animal { Id = 12, Name = "Zeehond", Type = "Sneeuw", Price = 1250, Image = "link" },
                new Animal { Id = 13, Name = "Kameel", Type = "Woestijn", Price = 750, Image = "link" },
                new Animal { Id = 14, Name = "Slang", Type = "Woestijn", Price = 1000, Image = "link" },
                new Animal { Id = 15, Name = "T-Rex", Type = "VIP", Price = 5000, Image = "link" },
                new Animal { Id = 16, Name = "Unicorn", Type = "VIP", Price = 10000, Image = "link" }
            );
        }
    }
}
