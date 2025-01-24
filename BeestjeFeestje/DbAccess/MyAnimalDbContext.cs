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
                new Animal { Id = 1, Name = "Aap", Type = "Jungle", Price = 1000, Image = "https://img.freepik.com/free-photo/cute-long-tailed-macaque-eating-fruits-mauritius_181624-47645.jpg?semt=ais_hybrid" },
                new Animal { Id = 2, Name = "Olifant", Type = "Jungle", Price = 1500, Image = "https://img.freepik.com/free-photo/beautiful-shot-african-elephant-savanna-field_181624-13908.jpg?semt=ais_hybrid" },
                new Animal { Id = 3, Name = "Zebra", Type = "Jungle", Price = 2000, Image = "https://img.freepik.com/free-photo/beautiful-baby-zebra-sitting-ground-captured-african-jungle_181624-35400.jpg?semt=ais_hybrid" },
                new Animal { Id = 4, Name = "Leeuw", Type = "Jungle", Price = 2500, Image = "https://img.freepik.com/free-photo/female-lion-animal-orphanage-kenya_181624-47473.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid" },
                new Animal { Id = 5, Name = "Hond", Type = "Boerderij", Price = 500, Image = "https://img.freepik.com/free-photo/closeup-shot-black-labrador-playing-grass-surrounded-by-greenery_181624-24383.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid" },
                new Animal { Id = 6, Name = "Ezel", Type = "Boerderij", Price = 750, Image = "https://img.freepik.com/free-photo/very-sweet-solitary-burro-standing-grass-meadow_493961-1064.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid" },
                new Animal { Id = 7, Name = "Koe", Type = "Boerderij", Price = 1000, Image = "https://img.freepik.com/free-photo/cows-standing-green-field-front-fuji-mountain-japan_335224-197.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid" },
                new Animal { Id = 8, Name = "Eend", Type = "Boerderij", Price = 1250, Image = "https://img.freepik.com/premium-photo/duck-drake-green-grass_167689-1415.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid" },
                new Animal { Id = 9, Name = "Kuiken", Type = "Boerderij", Price = 1500, Image = "https://img.freepik.com/premium-photo/newborn-little-cute-yellow-duckling-isolated-white_174533-1086.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid" },
                new Animal { Id = 10, Name = "Pinguïn", Type = "Sneeuw", Price = 750, Image = "https://img.freepik.com/free-photo/penguin-walking-frozen-beach_181624-50490.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid" },
                new Animal { Id = 11, Name = "IJsbeer", Type = "Sneeuw", Price = 1000, Image = "https://img.freepik.com/premium-photo/polar-bear-ursus-maritimus-cub-pack-ice_160321-4130.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid" },
                new Animal { Id = 12, Name = "Zeehond", Type = "Sneeuw", Price = 1250, Image = "https://img.freepik.com/free-photo/seal-beach-dune-island-near-helgoland_475641-186.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid" },
                new Animal { Id = 13, Name = "Kameel", Type = "Woestijn", Price = 750, Image = "https://img.freepik.com/free-photo/dromedary-portrait_463209-91.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid" },
                new Animal { Id = 14, Name = "Slang", Type = "Woestijn", Price = 1000, Image = "https://img.freepik.com/free-photo/green-viper-snake-branch_488145-411.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid" },
                new Animal { Id = 15, Name = "T-Rex", Type = "VIP", Price = 5000, Image = "https://img.freepik.com/premium-photo/tyrannosaurus-t-rex-dinosaur-smoke-background_44074-5290.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid" },
                new Animal { Id = 16, Name = "Unicorn", Type = "VIP", Price = 10000, Image = "https://img.freepik.com/free-photo/photorealistic-unicorn-creature_23-2151581099.jpg?ga=GA1.1.1551102030.1737730470&semt=ais_hybrid" }
            );
        }
    }
}
