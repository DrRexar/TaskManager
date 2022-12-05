using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TaskManager.Models;

namespace TaskManager.Data
{
    public class ApplicationContext: DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserTask> UsersTasks { get; set; }
        public ApplicationContext()
        {
            //Database.EnsureDeleted();   // удаляем бд со старой схемой
            //Database.EnsureCreated();   // создаем бд с новой схемой
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var dbFile = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "TaskManager.db");
            optionsBuilder.UseSqlite($"Filename={dbFile}");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasIndex(x => x.UserName).IsUnique(true);
            modelBuilder.Entity<User>().HasMany(x => x.Tasks).WithOne(x=>x.User).HasForeignKey(x=>x.UserId);
            modelBuilder.Entity<User>().Navigation(x => x.Tasks).AutoInclude();
            modelBuilder.Entity<UserTask>().Navigation(x => x.User).AutoInclude();
            base.OnModelCreating(modelBuilder);
        }
    }
}
