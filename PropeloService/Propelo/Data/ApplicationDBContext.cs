using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Propelo.Models;

namespace Propelo.Data
{
    public class ApplicationDBContext : IdentityDbContext<User>
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {
        }
        public DbSet<Promoter> Promoters { get; set; }
        public DbSet<PromoterPicture> PromoterPictures { get; set; }
        public DbSet<Property> Properties { get; set; }
        public DbSet<PropertyPicture> PropertyPictures { get; set; }
        public DbSet<Apartment> Apartments { get; set; }
        public DbSet<ApartmentPicture> ApartmentPictures { get; set; }
        public DbSet<ApartmentDocument> ApartmentDocuments { get; set; }
        public DbSet<Area> Areas { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Setting> Settings { get; set; }
        public DbSet<Logo> Logos { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>().Property(u => u.Initials).HasMaxLength(5);
            modelBuilder.HasDefaultSchema("Propelo"); 
                

            //Promoter relationships
            modelBuilder.Entity<Promoter>()
                .HasMany(p=>p.properties)
                .WithOne(p=>p.Promoter)
                .HasForeignKey(p=>p.PromoterID)
                .HasPrincipalKey(p=>p.Id);

            modelBuilder.Entity<Promoter>()
                .HasOne(p=>p.Picture)
                .WithOne(p=>p.Promoter)
                .HasForeignKey<PromoterPicture>(p=>p.PromoterId)
                .HasPrincipalKey<Promoter>(p=>p.Id);

            //Property relationships
            modelBuilder.Entity<Property>()
                .HasMany(p=>p.Apartments)
                .WithOne(p=>p.Property)
                .HasForeignKey(p=>p.PropertyId)
                .HasPrincipalKey(p=>p.Id);

            modelBuilder.Entity<Property>()
                .HasMany(p=>p.PropertyPictures)
                .WithOne(p=>p.Property)
                .HasForeignKey(p=>p.PropertyId)
                .HasPrincipalKey(p=>p.Id);


            //Apartment relationships
            modelBuilder.Entity<Apartment>()
                .HasMany(p=>p.Areas)
                .WithOne(p=>p.Apartment)
                .HasForeignKey(p=>p.ApartmentId)
                .HasPrincipalKey(p=>p.Id);

            modelBuilder.Entity<Apartment>()
                .HasMany(p=>p.ApartmentDocuments)
                .WithOne(p=>p.Apartment)
                .HasForeignKey(p=>p.ApartmentId)
                .HasPrincipalKey(p=>p.Id);

            modelBuilder.Entity<Apartment>()
                .HasMany(p=>p.ApartmentPictures)
                .WithOne(p=>p.Apartment)
                .HasForeignKey(p=>p.ApartmentId)
                .HasPrincipalKey(p=>p.Id);

            modelBuilder.Entity<Apartment>()
                .HasMany(p=>p.Orders)
                .WithOne(p=>p.Apartment)
                .HasForeignKey(p=>p.ApartmentID)
                .HasPrincipalKey(p=>p.Id);

            //Setting relationships
            modelBuilder.Entity<Setting>()
                .HasOne(p=>p.Logo)
                .WithOne(p=>p.Setting)
                .HasForeignKey<Logo>(p=>p.SettingId)
                .HasPrincipalKey<Setting>(p=>p.Id);

        }


    }
    
}
