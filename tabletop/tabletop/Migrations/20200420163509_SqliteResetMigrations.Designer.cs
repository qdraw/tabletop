﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using tabletop.Data;

namespace tabletop.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20200420163509_SqliteResetMigrations")]
    partial class SqliteResetMigrations
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("tabletop.Models.ChannelEvent", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("ChannelUserId")
                        .HasColumnType("varchar(80) CHARACTER SET utf8mb4");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<int>("Weight")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ChannelUserId");

                    b.ToTable("ChannelEvent");
                });

            modelBuilder.Entity("tabletop.Models.ChannelUser", b =>
                {
                    b.Property<string>("NameId")
                        .HasColumnType("varchar(80) CHARACTER SET utf8mb4")
                        .HasMaxLength(80);

                    b.Property<string>("Bearer")
                        .HasColumnType("varchar(100) CHARACTER SET utf8mb4")
                        .HasMaxLength(100);

                    b.Property<bool>("IsAccessible")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("IsVisible")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("varchar(80) CHARACTER SET utf8mb4")
                        .HasMaxLength(80);

                    b.Property<string>("NameUrlSafe")
                        .IsRequired()
                        .HasColumnType("varchar(80) CHARACTER SET utf8mb4")
                        .HasMaxLength(80);

                    b.HasKey("NameId");

                    b.ToTable("ChannelUser");
                });

            modelBuilder.Entity("tabletop.Models.ChannelEvent", b =>
                {
                    b.HasOne("tabletop.Models.ChannelUser", "ChannelUser")
                        .WithMany("ChannelEvents")
                        .HasForeignKey("ChannelUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}