using MailSender.Models.Domains;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace MailSender.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }
        public DbSet<Email> Emails { get; set; }
        public DbSet<Receiver> Receivers { get; set; }
        public DbSet<Pattern> Patterns { get; set; }
        public DbSet<EmailAttachment> Attachments { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ApplicationUser>()
                 .HasMany(x => x.Emails)
                 .WithRequired(x => x.User)
                 .HasForeignKey(x => x.UserId)
                 .WillCascadeOnDelete(false);

            modelBuilder.Entity<ApplicationUser>()
                 .HasMany(x => x.Receivers)
                 .WithRequired(x => x.User)
                 .HasForeignKey(x => x.UserId)
                 .WillCascadeOnDelete(false);
            base.OnModelCreating(modelBuilder);
        }
    }
}