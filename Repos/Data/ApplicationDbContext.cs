using accountSystem.Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<TransactionAccount> TransactionAccounts { get; set; }
    public DbSet<LedgerEntry> LedgerEntries { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<UserRoleAssignment> UserRoleAssignments { get; set; }
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<TransactionLog> TransactionLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Configuring the TransactionAccount Junction Table for Many-to-Many relationship
        modelBuilder.Entity<TransactionAccount>()
            .HasKey(ta => new { ta.TransactionId, ta.AccountId });

        modelBuilder.Entity<TransactionAccount>()
            .HasOne(ta => ta.Transaction)
            .WithMany(t => t.TransactionAccounts)
            .HasForeignKey(ta => ta.TransactionId);

        modelBuilder.Entity<TransactionAccount>()
            .HasOne(ta => ta.Account)
            .WithMany(a => a.TransactionAccounts)
            .HasForeignKey(ta => ta.AccountId);

        // 2. Configuring the LedgerEntry relationship with Transaction (One-to-Many)
        // تكوين العلاقة مع Transaction (واحد لعدة)
        modelBuilder.Entity<LedgerEntry>()
            .HasOne(le => le.Transaction)
            .WithMany(t => t.LedgerEntries)
            .HasForeignKey(le => le.TransactionId);

        // تحديد نوع العمود للخصائص العشرية
        modelBuilder.Entity<LedgerEntry>()
            .Property(le => le.Amount)
            .HasColumnType("decimal(18,2)");  // تأكد من إضافة القوس المغلق هنا

        modelBuilder.Entity<LedgerEntry>()
            .Property(le => le.BalanceAfterTransaction)
            .HasColumnType("decimal(18,2)");  

            // 3. Configuring the UserRoleAssignment Junction Table for Many-to-Many relationship between Users and UserRoles
            modelBuilder.Entity<UserRoleAssignment>()
            .HasKey(ura => new { ura.UserId, ura.UserRoleId });

        modelBuilder.Entity<UserRoleAssignment>()
            .HasOne(ura => ura.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ura => ura.UserId);

        modelBuilder.Entity<UserRoleAssignment>()
            .HasOne(ura => ura.UserRole)
            .WithMany(ur => ur.Users)
            .HasForeignKey(ura => ura.UserRoleId);

        // Additional configurations if necessary
    }
}
