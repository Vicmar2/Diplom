using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Diplom2.Db;

public partial class DiplomContext : DbContext
{
    public DiplomContext()
    {
    }

    public DiplomContext(DbContextOptions<DiplomContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Buket> Bukets { get; set; }

    public virtual DbSet<KrossBuket> KrossBukets { get; set; }

    public virtual DbSet<KrossOrder> KrossOrders { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<Raspisanie> Raspisanies { get; set; }

    public virtual DbSet<Skidka> Skidkas { get; set; }

    public virtual DbSet<Smena> Smenas { get; set; }

    public virtual DbSet<Sotrud> Sotruds { get; set; }

    public virtual DbSet<Status> Statuses { get; set; }

    public virtual DbSet<Tovar> Tovars { get; set; }

    public virtual DbSet<TypeTovar> TypeTovars { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("server=localhost; database=Diplom;TrustServerCertificate=true; Trusted_Connection=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Buket>(entity =>
        {
            entity.HasKey(e => e.IdBuket);

            entity.ToTable("Buket");

            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.ImageBuket).HasColumnType("image");
            entity.Property(e => e.PriceBuket).HasColumnType("decimal(18, 0)");
        });

        modelBuilder.Entity<KrossBuket>(entity =>
        {
            entity.HasKey(e => e.IdKrossBuket);

            entity.ToTable("KrossBuket");

            entity.HasOne(d => d.IdBuketNavigation).WithMany(p => p.KrossBukets)
                .HasForeignKey(d => d.IdBuket)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KrossBuket_Buket1");

            entity.HasOne(d => d.IdTovarNavigation).WithMany(p => p.KrossBukets)
                .HasForeignKey(d => d.IdTovar)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_KrossBuket_Flower");
        });

        modelBuilder.Entity<KrossOrder>(entity =>
        {
            entity.HasKey(e => e.IdKrossOrder);

            entity.ToTable("KrossOrder");

            entity.Property(e => e.IdKrossOrder).ValueGeneratedNever();

            entity.HasOne(d => d.IdBuketNavigation).WithMany(p => p.KrossOrders)
                .HasForeignKey(d => d.IdBuket)
                .HasConstraintName("FK_KrossOrder_Buket");

            entity.HasOne(d => d.IdOrderNavigation).WithMany(p => p.KrossOrders)
                .HasForeignKey(d => d.IdOrder)
                .HasConstraintName("FK_KrossOrder_Order");

            entity.HasOne(d => d.IdTovarNavigation).WithMany(p => p.KrossOrders)
                .HasForeignKey(d => d.IdTovar)
                .HasConstraintName("FK_KrossOrder_Tovar");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.IdOrder).HasName("PK_Orders");

            entity.ToTable("Order");

            entity.Property(e => e.CreatedAt).HasColumnType("datetime");
            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.PriceOrder).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdSkidkaNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdSkidka)
                .HasConstraintName("FK_Order_Skidka");

            entity.HasOne(d => d.IdUserNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.IdUser)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Orders_User");

            entity.HasOne(d => d.StatusOrderNavigation).WithMany(p => p.Orders)
                .HasForeignKey(d => d.StatusOrder)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Order_Status");
        });

        modelBuilder.Entity<Raspisanie>(entity =>
        {
            entity.HasKey(e => e.IdRaspisanie);

            entity.ToTable("Raspisanie");

            entity.Property(e => e.IdRaspisanie).ValueGeneratedNever();

            entity.HasOne(d => d.IdSmenaNavigation).WithMany(p => p.Raspisanies)
                .HasForeignKey(d => d.IdSmena)
                .HasConstraintName("FK_Raspisanie_Smena");

            entity.HasOne(d => d.IdSotrudNavigation).WithMany(p => p.Raspisanies)
                .HasForeignKey(d => d.IdSotrud)
                .HasConstraintName("FK_Raspisanie_Sotrud");
        });

        modelBuilder.Entity<Skidka>(entity =>
        {
            entity.HasKey(e => e.IdSkidka);

            entity.ToTable("Skidka");

            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.NameSkidka)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Smena>(entity =>
        {
            entity.HasKey(e => e.IdSmena);

            entity.ToTable("Smena");

            entity.Property(e => e.IdSmena).ValueGeneratedNever();
            entity.Property(e => e.EndSmena).HasColumnType("datetime");
            entity.Property(e => e.StartSmena).HasColumnType("datetime");
        });

        modelBuilder.Entity<Sotrud>(entity =>
        {
            entity.HasKey(e => e.IdSotrud);

            entity.ToTable("Sotrud");

            entity.Property(e => e.LoginSotrud)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NameSotrud)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ParolSotrud)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Status>(entity =>
        {
            entity.HasKey(e => e.IdStatusOrder);

            entity.ToTable("Status");

            entity.Property(e => e.NameStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Tovar>(entity =>
        {
            entity.HasKey(e => e.IdTovar).HasName("PK_Flower");

            entity.ToTable("Tovar");

            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.ImageTovar).HasColumnType("image");
            entity.Property(e => e.NameTovar)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PriceTovar).HasColumnType("decimal(10, 2)");

            entity.HasOne(d => d.IdTypeTovarNavigation).WithMany(p => p.Tovars)
                .HasForeignKey(d => d.IdTypeTovar)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Tovar_Type");
        });

        modelBuilder.Entity<TypeTovar>(entity =>
        {
            entity.HasKey(e => e.IdTypeTovar).HasName("PK_Type");

            entity.ToTable("TypeTovar");

            entity.Property(e => e.NameType)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.IdUser);

            entity.ToTable("User");

            entity.Property(e => e.DeleteAt).HasColumnType("datetime");
            entity.Property(e => e.EmailUser)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.NameUser)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.ParolUser)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
