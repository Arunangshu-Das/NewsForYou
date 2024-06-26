﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace NewsForYou.DAL.Models;

public partial class NewsForYouContext : DbContext
{
    public NewsForYouContext()
    {
    }

    public NewsForYouContext(DbContextOptions<NewsForYouContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Agency> Agencies { get; set; }

    public virtual DbSet<AgencyFeed> AgencyFeeds { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<News> News { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Agency>(entity =>
        {
            entity.HasKey(e => e.AgencyId).HasName("PK__Agency__95C546DB4A5FF416");

            entity.ToTable("Agency");

            entity.Property(e => e.AgencyLogoPath)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.AgencyName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<AgencyFeed>(entity =>
        {
            entity.HasKey(e => e.AgencyFeedId).HasName("PK__AgencyFe__CD7F82BDF1F34190");

            entity.ToTable("AgencyFeed");

            entity.Property(e => e.AgencyFeedUrl)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Agency).WithMany(p => p.AgencyFeeds)
                .HasForeignKey(d => d.AgencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AgencyFee__Agenc__5DCAEF64");

            entity.HasOne(d => d.Category).WithMany(p => p.AgencyFeeds)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__AgencyFee__Categ__5EBF139D");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.CategoryId).HasName("PK__Category__19093A0B62DC6052");

            entity.ToTable("Category");

            entity.Property(e => e.CategoryTitle)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<News>(entity =>
        {
            entity.HasKey(e => e.NewsId).HasName("PK__News__954EBDF3EF546BB4");

            entity.HasIndex(e => e.NewsLink, "UQ__News__4BEBCC126E40277F").IsUnique();

            entity.Property(e => e.ClickCount).HasDefaultValue(0);
            entity.Property(e => e.NewsDescription).HasColumnType("text");
            entity.Property(e => e.NewsLink)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.NewsPublishDateTime).HasColumnType("datetime");
            entity.Property(e => e.NewsTitle)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.Agency).WithMany(p => p.News)
                .HasForeignKey(d => d.AgencyId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__News__AgencyId__6477ECF3");

            entity.HasOne(d => d.Category).WithMany(p => p.News)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__News__CategoryId__6383C8BA");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CC4C290FA7E2");

            entity.ToTable("User");

            entity.HasIndex(e => e.Email, "UQ__User__A9D10534B1136AB3").IsUnique();

            entity.Property(e => e.Email)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Name)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
