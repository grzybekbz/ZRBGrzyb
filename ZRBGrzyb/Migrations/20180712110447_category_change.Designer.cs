﻿// <auto-generated />
using ZRBGrzyb.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace ZRBGrzyb.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20180712110447_category_change")]
    partial class category_change
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.0.1-rtm-125")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Grzyb.Models.Category", b =>
                {
                    b.Property<int>("CategoryID")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("RouteName");

                    b.HasKey("CategoryID");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("Grzyb.Models.Photo", b =>
                {
                    b.Property<int>("PhotoID")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("AddDate");

                    b.Property<int>("CategoryID");

                    b.Property<string>("Description")
                        .IsRequired();

                    b.Property<string>("FileName")
                        .IsRequired();

                    b.HasKey("PhotoID");

                    b.HasIndex("CategoryID");

                    b.ToTable("Photos");
                });

            modelBuilder.Entity("Grzyb.Models.Photo", b =>
                {
                    b.HasOne("Grzyb.Models.Category", "Category")
                        .WithMany()
                        .HasForeignKey("CategoryID")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}
