using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using VisualizationSystemDatabaseEditor.Database.Models;

namespace VisualizationSystemDatabaseEditor.Database;

public sealed partial class VisualizationSystemContext : DbContext
{
    public VisualizationSystemContext()
    {
        Database.EnsureCreated();
    }

    public DbSet<Dashboard> Dashboards { get; set; }

    public DbSet<DataSource> DataSources { get; set; }

    public DbSet<DataSourceType> DataSourceTypes { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<User> Users { get; set; }

    public DbSet<Visualization> Visualizations { get; set; }

    public DbSet<VisualizationType> VisualizationTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        const string connection = "Server=localhost;Database=VisualizationSystemDatabase;Trusted_Connection=True;TrustServerCertificate=True;";
        optionsBuilder.UseLazyLoadingProxies()
            .UseSqlServer(connection);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Dashboard>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Dashboard__3214EC27459C7F10");

            entity.ToTable("Dashboard");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AnalystId).HasColumnName("AnalystID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Analyst).WithMany(p => p.Dashboards)
                .HasForeignKey(d => d.AnalystId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Dashboard_Analyst");

            entity.HasMany(d => d.Visualizations).WithMany(p => p.Dashboards)
                .UsingEntity<Dictionary<string, object>>(
                    "DashboardVisualization",
                    r => r.HasOne<Visualization>().WithMany()
                        .HasForeignKey("VisualizationId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DashboardVisualization_Visualization"),
                    l => l.HasOne<Dashboard>().WithMany()
                        .HasForeignKey("DashboardId")
                        .OnDelete(DeleteBehavior.ClientSetNull)
                        .HasConstraintName("FK_DashboardVisualization_Dashboard"),
                    j =>
                    {
                        j.HasKey("DashboardId", "VisualizationId").HasName("PK__Dashboar__B18992B97AFF9778");
                        j.ToTable("DashboardVisualization");
                        j.IndexerProperty<int>("DashboardId").HasColumnName("DashboardID");
                        j.IndexerProperty<int>("VisualizationId").HasColumnName("VisualizationID");
                    });
        });

        modelBuilder.Entity<DataSource>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DataSour__3214EC27DD606354");

            entity.ToTable("DataSource");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Location)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.SecurityCredentials)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.TypeId).HasColumnName("TypeID");

            entity.HasOne(d => d.Type).WithMany(p => p.DataSources)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_DataSource_Type");
        });

        modelBuilder.Entity<DataSourceType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__DataSour__3214EC270E2E82D8");

            entity.ToTable("DataSourceType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Role__3214EC274C76FC20");

            entity.ToTable("Role");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__User__3214EC2734C97CFD");

            entity.ToTable("User");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PhoneNumber)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.RoleId).HasColumnName("RoleID");

            entity.HasOne(d => d.Role).WithMany(p => p.Users)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_Role");
        });

        modelBuilder.Entity<Visualization>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Visualiz__3214EC27886696F0");

            entity.ToTable("Visualization");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.AnalystId).HasColumnName("AnalystID");
            entity.Property(e => e.DataSourceId).HasColumnName("DataSourceID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Parameters).IsUnicode(false);
            entity.Property(e => e.TypeId).HasColumnName("TypeID");
            entity.Property(e => e.VisualizationSettings).IsUnicode(false);

            entity.HasOne(d => d.Analyst).WithMany(p => p.Visualizations)
                .HasForeignKey(d => d.AnalystId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Visualization_Analyst");

            entity.HasOne(d => d.DataSource).WithMany(p => p.Visualizations)
                .HasForeignKey(d => d.DataSourceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Visualization_DataSource");

            entity.HasOne(d => d.Type).WithMany(p => p.Visualizations)
                .HasForeignKey(d => d.TypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Visualization_Type");
        });

        modelBuilder.Entity<VisualizationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Visualiz__3214EC27EC9CCF16");

            entity.ToTable("VisualizationType");

            entity.Property(e => e.Id).HasColumnName("ID");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
