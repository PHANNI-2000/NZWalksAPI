using Microsoft.EntityFrameworkCore;
using NZWalksAPI.Model.Domain;
using System;
using System.Collections.Generic;

namespace NZWalksAPI.Data
{
    public class NzWalksDbContext : DbContext
    {
        public NzWalksDbContext(DbContextOptions<NzWalksDbContext> dbContextOptions) : base(dbContextOptions) { }

        public DbSet<Difficulty> Difficulties { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<Walk> Walks { get; set; }
        public DbSet<Image> Images { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Seed data for Difficulties
            // Easy, Mediem, Hard

            var difficulties = new List<Difficulty>()
            {
                new Difficulty()
                {
                    Id = Guid.Parse("f21f9dfc-85e2-43c4-9b8f-5217cbae4f17"),
                    Name = "Easy",
                },
                new Difficulty()
                {
                    Id = Guid.Parse("46e8b6ca-d71e-40b1-bc06-5dcf42d3628e"),
                    Name = "Medium",
                },
                new Difficulty()
                {
                    Id = Guid.Parse("87eb3384-d005-45f9-b490-0ee1ad1ce9f9"),
                    Name = "Hard",
                },
            };
            // Seed difficulties to the database
            modelBuilder.Entity<Difficulty>().HasData(difficulties);

            // Seed data for Regions
            var regions = new List<Region>()
            {
                new Region()
                {
                    Id = Guid.Parse("360bfbd7-8181-4ea1-bb16-44b0bd2ffd62"),
                    Name = "Auckland",
                    Code = "AKL",
                    RegionImageUrl = "https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                 new Region()
                {
                    Id = Guid.Parse("8823b1ea-6f48-47b5-ac6f-4b463fa65dec"),
                    Name = "Bay Of Plenty",
                    Code = "BOP",
                    RegionImageUrl = null
                },
                  new Region()
                {
                    Id = Guid.Parse("6650b5c4-10b0-48ee-bdf2-c7e4fa0ad6c2"),
                    Name = "Wellington",
                    Code = "WGN",
                    RegionImageUrl = "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                   new Region()
                {
                    Id = Guid.Parse("f84f448f-357d-43e2-9f37-d8170021ac27"),
                    Name = "Nelson",
                    Code = "NSN",
                    RegionImageUrl = "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1"
                },
                    new Region()
                {
                    Id = Guid.Parse("0f35754f-c4d7-4d1c-afce-5f0b88d55393"),
                    Name = "Southland",
                    Code = "STL",
                    RegionImageUrl = null
                },
            };

            modelBuilder.Entity<Region>().HasData(regions);
        }
    }
}