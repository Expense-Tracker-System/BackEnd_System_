using backend_dotnet7.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace backend_dotnet7.Core.DbContext
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser> //generic type
    {
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Log> Logs { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Reminder> Reminders { get; set; }
        public DbSet<Budget> Budgets { get; set; }
        public DbSet<BExpense> BExpenses { get; set; }
        public DbSet<UserIncome> UserIncomes { get; set; }
        public DbSet<UserExpense> UserExpenses { get; set; }
        public DbSet<UserOrganization> UserOrganizations { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        public DbSet<OrganizationIncome> OrganizationIncomes { get; set; }
        public DbSet<OrganizationExpense> OrganizationExpenses { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Category> categories { get; set; }


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

            builder.Entity<Transaction>().HasData(new Transaction[] {
               new Transaction
               {
                    Id = 4,
                    Amount = 800,
                    Note = "Ileccity Bill",
                    Created = DateTime.Now,
                    Status = TransactionStatus.Completed,
                    CategoryId = 1,
               },
               new Transaction
               {

                    Id = 1,
                    Amount = 200,
                    Note = "Elecity Bill",
                    Created = DateTime.Now,
                    Status = TransactionStatus.Completed,
                    CategoryId = 2,
               },
               new Transaction{
                    Id = 2,
                    Amount = 500,
                    Note = "water Bill",
                    Created = DateTime.Now,
                    Status = TransactionStatus.Completed,
                    CategoryId= 3,
               },
               new Transaction{
                    Id = 3,
                    Amount = 1000,
                    Note = "Medicine",
                    Created = DateTime.Now,
                    Status = TransactionStatus.Completed,
                    CategoryId= 4,
               }
            });

            builder.Entity<Category>().HasData(new Category[] { 
                new Category{ Id = 1,Title="Eleccity Bill", Icon="💡"},
                new Category{ Id = 2,Title="Water bill", Icon="🚰" },
                new Category{ Id = 3,Title="Travel", Icon="✈️" },
                new Category{ Id = 4,Title="Medicine", Icon="💊" }
            });

            // primary key
            builder.Entity<Log>()
                .HasKey(log => log.Id);

            // relationship logs
            builder.Entity<Log>()
                .HasOne(log => log.applicationUser)
                .WithMany(u => u.logs)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // primary key
            builder.Entity<Message>()
                .HasKey(message => message.Id);

            // relationship message
            builder.Entity<Message>()
                .HasOne(message => message.applicationUser)
                .WithMany(u => u.messages)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // primary key
            builder.Entity<UserIncome>()
                .HasKey(userIncome => userIncome.Id);

            // relationship user income
            builder.Entity<UserIncome>()
                .HasOne(income => income.applicationUser)
                .WithMany(u => u.userIncomes)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // primary key
            builder.Entity<UserExpense>()
                .HasKey(expense => expense.Id);

            // relationship user expense
            builder.Entity<UserExpense>()
                .HasOne(expense => expense.applicationUser)
                .WithMany(u => u.userExpenses)
                .HasForeignKey(u => u.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            // primary key
            builder.Entity<UserOrganization>()
                .HasKey(uo => new { uo.UserId, uo.OrganizationId });

            // relationship user organization -> user
            builder.Entity<UserOrganization>()
                .HasOne(uo => uo.applicationUser)
                .WithMany(u => u.userOrganizations)
                .HasForeignKey(uo => uo.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            

            builder.Entity<UserOrganization>()
                .HasOne(uo => uo.organization)
                .WithMany(o => o.userOrganizations)
                .HasForeignKey(uo => uo.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            // primary key
            builder.Entity<Organization>()
                .HasKey(organization => organization.Id);

            // primary key
            builder.Entity<OrganizationIncome>()
                .HasKey(orgIn => orgIn.Id);

            // relationship organization
            builder.Entity<OrganizationIncome>()
                .HasOne(oi => oi.organization)
                .WithMany(o => o.organizationIncomes)
                .HasForeignKey(oi => oi.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);

            // primary key
            builder.Entity<OrganizationExpense>()
                .HasKey(orgEx => orgEx.Id);

            // relationship organization
            builder.Entity<OrganizationExpense>()
                .HasOne(oe => oe.organization)
                .WithMany(o => o.organizationExpenses)
                .HasForeignKey(oi => oi.OrganizationId)
                .OnDelete(DeleteBehavior.Restrict);
        }
            
    }
}
