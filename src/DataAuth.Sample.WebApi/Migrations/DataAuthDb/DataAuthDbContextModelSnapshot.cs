﻿// <auto-generated />
using System;
using DataAuth;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAuth.Sample.WebApi.Migrations.DataAuthDb
{
    [DbContext(typeof(DataAuthDbContext))]
    partial class DataAuthDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataAuth.Entities.AccessAttribute", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("DataAuth_AccessAttributes");
                });

            modelBuilder.Entity("DataAuth.Entities.AccessAttributeTable", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessAttributeId")
                        .HasColumnType("int");

                    b.Property<string>("Alias")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<int?>("HierarchyLevel")
                        .HasColumnType("int");

                    b.Property<string>("IdColumn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsLeafLevel")
                        .HasColumnType("bit");

                    b.Property<bool>("IsSelfReference")
                        .HasColumnType("bit");

                    b.Property<string>("LocalPermissionIdColumn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LocalPermissionLookupColumn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LocalPermissionTableName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("NameColumn")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ParentColumn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("TableName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccessAttributeId");

                    b.HasIndex("Alias")
                        .IsUnique();

                    b.ToTable("DataAuth_AccessAttributeTables");
                });

            modelBuilder.Entity("DataAuth.Entities.DataPermission", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("AccessAttributeTableId")
                        .HasColumnType("int");

                    b.Property<int>("AccessLevel")
                        .HasColumnType("int");

                    b.Property<int>("GrantType")
                        .HasColumnType("int");

                    b.Property<string>("GrantedDataValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubjectId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("AccessAttributeTableId");

                    b.ToTable("DataAuth_DataPermissions");
                });

            modelBuilder.Entity("DataAuth.Entities.Role", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Code")
                        .IsUnique();

                    b.ToTable("DataAuth_Roles");
                });

            modelBuilder.Entity("DataAuth.Entities.UserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId", "RoleId")
                        .IsUnique();

                    b.ToTable("DataAuth_UserRoles");
                });

            modelBuilder.Entity("DataAuth.Entities.AccessAttributeTable", b =>
                {
                    b.HasOne("DataAuth.Entities.AccessAttribute", "AccessAttribute")
                        .WithMany()
                        .HasForeignKey("AccessAttributeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccessAttribute");
                });

            modelBuilder.Entity("DataAuth.Entities.DataPermission", b =>
                {
                    b.HasOne("DataAuth.Entities.AccessAttributeTable", "AccessAttributeTable")
                        .WithMany()
                        .HasForeignKey("AccessAttributeTableId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccessAttributeTable");
                });

            modelBuilder.Entity("DataAuth.Entities.UserRole", b =>
                {
                    b.HasOne("DataAuth.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });
#pragma warning restore 612, 618
        }
    }
}
