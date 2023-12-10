using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace _08122023adminapproval.Models;

public partial class Role1Context : DbContext
{
    public Role1Context()
    {
    }

    public Role1Context(DbContextOptions<Role1Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Registration> Registrations { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=TPAREPAL-L-5467\\SQLEXPRESS;Initial Catalog=Role1;Persist Security Info=True;User ID=sa;Password=Welcome2evoke@1234; Trust Server Certificate=true");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Registration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Registra__3214EC0739F74F26");

            entity.ToTable("Registration");

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Firstname)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");
            entity.Property(e => e.Lastname)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Phone)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Role).WithMany(p => p.Registrations)
                .HasForeignKey(d => d.Roleid)
                .HasConstraintName("FK__Registrat__Rolei__5BE2A6F2");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Roleid).HasName("PK__Role__8AF5CA323481B71F");

            entity.ToTable("Role");

            entity.Property(e => e.Rolename)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
