﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NTR02.Data;

namespace NTR02.Migrations.NoteCategory
{
    [DbContext(typeof(NoteCategoryContext))]
    partial class NoteCategoryContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.1");

            modelBuilder.Entity("NTR02.Models.NoteCategory", b =>
                {
                    b.Property<int>("NoteCategoryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("CategoryID")
                        .HasColumnType("INTEGER");

                    b.Property<int>("NoteID")
                        .HasColumnType("INTEGER");

                    b.HasKey("NoteCategoryID");

                    b.ToTable("NoteCategory");
                });
#pragma warning restore 612, 618
        }
    }
}
