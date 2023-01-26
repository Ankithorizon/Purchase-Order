using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace PartTracking.Context.Models.Models
{
    public partial class PartMgtContext : DbContext
    {
        public PartMgtContext()
        {
        }

        public PartMgtContext(DbContextOptions<PartMgtContext> options)
            : base(options)
        {
        }

        public virtual DbSet<CustomerOrder> CustomerOrder { get; set; }
        public virtual DbSet<OrderMaster> OrderMaster { get; set; }
        public virtual DbSet<PartDetail> PartDetail { get; set; }
        public virtual DbSet<PartMaster> PartMaster { get; set; }
        public virtual DbSet<PartWorkOrder> PartWorkOrder { get; set; }
        public virtual DbSet<ReceivePart> ReceivePart { get; set; }
        public virtual DbSet<WorkOrder> WorkOrder { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=CHICAAMBICA\\SQLExpress;Database=PartMgt;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomerOrder>(entity =>
            {
                entity.Property(e => e.ProductDrawing).HasMaxLength(500);

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<OrderMaster>(entity =>
            {
                entity.Property(e => e.OrderDate).HasColumnType("datetime");

                entity.Property(e => e.RefCode).HasMaxLength(6);

                entity.HasOne(d => d.PartMaster)
                    .WithMany(p => p.OrderMaster)
                    .HasForeignKey(d => d.PartMasterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PartMaster_OrderMaster");
            });

            modelBuilder.Entity<PartDetail>(entity =>
            {
                entity.HasIndex(e => e.PartMasterId)
                    .HasName("UQ__PartDeta__75828412E41B0673")
                    .IsUnique();

                entity.Property(e => e.PartDesc).HasMaxLength(1000);

                entity.Property(e => e.PartDrgFile).HasMaxLength(1000);

                entity.HasOne(d => d.PartMaster)
                    .WithOne(p => p.PartDetail)
                    .HasForeignKey<PartDetail>(d => d.PartMasterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_PartMaster_PartDetail");
            });

            modelBuilder.Entity<PartMaster>(entity =>
            {
                entity.HasIndex(e => e.PartCode)
                    .HasName("PartCode_unique")
                    .IsUnique();

                entity.Property(e => e.PartCode)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.PartName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<PartWorkOrder>(entity =>
            {
                entity.Property(e => e.PulledDate).HasColumnType("datetime");

                entity.HasOne(d => d.Part)
                    .WithMany(p => p.PartWorkOrder)
                    .HasForeignKey(d => d.PartId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PartWorkO__PartI__35BCFE0A");
            });

            modelBuilder.Entity<ReceivePart>(entity =>
            {
                entity.Property(e => e.ReceiveDate).HasColumnType("datetime");

                entity.HasOne(d => d.OrderMaster)
                    .WithMany(p => p.ReceivePart)
                    .HasForeignKey(d => d.OrderMasterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReceivePa__Order__1BFD2C07");

                entity.HasOne(d => d.PartMaster)
                    .WithMany(p => p.ReceivePart)
                    .HasForeignKey(d => d.PartMasterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__ReceivePa__PartM__1CF15040");
            });

            modelBuilder.Entity<WorkOrder>(entity =>
            {
                entity.HasKey(e => e.Woid)
                    .HasName("PK__WorkOrde__8D75741C5700F0A4");

                entity.Property(e => e.Woid).HasColumnName("WOId");

                entity.HasOne(d => d.CustomerOrder)
                    .WithMany(p => p.WorkOrder)
                    .HasForeignKey(d => d.CustomerOrderId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__WorkOrder__Custo__2F10007B");

                entity.HasOne(d => d.PartMaster)
                    .WithMany(p => p.WorkOrder)
                    .HasForeignKey(d => d.PartMasterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__WorkOrder__PartM__300424B4");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
