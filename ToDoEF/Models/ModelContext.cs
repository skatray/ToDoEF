using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;


namespace ToDoEF.Models
{
    public class ModelContext : DbContext
    {
      
        public ModelContext(DbContextOptions<ModelContext> options)
            : base(options)
        {
           
            Database.EnsureCreated();
        }

     /*   protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
        */
        public virtual DbSet<Tablelist> TablelistSet { get; set; }
        public virtual DbSet<Tablegroup> TablegroupSet { get; set; }
        public virtual DbSet<Tablelistgroup> TablelistTablegroup { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Tablelistgroup>()
                .HasKey(bc => new { bc.TablelistId, bc.TablegroupId });

            modelBuilder.Entity<Tablelistgroup>()
                .HasOne(bc => bc.Tablelist)
                .WithMany(b => b.Tablelistgroups)
                .HasForeignKey(bc => bc.TablelistId);

            modelBuilder.Entity<Tablelistgroup>()
                .HasOne(bc => bc.Tablegroup)
                .WithMany(c => c.Tablelistgroups)
                .HasForeignKey(bc => bc.TablegroupId);
/*
            modelBuilder.Entity<Tablelist>().ToTable("Tablelistset");
            modelBuilder.Entity<Tablelistgroup>().ToTable("Tablelistgroupset");
            modelBuilder.Entity<Tablelist>().ToTable("TablelistTablegroup");
            */
        }

    }
    public class NameViewModel
    {
        public Tablelist Tablelist { get; set; }
        public Tablegroup Tablegroup { get; set; }
        public Tablelistgroup  Tablelistgroup { get; set; }
    }
}

