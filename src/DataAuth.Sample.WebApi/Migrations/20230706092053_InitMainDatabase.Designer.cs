﻿// <auto-generated />
using System;
using DataAuth.Sample.WebApi;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAuth.Sample.WebApi.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20230706092053_InitMainDatabase")]
    partial class InitMainDatabase
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataAuth.Sample.WebApi.Entities.Department", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("ParentDepartmentId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ParentDepartmentId");

                    b.ToTable("Departments");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = "KDO",
                            Name = "Sales"
                        },
                        new
                        {
                            Id = 2,
                            Code = "KDO_CHA",
                            Name = "Store selling",
                            ParentDepartmentId = 1
                        },
                        new
                        {
                            Id = 3,
                            Code = "KDO_NVU",
                            Name = "Sale back office",
                            ParentDepartmentId = 2
                        },
                        new
                        {
                            Id = 4,
                            Code = "KDO_NVU_VTU",
                            Name = "Supplier management",
                            ParentDepartmentId = 3
                        },
                        new
                        {
                            Id = 5,
                            Code = "KDO_NVU_CTU",
                            Name = "Document management",
                            ParentDepartmentId = 3
                        },
                        new
                        {
                            Id = 6,
                            Code = "KDO_CHA_NVB",
                            Name = "Seller",
                            ParentDepartmentId = 2
                        });
                });

            modelBuilder.Entity("DataAuth.Sample.WebApi.Entities.Order", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("CustomerName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OrderNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StoreId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("StoreId");

                    b.ToTable("Orders");
                });

            modelBuilder.Entity("DataAuth.Sample.WebApi.Entities.OrderType", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("OrderTypeGroupId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.HasIndex("OrderTypeGroupId");

                    b.ToTable("OrderTypes");

                    b.HasData(
                        new
                        {
                            Id = new Guid("81c23610-904a-48f3-b0c0-4062d4c5dc15"),
                            Code = "DHCH",
                            Name = "Store retailing",
                            OrderTypeGroupId = new Guid("6edc2d8c-ada9-48ea-b347-72a2832607ed")
                        },
                        new
                        {
                            Id = new Guid("bfd95cac-1c38-4b31-8a49-59521ec84338"),
                            Code = "DHDL",
                            Name = "Agency wholesale",
                            OrderTypeGroupId = new Guid("6edc2d8c-ada9-48ea-b347-72a2832607ed")
                        });
                });

            modelBuilder.Entity("DataAuth.Sample.WebApi.Entities.OrderTypeGroup", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("OrderTypeGroups");

                    b.HasData(
                        new
                        {
                            Id = new Guid("6edc2d8c-ada9-48ea-b347-72a2832607ed"),
                            Code = "BL",
                            Name = "Offline retail"
                        },
                        new
                        {
                            Id = new Guid("f829ca08-1958-42d8-9aa1-812599b5a9de"),
                            Code = "DL",
                            Name = "Offline wholesale"
                        });
                });

            modelBuilder.Entity("DataAuth.Sample.WebApi.Entities.Province", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("RegionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RegionId");

                    b.ToTable("Provinces");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = "HCM",
                            Name = "Ho Chi Minh",
                            RegionId = 3
                        },
                        new
                        {
                            Id = 2,
                            Code = "TNI",
                            Name = "Tay Ninh",
                            RegionId = 3
                        },
                        new
                        {
                            Id = 3,
                            Code = "BDU",
                            Name = "Binh Duong",
                            RegionId = 3
                        },
                        new
                        {
                            Id = 4,
                            Code = "DNI",
                            Name = "Đồng Nai",
                            RegionId = 3
                        });
                });

            modelBuilder.Entity("DataAuth.Sample.WebApi.Entities.Region", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Regions");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = "Northern",
                            Name = "NT"
                        },
                        new
                        {
                            Id = 2,
                            Code = "Central",
                            Name = "CT"
                        },
                        new
                        {
                            Id = 3,
                            Code = "Southern",
                            Name = "ST"
                        });
                });

            modelBuilder.Entity("DataAuth.Sample.WebApi.Entities.Store", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ProvinceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ProvinceId");

                    b.ToTable("Stores");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Code = "HCM_001",
                            Name = "Tran Hung Dao store",
                            ProvinceId = 1
                        },
                        new
                        {
                            Id = 2,
                            Code = "HCM_002",
                            Name = "Nguyen Van Cu store",
                            ProvinceId = 1
                        },
                        new
                        {
                            Id = 3,
                            Code = "HCM_003",
                            Name = "Nguyen Oanh store",
                            ProvinceId = 1
                        },
                        new
                        {
                            Id = 4,
                            Code = "HCM_004",
                            Name = "Phan Dang Luu store",
                            ProvinceId = 1
                        },
                        new
                        {
                            Id = 5,
                            Code = "HCM_005",
                            Name = "Vo Van Viet store",
                            ProvinceId = 1
                        },
                        new
                        {
                            Id = 6,
                            Code = "DNI_001",
                            Name = "Bien Hoa 1 store",
                            ProvinceId = 4
                        },
                        new
                        {
                            Id = 7,
                            Code = "BDU_001",
                            Name = "Di An store",
                            ProvinceId = 3
                        });
                });

            modelBuilder.Entity("DataAuth.Sample.WebApi.Entities.Department", b =>
                {
                    b.HasOne("DataAuth.Sample.WebApi.Entities.Department", "ParentDepartment")
                        .WithMany()
                        .HasForeignKey("ParentDepartmentId");

                    b.Navigation("ParentDepartment");
                });

            modelBuilder.Entity("DataAuth.Sample.WebApi.Entities.Order", b =>
                {
                    b.HasOne("DataAuth.Sample.WebApi.Entities.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId");

                    b.Navigation("Store");
                });

            modelBuilder.Entity("DataAuth.Sample.WebApi.Entities.OrderType", b =>
                {
                    b.HasOne("DataAuth.Sample.WebApi.Entities.OrderTypeGroup", "Group")
                        .WithMany()
                        .HasForeignKey("OrderTypeGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("DataAuth.Sample.WebApi.Entities.Province", b =>
                {
                    b.HasOne("DataAuth.Sample.WebApi.Entities.Region", "Region")
                        .WithMany()
                        .HasForeignKey("RegionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Region");
                });

            modelBuilder.Entity("DataAuth.Sample.WebApi.Entities.Store", b =>
                {
                    b.HasOne("DataAuth.Sample.WebApi.Entities.Province", "Province")
                        .WithMany()
                        .HasForeignKey("ProvinceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Province");
                });
#pragma warning restore 612, 618
        }
    }
}
