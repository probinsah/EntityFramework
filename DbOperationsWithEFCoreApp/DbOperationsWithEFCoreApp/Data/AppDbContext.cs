using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEFCoreApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasOne(u => u.Profile)
                .WithOne(p => p.User)
                .HasForeignKey<UserProfile>(p => p.UserId);

            modelBuilder.Entity<CurrencyType>().HasData(
                new CurrencyType() { Id = 1, Currency = "INR", Description = "Indian Rupee"},
                new CurrencyType() { Id = 2, Currency = "USD", Description = "US Dollar"},
                new CurrencyType() { Id = 3, Currency = "TL", Description = "Turkish Lira"}
                );

            modelBuilder.Entity<Language>().HasData(
                new Language() { Id = 1, Title = "Hindi", Description = "Indian" },
                new Language() { Id = 2, Title = "English", Description = "USA" },
                new Language() { Id = 3, Title = "Turkish", Description = "Turkey" }
                );
        }
        public DbSet<Book> Books { get; set; }
        public DbSet<Language> Languages { get; set; }
        public DbSet<BookPrice> BookPrice { get; set; }
        public DbSet<CurrencyType> CurrencyTypes { get; set; }
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
