using Core.Entities;
using Core.Enums;
using Core.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Contexts
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
           : base(options)
        {
        }


        public DbSet<ApplicationUser> ApplicationUsers { get; set; }
        public DbSet<Specialization> Specializations { get; set; } = default!;
        public DbSet<Appointment> Appointments { get; set; } = default!;
        public DbSet<AppointmentTimeSlot> AppointmentTimeSlots { get; set; } = default!;
        public DbSet<Booking> Bookings { get; set; } = default!;
        public DbSet<Coupon> Coupons { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            // change Identity Db shecma
            builder.Entity<ApplicationUser>().ToTable("Users", "security");
            builder.Entity<IdentityRole>().ToTable("Roles", "security");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "security");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "security");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "security");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "security");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "security");


            // Configure the relationship between Appointment and  Application User(doctor)

            builder.Entity<Appointment>()
                .HasOne(a => a.ApplicationUser)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.ApplicationUserId)
                .IsRequired();


            // Configure the relationship between Appointment and AppointmentTimeSlot

            builder.Entity<AppointmentTimeSlot>()
                .HasOne(ts => ts.Appointment)
                .WithMany(a => a.AppointmentTimeSlots)
                .HasForeignKey(ts => ts.AppointmentId)
                .IsRequired();



            // Configure the relationship between Booking and  Application User(patient)

            builder.Entity<Booking>()
                .HasOne(a => a.ApplicationUser)
                .WithMany(b => b.Bookings)
                .HasForeignKey(a => a.ApplicationUserId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            // Configure the relationship between Booking and  AppointmentTimeSlot

            builder.Entity<Booking>()
                .HasOne(b => b.AppointmentTimeSlot)
                .WithMany(ts => ts.Bookings)
                .HasForeignKey(ts => ts.AppointmentTimeSlotId)
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);


            // configure coupons
            builder.Entity<Coupon>()
                       .Property(c => c.isApplied)
            .HasDefaultValue(false);

            builder.Entity<Coupon>()
                .Property(c => c.isActived)
                .HasDefaultValue(true);

            // Ensure DiscountCode is unique
            builder.Entity<Coupon>()
                .HasIndex(c => c.DiscoundCode)
                .IsUnique();

            // default booking status

            builder.Entity<Booking>()
                .Property(s => s.Status)
                .HasDefaultValue(BookingStatus.Pending);

            builder.Entity<Booking>()
               .Property(s => s.DiscountCode)
               .HasDefaultValue(null);

            builder.Entity<ApplicationUser>()
                .Property(n => n.NumOfRequests)
                .HasDefaultValue(0);



        }










    }
}
