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
        public KitStemDbContext(DbContextOptions<KitStemDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<IdentityRole>().HasData(SeedingRoles());
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