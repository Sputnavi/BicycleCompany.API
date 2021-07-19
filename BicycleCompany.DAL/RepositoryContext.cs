using BicycleCompany.DAL.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace BicycleCompany.DAL
{
    public class RepositoryContext : DbContext
    { 
        public RepositoryContext(DbContextOptions<RepositoryContext> options)
            : base(options)
        {
            //Database.EnsureCreated();
        }

        public DbSet<Bicycle> Bicycles { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Part> Parts { get; set; }
        public DbSet<Problem> Problems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Guid clientId = new Guid("3B4E22BE-C10D-4303-BF57-03ECA2F13F2B");
            Guid bicycleId = new Guid("0EA19DCD-17FF-4284-BF9D-D9CCF7C15FD6");

            modelBuilder.Entity<Bicycle>()
                .HasData(
                new Bicycle
                {
                    Id = bicycleId,
                    Name = "Stels",
                    Model = "Rocco"
                },
                new Bicycle
                {
                    Id = Guid.NewGuid(),
                    Name = "LTD",
                    Model = "Turbo"
                },
                new Bicycle
                {
                    Id = Guid.NewGuid(),
                    Name = "Aist",
                    Model = "Tango"
                }
            );
            
            modelBuilder.Entity<Client>()
                .HasData(
                new Client
                {
                    Id = clientId,
                    Name = "John Doe"
                },
                new Client
                {
                    Id = Guid.NewGuid(),
                    Name = "Andrew Vertuha"
                }
            );

            modelBuilder.Entity<Problem>()
                .HasData(
                new Problem
                {
                    Id = new Guid("F451E4EB-C5FC-4FF4-A751-57EEE391F9A7"),
                    ClientId = clientId,
                    BicycleId = bicycleId,
                    Place = "Outside the city",
                    Stage = Stage.Received,
                    Date = DateTime.Today,
                    Description = "The seat was broken in half"
                }
            );

            modelBuilder.Entity<Part>()
                .HasData(
                new Part
                {
                    Id = new Guid("8CC08FCB-1FDB-4353-8540-DDE0B1FCCE5B"),
                    Name = "Seat",
                    Amount = 5
                },
                new Part
                {
                    Id = Guid.NewGuid(),
                    Name = "Wheel",
                    Amount = 7
                },
                new Part
                {
                    Id = Guid.NewGuid(),
                    Name = "Handlebar",
                    Amount = 3
                }
            );
        }
    }
}
