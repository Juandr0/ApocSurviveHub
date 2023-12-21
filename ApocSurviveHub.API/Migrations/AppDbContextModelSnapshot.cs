﻿// <auto-generated />
using System;
using ApocSurviveHub.API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ApocSurviveHub.API.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.0");

            modelBuilder.Entity("ApocSurviveHub.API.Models.Coordinates", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("Latitude")
                        .HasColumnType("REAL");

                    b.Property<double>("Longitude")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("Coordinates");
                });

            modelBuilder.Entity("ApocSurviveHub.API.Models.Horde", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("LocationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ThreatLevel")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Hordes");
                });

            modelBuilder.Entity("ApocSurviveHub.API.Models.Item", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int?>("SurvivorId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("SurvivorId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("ApocSurviveHub.API.Models.Location", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CoordinatesId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("CoordinatesId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("ApocSurviveHub.API.Models.Survivor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsAlive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("LocationId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("LocationId");

                    b.ToTable("Survivors");
                });

            modelBuilder.Entity("ApocSurviveHub.API.Models.Horde", b =>
                {
                    b.HasOne("ApocSurviveHub.API.Models.Location", null)
                        .WithMany("Hordes")
                        .HasForeignKey("LocationId");
                });

            modelBuilder.Entity("ApocSurviveHub.API.Models.Item", b =>
                {
                    b.HasOne("ApocSurviveHub.API.Models.Survivor", null)
                        .WithMany("Inventory")
                        .HasForeignKey("SurvivorId");
                });

            modelBuilder.Entity("ApocSurviveHub.API.Models.Location", b =>
                {
                    b.HasOne("ApocSurviveHub.API.Models.Coordinates", "Coordinates")
                        .WithMany()
                        .HasForeignKey("CoordinatesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Coordinates");
                });

            modelBuilder.Entity("ApocSurviveHub.API.Models.Survivor", b =>
                {
                    b.HasOne("ApocSurviveHub.API.Models.Location", null)
                        .WithMany("Survivors")
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ApocSurviveHub.API.Models.Location", b =>
                {
                    b.Navigation("Hordes");

                    b.Navigation("Survivors");
                });

            modelBuilder.Entity("ApocSurviveHub.API.Models.Survivor", b =>
                {
                    b.Navigation("Inventory");
                });
#pragma warning restore 612, 618
        }
    }
}
