using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kit_stem_api.Models.Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace kit_stem_api.Data
{
    public class KitStemDbContext : IdentityDbContext<ApplicationUser>
    {

        public DbSet<Component> Components { get; set; }

        public DbSet<ComponentsType> ComponentsTypes { get; set; }

        public DbSet<Kit> Kits { get; set; }

        public DbSet<KitComponent> KitComponents { get; set; }

        public DbSet<KitImage> KitImages { get; set; }

        public DbSet<KitsCategory> KitsCategories { get; set; }

        public DbSet<Lab> Labs { get; set; }

        public DbSet<LabSupport> LabSupports { get; set; }

        public DbSet<Level> Levels { get; set; }

        public DbSet<Method> Methods { get; set; }

        public DbSet<Package> Packages { get; set; }

        public DbSet<PackageLab> PackageLabs { get; set; }

        public DbSet<PackageOrder> PackageOrders { get; set; }

        public DbSet<Payment> Payments { get; set; }

        public DbSet<ShippingStatus> ShippingStatuses { get; set; }

        public DbSet<UserOrders> UserOrders { get; set; }
        public KitStemDbContext(DbContextOptions<KitStemDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Component>(entity =>
       {
           entity.HasKey(e => e.Id).HasName("PK__Componen__3214EC072B0E2FDE");

           entity.HasOne(d => d.Type).WithMany(p => p.Components)
               .OnDelete(DeleteBehavior.ClientSetNull)
               .HasConstraintName("FK__Component__TypeI__6754599E");
       });

            modelBuilder.Entity<ComponentsType>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Componen__3214EC0732D88561");
            });

            modelBuilder.Entity<Kit>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Kit__3214EC071BF8D031");

                entity.HasOne(d => d.Category).WithMany(p => p.Kits).HasConstraintName("FK__Kit__CategoryId__6E01572D");
            });

            modelBuilder.Entity<KitComponent>(entity =>
            {
                entity.HasKey(e => new { e.KitId, e.ComponentId }).HasName("PK__KitCompo__34172BA37FE93EBD");

                entity.HasOne(d => d.Component).WithMany(p => p.KitComponents)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__KitCompon__Compo__73BA3083");

                entity.HasOne(d => d.Kit).WithMany(p => p.KitComponents)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__KitCompon__KitId__72C60C4A");
            });

            modelBuilder.Entity<KitImage>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__KitImage__3214EC079BF747AB");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Kit).WithMany(p => p.KitImages)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__KitImages__KitId__787EE5A0");
            });

            modelBuilder.Entity<KitsCategory>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__KitsCate__3214EC07C4BC9A6D");
            });

            modelBuilder.Entity<Lab>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Lab__3214EC070EF17576");

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.Status).HasDefaultValue(true);

                entity.HasOne(d => d.Kit).WithMany(p => p.Labs)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Lab__KitId__00200768");

                entity.HasOne(d => d.Level).WithMany(p => p.Labs)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Lab__LevelId__7F2BE32F");
            });

            modelBuilder.Entity<LabSupport>(entity =>
            {
                entity.HasKey(e => new { e.LabId, e.OrderId }).HasName("PK__LabSuppo__01846D66652C0B0E");

                entity.HasOne(d => d.Lab).WithMany(p => p.LabSupports)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LabSuppor__LabId__22751F6C");

                entity.HasOne(d => d.Order).WithMany(p => p.LabSupports)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__LabSuppor__Order__236943A5");
            });

            modelBuilder.Entity<Level>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Level__3214EC07C1764FCF");
            });

            modelBuilder.Entity<Method>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Method__3214EC07C4B69160");
            });

            modelBuilder.Entity<Package>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Package__3214EC07FCAB6A0E");

                entity.HasOne(d => d.Kit).WithMany(p => p.Packages)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__Package__KitId__05D8E0BE");
            });

            modelBuilder.Entity<PackageLab>(entity =>
            {
                entity.HasKey(e => new { e.PackageId, e.LabId }).HasName("PK__PackageL__8CFBE3412AEEA669");

                entity.HasOne(d => d.Lab).WithMany(p => p.PackageLabs)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PackageLa__LabId__0A9D95DB");

                entity.HasOne(d => d.Package).WithMany(p => p.PackageLabs)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__PackageLa__Packa__09A971A2");
            });

            modelBuilder.Entity<PackageOrder>(entity =>
            {
                entity.HasOne(d => d.Order).WithMany().HasConstraintName("FK__PackageOr__Order__1F98B2C1");

                entity.HasOne(d => d.Package).WithMany().HasConstraintName("FK__PackageOr__Packa__1EA48E88");
            });

            modelBuilder.Entity<Payment>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Payment__3214EC079CC5A858");

                entity.Property(e => e.Id).ValueGeneratedNever();

                entity.HasOne(d => d.Method).WithMany(p => p.Payments).HasConstraintName("FK__Payment__MethodI__0F624AF8");
            });

            modelBuilder.Entity<ShippingStatus>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__Shipping__3214EC0740CF1637");
            });

            modelBuilder.Entity<UserOrders>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PK__UserOrde__3214EC07B653418C");

                entity.Property(e => e.Id).ValueGeneratedNever();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("(getutcdate())");

                entity.HasOne(d => d.Payment).WithMany(p => p.UserOrders)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserOrders__Payme__160F4887");

                entity.HasOne(d => d.ShippingStatus).WithMany(p => p.UserOrders)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserOrders__Shipp__17F790F9");

                entity.HasOne(d => d.User).WithMany(p => p.UserOrders)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__UserOrders__UserI__151B244E");
            });
            base.OnModelCreating(modelBuilder);
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<IdentityRole>().HasData(SeedingRoles());
        }

        private ICollection<ApplicationUser> SeedingUsers()
        {
            var hasher = new PasswordHasher<ApplicationUser>();
            return new List<ApplicationUser>()
            {
                new ApplicationUser() {
                    Id = "f603a296-ebed-401f-a4e1-12c400aa1aa9",
                    UserName = "admin@gmail.com",
                    NormalizedUserName = "ADMIN@GMAIL.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Phương",
                    LastName = "Lê",
                    Address = "KitStemHub",
                    Points = 0,
                },
                new ApplicationUser() {
                    Id = "fbb62e34-6f7c-4ac2-a66a-e50d24af41b1",
                    UserName = "manager@gmail.com",
                    NormalizedUserName = "MANAGER@GMAIL.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Hoàng",
                    LastName = "Nguyễn",
                    Address = "KitStemHub",
                    Points = 0,
                },
                new ApplicationUser() {
                    Id = "4814332e-7697-4d26-842a-1e81b7fcfcee",
                    UserName = "trungnguyen@gmail.com",
                    NormalizedUserName = "TRUNGNGUYEN@GMAIL.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Trung",
                    LastName = "Nguyễn",
                    Address = "KitStemHub",
                    Points = 0,
                },
                new ApplicationUser() {
                    Id = "02bca83c-5843-4c0a-80fa-5908957f1547",
                    UserName = "khanhle@gmail.com",
                    NormalizedUserName = "KHANHLE@GMAIL.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Khánh",
                    LastName = "Lê",
                    Address = "KitStemHub",
                    Points = 0,
                },
                new ApplicationUser() {
                    Id = "1631c161-cc33-47b2-8fbf-eccf19caa810",
                    UserName = "hungnguyen@gmail.com",
                    NormalizedUserName = "HUNGNGUYEN@GMAIL.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Hưng",
                    LastName = "Nguyễn",
                    Address = "KitStemHub",
                    Points = 0,
                },
                new ApplicationUser() {
                    Id = "be88ca0d-f1c8-46ef-9086-d140ba0a7f44",
                    UserName = "khoale@gmail.com",
                    NormalizedUserName = "KHOALE@GMAIL.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Khoa",
                    LastName = "Lê",
                    Address = "KitStemHub",
                    Points = 0,
                },
                new ApplicationUser() {
                    Id = "1631c161-cc33-47b2-8fbf-eccf19caa810",
                    UserName = "quannguyen@gmail.com",
                    NormalizedUserName = "QUANNGUYEN@GMAIL.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Quân",
                    LastName = "Nguyễn",
                    Address = "KitStemHub",
                    Points = 0,
                },
                new ApplicationUser(){
                    Id = "6a26c696-2fcb-4794-91de-238886d65165",
                    UserName = "lethirieng@gmail.com",
                    NormalizedUserName = "LETHIRIENG@GMAIL.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Riêng",
                    LastName = "Lê",
                    Address = "123/37/38 Tây Hoà, Phước Bình, Thủ Đức",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "d713088c-cced-436d-856e-bf69c6d42f79",
                    UserName = "nguyenvana@example.com",
                    NormalizedUserName = "NGUYENVANA@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Văn A",
                    LastName = "Nguyễn",
                    Address = "123 Đường A, Quận 1, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "b975c566-0fef-480e-92c8-8b127ddf62ce",
                    UserName = "tranthib@example.com",
                    NormalizedUserName = "TRANTHIB@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Thị B",
                    LastName = "Trần",
                    Address = "234 Đường B, Quận 2, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "753e6e55-37bb-4f7d-a7b3-44e7594f39af",
                    UserName = "lethic@example.com",
                    NormalizedUserName = "LETHIC@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Thị C",
                    LastName = "Lê",
                    Address = "345 Đường C, Quận 3, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "a2c1b1cd-c604-45a6-bcff-761879347d18",
                    UserName = "phamvand@example.com",
                    NormalizedUserName = "PHAMVAND@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Văn D",
                    LastName = "Phạm",
                    Address = "456 Đường D, Quận 4, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "2c766140-c79f-4251-85f9-fc7c32a10ccb",
                    UserName = "dangthie@example.com",
                    NormalizedUserName = "DANGTHIE@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Thị E",
                    LastName = "Đặng",
                    Address = "567 Đường E, Quận 5, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "0fa5cfed-96e6-4610-9e09-eca0146159ac",
                    UserName = "nguyenthik@example.com",
                    NormalizedUserName = "NGUYENTHIK@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Thị K",
                    LastName = "Nguyễn",
                    Address = "678 Đường F, Quận 6, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "a6f157c4-6d26-43b3-b7de-262083d791bf",
                    UserName = "hoangvank@example.com",
                    NormalizedUserName = "HOANGVANK@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Văn K",
                    LastName = "Hoàng",
                    Address = "789 Đường G, Quận 7, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "69da9bc2-bd3d-40a9-83a6-2c015b7852ef",
                    UserName = "phamthil@example.com",
                    NormalizedUserName = "PHAMTHIL@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Thị L",
                    LastName = "Phạm",
                    Address = "890 Đường H, Quận 8, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "a7190ee0-3a75-45a7-ac13-8c07665cb413",
                    UserName = "doanthim@example.com",
                    NormalizedUserName = "DOANTHIM@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Thị M",
                    LastName = "Đoàn",
                    Address = "901 Đường I, Quận 9, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "d2f4a934-8640-475c-ba15-9ce7783719d1",
                    UserName = "vuvanl@example.com",
                    NormalizedUserName = "VUVANL@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Văn L",
                    LastName = "Vũ",
                    Address = "102 Đường J, Quận 10, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "3d724129-c506-4abf-84b0-ff2f5bde1bf7",
                    UserName = "tranthicn@example.com",
                    NormalizedUserName = "TRANTHICN@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Thị C N",
                    LastName = "Trần",
                    Address = "103 Đường K, Quận 11, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "a717ab47-ece0-420e-86ff-b25e32504b7e",
                    UserName = "lethibc@example.com",
                    NormalizedUserName = "LETHIBC@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Thị B C",
                    LastName = "Lê",
                    Address = "104 Đường L, Quận 12, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "0714e1f3-df88-4fbf-ad14-5cdeb124a7bd",
                    UserName = "phivantr@example.com",
                    NormalizedUserName = "PHIVANTR@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Văn Tr",
                    LastName = "Phi",
                    Address = "105 Đường M, Quận Tân Bình, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "f232e9dc-e6c3-4d9b-b0de-3b4696d3e30f",
                    UserName = "ngothith@example.com",
                    NormalizedUserName = "NGOTHITH@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Thị Th",
                    LastName = "Ngô",
                    Address = "106 Đường N, Quận Tân Phú, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "d58a9b62-218d-430c-aecd-0ed531a4cfa6",
                    UserName = "huynhvanp@example.com",
                    NormalizedUserName = "HUYNHVANP@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Văn P",
                    LastName = "Huỳnh",
                    Address = "107 Đường O, Quận Bình Thạnh, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "aa139df7-471d-4f65-a8e3-683040c00c36",
                    UserName = "phanthic@example.com",
                    NormalizedUserName = "PHANTHIC@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Thị C",
                    LastName = "Phan",
                    Address = "108 Đường P, Quận Gò Vấp, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "781225de-519c-4288-bff2-c5552a4d1c29",
                    UserName = "doanthixd@example.com",
                    NormalizedUserName = "DOANTHIXD@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Thị X D",
                    LastName = "Đoàn",
                    Address = "109 Đường Q, Quận Phú Nhuận, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "a7a407f4-1d0f-4269-9cb5-f243598cc9be",
                    UserName = "buithiyv@example.com",
                    NormalizedUserName = "BUITHIYV@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Thị Y V",
                    LastName = "Bùi",
                    Address = "110 Đường R, Quận Bình Chánh, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "344a92f1-b110-4c5b-8239-aa2193af8bcf",
                    UserName = "tranvand@example.com",
                    NormalizedUserName = "TRANVAND@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Văn D",
                    LastName = "Trần",
                    Address = "111 Đường S, Quận 1, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "6c0f3356-417b-496d-9ad3-3ea05aea9787",
                    UserName = "ngothix@example.com",
                    NormalizedUserName = "NGOTHIX@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Thị X",
                    LastName = "Ngô",
                    Address = "112 Đường T, Quận 2, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "dc631478-726b-4948-85de-8084b89163dc",
                    UserName = "nguyenvanth@example.com",
                    NormalizedUserName = "NGUYENVANTH@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Văn Th",
                    LastName = "Nguyễn",
                    Address = "113 Đường U, Quận 3, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "b00ed775-1060-4684-bdac-7e54b5fd1ff5",
                    UserName = "hoangthiq@example.com",
                    NormalizedUserName = "HOANGTHIQ@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Thị Q",
                    LastName = "Hoàng",
                    Address = "114 Đường V, Quận 4, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "523cc61b-5c55-4c1a-a960-0e88a546ff27",
                    UserName = "phithiy@example.com",
                    NormalizedUserName = "PHITHIY@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Thị Y",
                    LastName = "Phi",
                    Address = "115 Đường W, Quận 5, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "880b4d1a-2058-42e4-b5be-c6d26d39169b",
                    UserName = "dangthiz@example.com",
                    NormalizedUserName = "DANGTHIZ@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Thị Z",
                    LastName = "Đặng",
                    Address = "116 Đường X, Quận 6, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "9f66db98-5650-4a70-9277-8fd48d239e56",
                    UserName = "phamvanxx@example.com",
                    NormalizedUserName = "PHAMVANXX@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Văn X X",
                    LastName = "Phạm",
                    Address = "117 Đường Y, Quận 7, TP.HCM",
                    Points = 0,
                },
                new ApplicationUser
                {
                    Id = "b29fec83-9d5c-43e7-8d21-7c53ab51eb14",
                    UserName = "hoangthidl@example.com",
                    NormalizedUserName = "HOANGTHIDL@EXAMPLE.COM",
                    PasswordHash = hasher.HashPassword(null, "12345678aA@"),
                    FirstName = "Thị D L",
                    LastName = "Hoàng",
                    Address = "118 Đường Z, Quận 8, TP.HCM",
                    Points = 0,
                }
            };
        }
        private ICollection<IdentityRole> SeedingRoles()
        {
            return new List<IdentityRole>()
            {
                new IdentityRole() {
                    Id = "db9e2576-9d94-4183-8d6e-2743cc3d5d39",
                    Name = "admin",
                    NormalizedName = "ADMIN",
                    ConcurrencyStamp = "db9e2576-9d94-4183-8d6e-2743cc3d5d39"
                },
                new IdentityRole() {
                    Id = "9fd1af37-4fee-42e3-9bee-c025577ce705",
                    Name = "manager",
                    NormalizedName = "MANAGER",
                    ConcurrencyStamp = "9fd1af37-4fee-42e3-9bee-c025577ce705"
                },
                new IdentityRole() {
                    Id = "0da287c4-8e61-42eb-a177-5c9c24890ae2",
                    Name = "staff",
                    NormalizedName = "STAFF",
                    ConcurrencyStamp = "0da287c4-8e61-42eb-a177-5c9c24890ae2"
                },
                new IdentityRole() {
                    Id = "827dae5f-ef37-4606-a6a0-49721ce0e93e",
                    Name = "customer",
                    NormalizedName = "CUSTOMER",
                    ConcurrencyStamp = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                }
            };
        }
        private ICollection<IdentityUserRole<string>> SeedingUserRoles()
        {
            return new List<IdentityUserRole<string>>()
            {
                new IdentityUserRole<string>
                {
                    RoleId = "db9e2576-9d94-4183-8d6e-2743cc3d5d39",
                    UserId = "f603a296-ebed-401f-a4e1-12c400aa1aa9"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "9fd1af37-4fee-42e3-9bee-c025577ce705",
                    UserId = "fbb62e34-6f7c-4ac2-a66a-e50d24af41b1"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "0da287c4-8e61-42eb-a177-5c9c24890ae2",
                    UserId = "4814332e-7697-4d26-842a-1e81b7fcfcee"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "0da287c4-8e61-42eb-a177-5c9c24890ae2",
                    UserId = "02bca83c-5843-4c0a-80fa-5908957f1547"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "0da287c4-8e61-42eb-a177-5c9c24890ae2",
                    UserId = "1631c161-cc33-47b2-8fbf-eccf19caa810"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "0da287c4-8e61-42eb-a177-5c9c24890ae2",
                    UserId = "be88ca0d-f1c8-46ef-9086-d140ba0a7f44"
                },
                new IdentityUserRole<string>
                {
                    RoleId = "0da287c4-8e61-42eb-a177-5c9c24890ae2",
                    UserId = "1631c161-cc33-47b2-8fbf-eccf19caa810"
                },
                new IdentityUserRole<string>
                {
                    UserId = "6a26c696-2fcb-4794-91de-238886d65165",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "d713088c-cced-436d-856e-bf69c6d42f79",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "b975c566-0fef-480e-92c8-8b127ddf62ce",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "753e6e55-37bb-4f7d-a7b3-44e7594f39af",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "a2c1b1cd-c604-45a6-bcff-761879347d18",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "2c766140-c79f-4251-85f9-fc7c32a10ccb",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "0fa5cfed-96e6-4610-9e09-eca0146159ac",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "a6f157c4-6d26-43b3-b7de-262083d791bf",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "69da9bc2-bd3d-40a9-83a6-2c015b7852ef",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "a7190ee0-3a75-45a7-ac13-8c07665cb413",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "d2f4a934-8640-475c-ba15-9ce7783719d1",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "3d724129-c506-4abf-84b0-ff2f5bde1bf7",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "a717ab47-ece0-420e-86ff-b25e32504b7e",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "0714e1f3-df88-4fbf-ad14-5cdeb124a7bd",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "f232e9dc-e6c3-4d9b-b0de-3b4696d3e30f",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "d58a9b62-218d-430c-aecd-0ed531a4cfa6",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "aa139df7-471d-4f65-a8e3-683040c00c36",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "781225de-519c-4288-bff2-c5552a4d1c29",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "a7a407f4-1d0f-4269-9cb5-f243598cc9be",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "344a92f1-b110-4c5b-8239-aa2193af8bcf",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "6c0f3356-417b-496d-9ad3-3ea05aea9787",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "dc631478-726b-4948-85de-8084b89163dc",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "b00ed775-1060-4684-bdac-7e54b5fd1ff5",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "523cc61b-5c55-4c1a-a960-0e88a546ff27",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "880b4d1a-2058-42e4-b5be-c6d26d39169b",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "9f66db98-5650-4a70-9277-8fd48d239e56",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                },
                new IdentityUserRole<string>
                {
                    UserId = "b29fec83-9d5c-43e7-8d21-7c53ab51eb14",
                    RoleId = "827dae5f-ef37-4606-a6a0-49721ce0e93e"
                }
            };
        }
        private ICollection<ComponentsType> SeedingComponentsType()
        {
            return new List<ComponentsType>()
            {
                new ComponentsType() {
                    Id = 1,
                    Name = "Đèn LED, Điều khiển LED"
                },
                new ComponentsType() {
                    Id = 2,
                    Name = "Điện dân dụng và Công nghiệpĐiện dân dụng và Công nghiện"
                },
                new ComponentsType() {
                    Id = 3,
                    Name = "Điện năng lượng mặt trời"
                },
                new ComponentsType() {
                    Id = 4,
                    Name = "Đồng hồ vạn năng"
                },
                new ComponentsType() {
                    Id = 5,
                    Name = "Máy in 3D, Công nghệ"
                },
                new ComponentsType() {
                    Id = 6,
                    Name = "Module, Mạch điện"
                },
                new ComponentsType() {
                    Id = 7,
                    Name = "Phụ kiện, Dụng cụ"
                },
                new ComponentsType() {
                    Id = 8,
                    Name = "Robot, Phụ kiện DIY"
                }
            };
        }
    }
}