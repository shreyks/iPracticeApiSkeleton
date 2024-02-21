using iPractice.DataAccess.Models;
using Microsoft.EntityFrameworkCore;

namespace iPractice.DataAccess
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Psychologist> Psychologists { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Availability> AvailableSlots { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        //public ApplicationDbContext() { }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        { }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Psychologist>().HasKey(psychologist => psychologist.Id);
            modelBuilder.Entity<Client>().HasKey(client => client.Id);
            modelBuilder.Entity<Psychologist>().HasMany(p => p.Clients).WithMany(b => b.Psychologists);
            modelBuilder.Entity<Client>().HasMany(p => p.Psychologists).WithMany(b => b.Clients);

            modelBuilder.Entity<Availability>()
                .HasKey(availability => availability.Id);

            modelBuilder.Entity<Availability>()
                .HasOne(availability => availability.Psychologist)
                .WithMany(psychologist => psychologist.Availabilities);

            modelBuilder.Entity<Booking>()
                .HasKey(booking => booking.Id);

            modelBuilder.Entity<Booking>()
                .HasOne(booking => booking.Client)
                .WithMany(client => client.Bookings);

            modelBuilder.Entity<Booking>()
                .HasOne<Psychologist>(booking => booking.Psychologist)
                .WithMany(psychologist => psychologist.Bookings);
        }
    }
}
