using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

//The C# Dev Kit does not add the Namespace within the right folder correctly
namespace API.Data;
public class DataContext : IdentityDbContext<AppUser, AppRole, int, 
    IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, 
    IdentityRoleClaim<int>, IdentityUserToken<int>> 
{
    public DataContext(DbContextOptions options) : base(options)
    {
    }
    public DbSet<UserVisit> Visits { get; set; }
    public DbSet<UserLike> Likes {get;set;}
    public DbSet<Message> Messages {get; set;}
    public DbSet<Group> Groups {get; set;}
    public DbSet<Connection> Connections {get;set;}

    protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

            // Specify primary key
            builder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.TargetUserId });

            // Set the relationships
            builder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);
            

            builder.Entity<UserLike>()
                .HasOne(s => s.TargetUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(s => s.TargetUserId)
                .OnDelete(DeleteBehavior.Cascade);
                // Note: For SQL Server, set the DeleteBehavior to
                // DeleteBehavior.NoAction or you will get an error during migration.

            builder.Entity<Message>()
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessagesReceived)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(u => u.Sender)
                .WithMany(m => m.MessagesSent)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<UserVisit>()
                .HasKey(k => new { k.SourceUserId, k.TargetUserId });

            builder.Entity<UserVisit>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.VisitedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.Cascade);
            

            builder.Entity<UserVisit>()
                .HasOne(s => s.TargetUser)
                .WithMany(l => l.UsersVisitedBy)
                .HasForeignKey(s => s.TargetUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

}