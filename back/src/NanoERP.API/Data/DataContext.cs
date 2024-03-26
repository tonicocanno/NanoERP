using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;
using NanoERP.API.Domain.Entities;

namespace NanoERP.API.Data
{
    public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; init; }
        public DbSet<Partner> Partners { get; init; }
        public DbSet<PartnerAddress> PartnerAddresses { get; init; }
        public DbSet<Product> Products { get; init; }

        public static DataContext Create(IMongoDatabase database) =>
            new(new DbContextOptionsBuilder<DataContext>()
                .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
                .Options);

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().ToCollection("users");
            modelBuilder.Entity<Partner>().ToCollection("partners");
            modelBuilder.Entity<PartnerAddress>().ToCollection("partnerAddresses");
            modelBuilder.Entity<Product>().ToCollection("products");
        }
    }
}