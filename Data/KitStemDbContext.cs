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
    }
}