using Microsoft.EntityFrameworkCore;
using NetCoreApp.Crosscutting.Helpers;
using NetCoreApp.Domain.Entities;
using System.Collections.Generic;

namespace NetCoreApp.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public virtual DbSet<User> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            BuildUserModel(modelBuilder);
            BuildLogModel(modelBuilder);
            AddInitialDataSets(modelBuilder);
        }

        private static void BuildUserModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .Property(u => u.FirstName)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.LastName)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Username)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.Email)
                .IsRequired();

            modelBuilder.Entity<User>()
                .Property(u => u.PasswordHash)
                .IsRequired();
        }

        private static void BuildLogModel(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Log>()
                .Property(l => l.TimeStamp)
                .IsRequired();
        }

        private static void AddInitialDataSets(ModelBuilder modelBuilder)
        {
            InitialDataUsers(modelBuilder);
        }

        private static void InitialDataUsers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(new List<User>()
            {
                new User()
                {
                    Id = 1,
                    FirstName = "First name test",
                    LastName = "Last name test",
                    Username = "Admin",
                    Email = "Admin@admin.com",
                    PasswordHash = CryptographyHelper.EncryptPassword("1234")
                }
            });
        }

    }
}
