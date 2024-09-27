﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using kit_stem_api.Data;

#nullable disable

namespace kit_stem_api.Migrations
{
    [DbContext(typeof(KitStemDbContext))]
    partial class KitStemDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "db9e2576-9d94-4183-8d6e-2743cc3d5d39",
                            ConcurrencyStamp = "db9e2576-9d94-4183-8d6e-2743cc3d5d39",
                            Name = "admin",
                            NormalizedName = "ADMIN"
                        },
                        new
                        {
                            Id = "9fd1af37-4fee-42e3-9bee-c025577ce705",
                            ConcurrencyStamp = "9fd1af37-4fee-42e3-9bee-c025577ce705",
                            Name = "manager",
                            NormalizedName = "MANAGER"
                        },
                        new
                        {
                            Id = "0da287c4-8e61-42eb-a177-5c9c24890ae2",
                            ConcurrencyStamp = "0da287c4-8e61-42eb-a177-5c9c24890ae2",
                            Name = "staff",
                            NormalizedName = "STAFF"
                        },
                        new
                        {
                            Id = "827dae5f-ef37-4606-a6a0-49721ce0e93e",
                            ConcurrencyStamp = "827dae5f-ef37-4606-a6a0-49721ce0e93e",
                            Name = "customer",
                            NormalizedName = "CUSTOMER"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasMaxLength(45)
                        .HasColumnType("nvarchar(45)");

                    b.Property<string>("LastName")
                        .HasMaxLength(45)
                        .HasColumnType("nvarchar(45)");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<int>("Points")
                        .HasColumnType("int");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.Component", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int>("TypeId")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__Componen__3214EC072B0E2FDE");

                    b.HasIndex("TypeId");

                    b.ToTable("Component");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.ComponentsType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id")
                        .HasName("PK__Componen__3214EC0732D88561");

                    b.ToTable("ComponentsType");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.Kit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Brief")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("PurchaseCost")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id")
                        .HasName("PK__Kit__3214EC071BF8D031");

                    b.HasIndex("CategoryId");

                    b.ToTable("Kit");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.KitComponent", b =>
                {
                    b.Property<int>("KitId")
                        .HasColumnType("int");

                    b.Property<int>("ComponentId")
                        .HasColumnType("int");

                    b.Property<int>("ComponentQuantity")
                        .HasColumnType("int");

                    b.HasKey("KitId", "ComponentId")
                        .HasName("PK__KitCompo__34172BA37FE93EBD");

                    b.HasIndex("ComponentId");

                    b.ToTable("KitComponent");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.KitImage", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("KitId")
                        .HasColumnType("int");

                    b.Property<string>("Url")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.HasKey("Id")
                        .HasName("PK__KitImage__3214EC079BF747AB");

                    b.HasIndex("KitId");

                    b.ToTable("KitImages");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.KitsCategory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id")
                        .HasName("PK__KitsCate__3214EC07C4BC9A6D");

                    b.ToTable("KitsCategory");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.Lab", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Author")
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("KitId")
                        .HasColumnType("int");

                    b.Property<int>("LevelId")
                        .HasColumnType("int");

                    b.Property<int>("MaxSupportTimes")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(true);

                    b.Property<string>("Url")
                        .IsRequired()
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.HasKey("Id")
                        .HasName("PK__Lab__3214EC070EF17576");

                    b.HasIndex("KitId");

                    b.HasIndex("LevelId");

                    b.ToTable("Lab");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.LabSupport", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("LabId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PackageId")
                        .HasColumnType("int");

                    b.Property<int>("RemainSupportTimes")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__LabSuppor__01846D66652C0B0E");

                    b.HasIndex("OrderId");

                    b.HasIndex("PackageId");

                    b.HasIndex("LabId", "PackageId", "OrderId")
                        .IsUnique();

                    b.ToTable("LabSupport");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.LabSupporter", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("FeedBack")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsFinished")
                        .HasColumnType("bit");

                    b.Property<Guid>("LabSupportId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Rating")
                        .HasColumnType("int");

                    b.Property<string>("StaffId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("LabSupportId");

                    b.HasIndex("StaffId");

                    b.ToTable("LabSupporters");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.Level", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id")
                        .HasName("PK__Level__3214EC07C1764FCF");

                    b.HasIndex(new[] { "Name" }, "UQ__Level__737584F6684B4625")
                        .IsUnique();

                    b.ToTable("Level");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.Method", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<string>("NormalizedName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.HasKey("Id")
                        .HasName("PK__Method__3214EC07C4B69160");

                    b.ToTable("Method");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.Package", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("KitId")
                        .HasColumnType("int");

                    b.Property<int>("LevelId")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.HasKey("Id")
                        .HasName("PK__Package__3214EC07FCAB6A0E");

                    b.HasIndex("KitId");

                    b.HasIndex("LevelId");

                    b.ToTable("Package");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.PackageLab", b =>
                {
                    b.Property<int>("PackageId")
                        .HasColumnType("int");

                    b.Property<Guid>("LabId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("PackageId", "LabId")
                        .HasName("PK__PackageL__8CFBE3412AEEA669");

                    b.HasIndex("LabId");

                    b.ToTable("PackageLab");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.PackageOrder", b =>
                {
                    b.Property<Guid>("OrderId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("PackageId")
                        .HasColumnType("int");

                    b.Property<int>("PackageQuantity")
                        .HasColumnType("int");

                    b.HasIndex("OrderId");

                    b.HasIndex("PackageId");

                    b.ToTable("PackageOrder");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.Payment", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("MethodId")
                        .HasColumnType("int");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id")
                        .HasName("PK__Payment__3214EC079CC5A858");

                    b.HasIndex("MethodId");

                    b.ToTable("Payment");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.RefreshToken", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTime>("ExpirationTime")
                        .HasColumnType("datetime2");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("RefreshTokens");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.UserOrders", b =>
                {
                    b.Property<Guid>("Id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetimeoffset")
                        .HasDefaultValueSql("(getutcdate())");

                    b.Property<DateTimeOffset?>("DeliveredAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Discount")
                        .HasColumnType("int");

                    b.Property<bool>("IsLabDownloaded")
                        .HasColumnType("bit");

                    b.Property<string>("Note")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("PaymentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<string>("ShippingStatus")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("TotalPrice")
                        .HasColumnType("int");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(450)
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id")
                        .HasName("PK__UserOrde__3214EC07B653418C");

                    b.HasIndex("PaymentId");

                    b.HasIndex("UserId");

                    b.ToTable("UserOrders");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("kit_stem_api.Models.Domain.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("kit_stem_api.Models.Domain.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("kit_stem_api.Models.Domain.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("kit_stem_api.Models.Domain.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.Component", b =>
                {
                    b.HasOne("kit_stem_api.Models.Domain.ComponentsType", "Type")
                        .WithMany("Components")
                        .HasForeignKey("TypeId")
                        .IsRequired()
                        .HasConstraintName("FK__Component__TypeI__6754599E");

                    b.Navigation("Type");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.Kit", b =>
                {
                    b.HasOne("kit_stem_api.Models.Domain.KitsCategory", "Category")
                        .WithMany("Kits")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Kit__CategoryId__6E01572D");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.KitComponent", b =>
                {
                    b.HasOne("kit_stem_api.Models.Domain.Component", "Component")
                        .WithMany("KitComponents")
                        .HasForeignKey("ComponentId")
                        .IsRequired()
                        .HasConstraintName("FK__KitCompon__Compo__73BA3083");

                    b.HasOne("kit_stem_api.Models.Domain.Kit", "Kit")
                        .WithMany("KitComponents")
                        .HasForeignKey("KitId")
                        .IsRequired()
                        .HasConstraintName("FK__KitCompon__KitId__72C60C4A");

                    b.Navigation("Component");

                    b.Navigation("Kit");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.KitImage", b =>
                {
                    b.HasOne("kit_stem_api.Models.Domain.Kit", "Kit")
                        .WithMany("KitImages")
                        .HasForeignKey("KitId")
                        .IsRequired()
                        .HasConstraintName("FK__KitImages__KitId__787EE5A0");

                    b.Navigation("Kit");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.Lab", b =>
                {
                    b.HasOne("kit_stem_api.Models.Domain.Kit", "Kit")
                        .WithMany("Labs")
                        .HasForeignKey("KitId")
                        .IsRequired()
                        .HasConstraintName("FK__Lab__KitId__00200768");

                    b.HasOne("kit_stem_api.Models.Domain.Level", "Level")
                        .WithMany("Labs")
                        .HasForeignKey("LevelId")
                        .IsRequired()
                        .HasConstraintName("FK__Lab__LevelId__7F2BE32F");

                    b.Navigation("Kit");

                    b.Navigation("Level");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.LabSupport", b =>
                {
                    b.HasOne("kit_stem_api.Models.Domain.Lab", "Lab")
                        .WithMany("LabSupports")
                        .HasForeignKey("LabId")
                        .IsRequired()
                        .HasConstraintName("FK__LabSuppor__LabId__22751F6C");

                    b.HasOne("kit_stem_api.Models.Domain.UserOrders", "Order")
                        .WithMany("LabSupports")
                        .HasForeignKey("OrderId")
                        .IsRequired()
                        .HasConstraintName("FK__LabSuppor__Order__236943A5");

                    b.HasOne("kit_stem_api.Models.Domain.Package", "Package")
                        .WithMany()
                        .HasForeignKey("PackageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Lab");

                    b.Navigation("Order");

                    b.Navigation("Package");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.LabSupporter", b =>
                {
                    b.HasOne("kit_stem_api.Models.Domain.LabSupport", "LabSupport")
                        .WithMany("LabSupporters")
                        .HasForeignKey("LabSupportId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("kit_stem_api.Models.Domain.ApplicationUser", "Staff")
                        .WithMany("LabSupporters")
                        .HasForeignKey("StaffId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.Navigation("LabSupport");

                    b.Navigation("Staff");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.Package", b =>
                {
                    b.HasOne("kit_stem_api.Models.Domain.Kit", "Kit")
                        .WithMany("Packages")
                        .HasForeignKey("KitId")
                        .IsRequired()
                        .HasConstraintName("FK__Package__KitId__05D8E0BE");

                    b.HasOne("kit_stem_api.Models.Domain.Level", "Level")
                        .WithMany()
                        .HasForeignKey("LevelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Kit");

                    b.Navigation("Level");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.PackageLab", b =>
                {
                    b.HasOne("kit_stem_api.Models.Domain.Lab", "Lab")
                        .WithMany("PackageLabs")
                        .HasForeignKey("LabId")
                        .IsRequired()
                        .HasConstraintName("FK__PackageLa__LabId__0A9D95DB");

                    b.HasOne("kit_stem_api.Models.Domain.Package", "Package")
                        .WithMany("PackageLabs")
                        .HasForeignKey("PackageId")
                        .IsRequired()
                        .HasConstraintName("FK__PackageLa__Packa__09A971A2");

                    b.Navigation("Lab");

                    b.Navigation("Package");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.PackageOrder", b =>
                {
                    b.HasOne("kit_stem_api.Models.Domain.UserOrders", "Order")
                        .WithMany()
                        .HasForeignKey("OrderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__PackageOr__Order__1F98B2C1");

                    b.HasOne("kit_stem_api.Models.Domain.Package", "Package")
                        .WithMany()
                        .HasForeignKey("PackageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__PackageOr__Packa__1EA48E88");

                    b.Navigation("Order");

                    b.Navigation("Package");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.Payment", b =>
                {
                    b.HasOne("kit_stem_api.Models.Domain.Method", "Method")
                        .WithMany("Payments")
                        .HasForeignKey("MethodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__Payment__MethodI__0F624AF8");

                    b.Navigation("Method");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.UserOrders", b =>
                {
                    b.HasOne("kit_stem_api.Models.Domain.Payment", "Payment")
                        .WithMany("UserOrders")
                        .HasForeignKey("PaymentId")
                        .IsRequired()
                        .HasConstraintName("FK__UserOrders__Payme__160F4887");

                    b.HasOne("kit_stem_api.Models.Domain.ApplicationUser", "User")
                        .WithMany("UserOrders")
                        .HasForeignKey("UserId")
                        .IsRequired()
                        .HasConstraintName("FK__UserOrders__UserI__151B244E");

                    b.Navigation("Payment");

                    b.Navigation("User");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.ApplicationUser", b =>
                {
                    b.Navigation("LabSupporters");

                    b.Navigation("UserOrders");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.Component", b =>
                {
                    b.Navigation("KitComponents");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.ComponentsType", b =>
                {
                    b.Navigation("Components");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.Kit", b =>
                {
                    b.Navigation("KitComponents");

                    b.Navigation("KitImages");

                    b.Navigation("Labs");

                    b.Navigation("Packages");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.KitsCategory", b =>
                {
                    b.Navigation("Kits");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.Lab", b =>
                {
                    b.Navigation("LabSupports");

                    b.Navigation("PackageLabs");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.LabSupport", b =>
                {
                    b.Navigation("LabSupporters");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.Level", b =>
                {
                    b.Navigation("Labs");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.Method", b =>
                {
                    b.Navigation("Payments");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.Package", b =>
                {
                    b.Navigation("PackageLabs");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.Payment", b =>
                {
                    b.Navigation("UserOrders");
                });

            modelBuilder.Entity("kit_stem_api.Models.Domain.UserOrders", b =>
                {
                    b.Navigation("LabSupports");
                });
#pragma warning restore 612, 618
        }
    }
}
