using FirstProject.Models;
using Microsoft.EntityFrameworkCore;
namespace FirstProject.Data
{

    public class AddDbContext:DbContext
    {
        public AddDbContext(DbContextOptions<AddDbContext> options) : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Author>()
                .HasIndex(x => x.PersonalNumber)
                .IsUnique(true);

            modelBuilder.Entity<Author>()
              .HasOne(x => x.Sex)
              .WithMany()
              .HasForeignKey(x => x.SexId);

            modelBuilder.Entity<Author>()
                .HasOne(x => x.City)
                .WithMany()
                .HasForeignKey(x => x.CityId);

            modelBuilder.Entity<Author>()
                .HasOne(x=>x.Country)
                .WithMany()
                .HasForeignKey(x => x.CountryId);

            modelBuilder.Entity<Product>()
                .HasIndex(x => x.ISBN)
                .IsUnique(true);

            modelBuilder.Entity<Product>()
                .HasOne(x => x.Product_Type)
                .WithMany()
                .HasForeignKey(x => x.typeId);

            modelBuilder.Entity<Product>()
                .HasOne(x => x.publishing_house)
                .WithMany()
                .HasForeignKey(x => x.publishId);

            modelBuilder.Entity<Author>()
                .HasMany(x => x.Products)
                .WithMany(b => b.Authors)
                .UsingEntity<Dictionary<string, object>>(
                "AuthorProducts",
                    l => l.HasOne<Product>().WithMany().HasForeignKey("ProductId").OnDelete(DeleteBehavior.Cascade),
                    r => r.HasOne<Author>().WithMany().HasForeignKey("AuthorId").OnDelete(DeleteBehavior.Cascade),
                    j =>
                  {
                    j.HasKey("AuthorId", "ProductId");
                    j.ToTable("AuthorProducts");
                    });
        }

        public DbSet<Admin> admin { get; set; }
        public DbSet<City> city { get; set; }
        public DbSet<Country> country { get; set; }
        public DbSet<Author_Sex> author_sex {  get; set; }
        public DbSet<Author> author { get; set; }
        public DbSet<Publishing_house> publishing_houses { get; set; }
        public DbSet<Product_Type> product_types { get; set; }

        public DbSet<Product> product { get; set; }
    }
}
