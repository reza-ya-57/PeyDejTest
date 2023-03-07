using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PeyDej.Models.Bases;

namespace PeyDej.Data;

public partial class PeyDejContext : DbContext
{
    public PeyDejContext(DbContextOptions<PeyDejContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SparePart> SpareParts { get; set; }

    // protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //     => optionsBuilder.UseSqlServer(
    //         "Data Source=45.159.149.246,8498;Initial Catalog=PeyDej;User ID=ABC@AriaGroup;Password=ArIaGrOuP@2181811418302132;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.UseCollation("Persian_100_CI_AI_WS_SC_UTF8");

        modelBuilder.Entity<SparePart>(entity =>
        {
            entity.ToTable("SparePart", "Base");

            entity.Property(e => e.Count).HasComment("تعداد");
            entity.Property(e => e.Description)
                .HasMaxLength(2048)
                .HasComment("توضیحات");
            entity.Property(e => e.Emplacement)
                .HasMaxLength(1024)
                .HasComment("محل نصب");
            entity.Property(e => e.GeneralStatusId).HasDefaultValueSql("((1))");
            entity.Property(e => e.InsDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Model)
                .HasMaxLength(1024)
                .HasComment("مدل");
            entity.Property(e => e.Name)
                .HasMaxLength(1024)
                .HasComment("اسم");
        });
        modelBuilder.HasSequence<int>("InspectionSubCategory_Counter", "Base");
        modelBuilder.HasSequence<int>("InspectionSubCategory_Counter");

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}