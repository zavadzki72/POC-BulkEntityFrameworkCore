﻿// <auto-generated />
using System;
using Bulk.App.Infra;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Bulk.App.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20230126005512_RemoveUniqueConstraint")]
    partial class RemoveUniqueConstraint
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Bulk.App.Models.Entities.StadiumEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Capacity")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("stadium", (string)null);
                });

            modelBuilder.Entity("Bulk.App.Models.Entities.TeamEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("Founded_At")
                        .HasColumnType("datetime2");

                    b.Property<string>("Initials")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("nvarchar(5)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("StadiumId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StadiumId")
                        .IsUnique();

                    b.ToTable("team", (string)null);
                });

            modelBuilder.Entity("Bulk.App.Models.Entities.TeamEntity", b =>
                {
                    b.HasOne("Bulk.App.Models.Entities.StadiumEntity", "Stadium")
                        .WithOne("Team")
                        .HasForeignKey("Bulk.App.Models.Entities.TeamEntity", "StadiumId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Stadium");
                });

            modelBuilder.Entity("Bulk.App.Models.Entities.StadiumEntity", b =>
                {
                    b.Navigation("Team")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}