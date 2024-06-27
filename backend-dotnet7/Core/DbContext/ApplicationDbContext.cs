using backend_dotnet7.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace backend_dotnet7.Core.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> //generic type
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Log> Logs { get; set; }
        public DbSet<Message> Messages { get; set; }

        public DbSet<Transaction> Transactions { get; set; }

        public DbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Config anything we want
            //1
            builder.Entity<ApplicationUser>(e =>
            {
                e.ToTable("Users");
            });

            //2
            builder.Entity<IdentityUserClaim<string>>(e =>
            {
                e.ToTable("UserClaims");
            });

            //3
            builder.Entity<IdentityUserLogin<string>>(e =>
            {
                e.ToTable("UserLogins");
            });

            //4
            builder.Entity<IdentityUserToken<string>>(e =>
            {
                e.ToTable("UserTokens");
            });

            //5
            builder.Entity<IdentityRole>(e =>
            {
                e.ToTable("Roles");
            });

            //6
            builder.Entity<IdentityRoleClaim<string>>(e =>
            {
                e.ToTable("RoleClaims");
            });

            //7
            builder.Entity<IdentityUserRole<string>>(e =>
            {
               e.ToTable("UserRoles");
            });


            builder.Entity<Category>().HasData(new Category[]
            {
                new Category {
                    CategoryId = 1,
                    Title="House Bill",
                    Icon = "🏚️",
                    Type="Expense"
                },
                new Category {
                    CategoryId=2,
                    Title="Water Bill",
                    Icon="🚰",
                    Type= "Income",

                }
            });

            builder.Entity<Transaction>().HasData(new Transaction[]
            {
                new Transaction {
                    TransactionId = 100,
                    CategoryId = 1,

                    Amount = 220,
                    Note = "House utility Bill",
                    Date = DateTime.Now,
                    Status = TransactionStatus.New,

                },
                new Transaction
                {
                    TransactionId = 108,
                    CategoryId = 2,

                    Amount = 220,
                    Note = "Water utility Bill",
                    Date = DateTime.Now,
                    Status = TransactionStatus.New,
                }
            });


        }
    }
}
