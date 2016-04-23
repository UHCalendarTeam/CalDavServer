﻿using DataLayer.Models.ACL;
using DataLayer.Models.Entities;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Scaffolding.Internal.Configuration;
using Microsoft.Extensions.Configuration;

namespace DataLayer
{
    public class CalDavContext : DbContext
    {
        public readonly IConfigurationRoot Configuration;

      



        public CalDavContext(DbContextOptions options)
            : base(options)
        {
           
        }
        /// <summary>
        /// inject the configuration of the app to be accessible in the DataLayer
        /// </summary>
        /// <param name="config"></param>
        public CalDavContext()
        {
            
        }

        public DbSet<User> Users { get; set; }

        public DbSet<Student> Students { get; set; }

        public DbSet<Worker> Workers { get; set; }
        public DbSet<CalendarCollection> CalendarCollections { get; set; }

        public DbSet<CalendarResource> CalendarResources { get; set; }

        public DbSet<Principal> Principals { get; set; }
        public DbSet<Property> CollectionProperties { get; set; }

        public DbSet<Property> ResourceProperties { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            var connection = @"Server=(localdb)\mssqllocaldb;Database=UHCalendarDB;Trusted_Connection=True;";
            optionBuilder.UseSqlServer(connection);
            //optionBuilder.UseInMemoryDatabase();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Principal>()
                .HasOne(p=>p.User)
                .WithOne(u=>u.Principal)
                .HasForeignKey<User>(u=>u.PrincipalId);

            modelBuilder.Entity<CalendarCollection>()
                .HasOne(u => u.Principal)
                .WithMany(cl=>cl.CalendarCollections)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<CalendarResource>()
                .HasOne(cl => cl.CalendarCollection)
                .WithMany(u => u.CalendarResources)
                .HasForeignKey(k => k.CalendarCollectionId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Property>()
                .HasOne(c => c.Collection)
                .WithMany(p => p.Properties)
                .HasForeignKey(k => k.CollectionId);


            modelBuilder.Entity<Property>()
                .HasOne(r => r.Resource)
                .WithMany(p => p.Properties)
                .HasForeignKey(k => k.ResourceId);

        }
    }
}