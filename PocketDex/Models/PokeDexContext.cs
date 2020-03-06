using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PocketDex.Models
{
    public partial class PokeDexContext : DbContext
    {
        //public PokeDexContext()
        //{
        //}

        public PokeDexContext(DbContextOptions<PokeDexContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Attack> Attack { get; set; }
        public virtual DbSet<Pokemon> Pokemon { get; set; }
        public virtual DbSet<PokemonAttack> PokemonAttack { get; set; }
        public virtual DbSet<PokemonType> PokemonType { get; set; }
        public virtual DbSet<Region> Region { get; set; }
        public virtual DbSet<Types> Types { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("server=(localdb)\\MSSQLLocalDB;database=PokeDex;Trusted_Connection=True");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Attack>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Pokemon>(entity =>
            {
                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Gender)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Height)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PhotoPath)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Weight)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.HasOne(d => d.Region)
                    .WithMany(p => p.Pokemon)
                    .HasForeignKey(d => d.RegionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Pokemon_Region");
            });

            modelBuilder.Entity<PokemonAttack>(entity =>
            {
                entity.HasOne(d => d.Attack)
                    .WithMany(p => p.PokemonAttack)
                    .HasForeignKey(d => d.AttackId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PokemonAttack_Attack");

                entity.HasOne(d => d.Pokemon)
                    .WithMany(p => p.PokemonAttack)
                    .HasForeignKey(d => d.PokemonId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PokemonAttack_Pokemon");
            });

            modelBuilder.Entity<PokemonType>(entity =>
            {
                //entity.Property(e => e.Id).ValueGeneratedNever();

                //entity.Property(e => e.TypeId).ValueGeneratedOnAdd();

                entity.HasOne(d => d.Pokemon)
                    .WithMany(p => p.PokemonType)
                    .HasForeignKey(d => d.PokemonId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PokemonType_Pokemon");

                entity.HasOne(d => d.Types)
                    .WithMany(p => p.PokemonType)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("FK_PokemonType_Type");
            });

            modelBuilder.Entity<Region>(entity =>
            {
                entity.Property(e => e.ClassType)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });

            modelBuilder.Entity<Types>(entity =>
            {
                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50);
            });
        }
    }
}
