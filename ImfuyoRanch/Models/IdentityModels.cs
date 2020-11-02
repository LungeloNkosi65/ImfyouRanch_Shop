using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Shopping;

namespace ImfuyoRanch.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        public DbSet<Department> Departments { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Cart> Carts { get; set; }
        public DbSet<Cart_Item> Cart_Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Order_Item> Order_Items { get; set; }
        public DbSet<Order_Tracking> Order_Trackings { get; set; }
        public DbSet<Address> Addresses { get; set; }
        public DbSet<Shipping_Address> Shipping_Addresses { get; set; }
        public DbSet<Payment> Payments { get; set; }
        //
        public DbSet<Admin> Admins { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Affiliate> Affiliates { get; set; }
        //public DbSet<Affiliate_Joiner> Affiliate_Joiners { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<Deposit> Deposits { get; set; }
        public DbSet<Withdraw> Withdrawals { get; set; }
        public DbSet<Affiliate_Joiner> Affiliate_Joiners { get; set; }
        public DbSet<Transfer> Transfers { get; set; }
        public DbSet<WithdrawLevel> WithdrawLevels { get; set; }

        public DbSet<Employee> Employees { get; set; }

        public DbSet<Manager> Managers { get; set; }

        public DbSet<BranchAddress> BranchAddresses { get; set; }

        public System.Data.Entity.DbSet<ImfuyoRanch.Models.Tasks> Tasks { get; set; }

        public System.Data.Entity.DbSet<ImfuyoRanch.Models.EmployeeTask> EmployeeTasks { get; set; }

        public System.Data.Entity.DbSet<ImfuyoRanch.Models.ActivateManager> ActivateManagers { get; set; }
    }
}