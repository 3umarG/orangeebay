using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Orange_Bay.Models.AdditionalServices;
using Orange_Bay.Models.Auth;
using Orange_Bay.Models.Booking;
using Orange_Bay.Models.CompanyImages;
using Orange_Bay.Models.ContactUs;
using Orange_Bay.Models.Dining;
using Orange_Bay.Models.Gallery;
using Orange_Bay.Models.Programs;
using Orange_Bay.Models.SliderImage;
using Orange_Bay.Models.Tickets;
using Orange_Bay.Models.Wishlist;

namespace Orange.EF;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<int>, int>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public ApplicationDbContext()
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<ApplicationUser>().ToTable("Users");
        builder.Entity<IdentityRole<int>>().ToTable("Roles");
        builder.Entity<IdentityUserRole<int>>().ToTable("UserRoles");
        builder.Entity<IdentityUserToken<int>>().ToTable("UserTokens");
        builder.Entity<IdentityUserClaim<int>>().ToTable("UserClaims");
        builder.Entity<IdentityRoleClaim<int>>().ToTable("RoleClaims");


        builder.Entity<IdentityRole<int>>().HasData(
            new IdentityRole<int>
            {
                Id = 1,
                Name = "user",
                NormalizedName = "user".ToUpper()
            },
            new IdentityRole<int>
            {
                Id = 2,
                Name = "admin",
                NormalizedName = "admin".ToUpper()
            }
        );


        builder.Entity<ApplicationUser>()
            .HasOne(user => user.CultureType)
            .WithMany(culture => culture.ApplicationUsers)
            .HasForeignKey(user => user.CultureTypeId);

        builder.Entity<ApplicationUser>()
            .HasOne(user => user.UserType)
            .WithMany(userType => userType.ApplicationUsers)
            .HasForeignKey(user => user.UserTypeId);

        builder.Entity<CultureType>().HasData(
            new CultureType
            {
                Id = 1,
                Name = "ENG",
                Description = "English"
            },
            new CultureType
            {
                Id = 2,
                Name = "ARB",
                Description = "Arabic"
            }
        );


        builder.Entity<UserType>().HasData(
            new UserType
            {
                Id = 1,
                Name = "Client",
                Description = "Original Client"
            },
            new UserType
            {
                Id = 2,
                Name = "Yacht Owner",
                Description = "All Users who have yachts"
            }
        );


        builder.Entity<Program>().HasData(
            new Program
            {
                Id = 1,
                Name = "Program 1",
                Description = "Description for Program 1",
                InternalNotes = "Notes .........",
                SpecialRequirements = "Requirements ..........",
                MaxCapacity = 25,
                Location = "Orange Island",
                DurationInHours = 12
            },
            new Program
            {
                Id = 2,
                Name = "Program 2",
                Description = "Description for Program 2",
                InternalNotes = "Notes .........",
                SpecialRequirements = "Requirements ..........",
                MaxCapacity = 12,
                Location = "Orange Island",
                DurationInHours = 32
            }
        );

        builder.Entity<ProgramPlan>().HasData(
            new ProgramPlan
            {
                Id = 1,
                Description = "Start the Trip",
                ProgramId = 1,
                Time = "09:00 AM"
            },
            new ProgramPlan
            {
                Id = 2,
                Description = "Sea",
                ProgramId = 1,
                Time = "03:00 AM"
            },
            new ProgramPlan
            {
                Id = 3,
                Description = "Evening",
                ProgramId = 1,
                Time = "10:00 PM"
            },
            new ProgramPlan
            {
                Id = 4,
                Description = "Ending",
                ProgramId = 1,
                Time = "12:00 PM"
            }
        );

        builder.Entity<DiningCategory>().HasData(
            new DiningCategory
            {
                Id = 1,
                Name = "Restaurant"
            },
            new DiningCategory
            {
                Id = 2,
                Name = "Bar"
            },
            new DiningCategory
            {
                Id = 3,
                Name = "Lounges"
            }
        );


        builder.Entity<AdditionalService>().HasData(
            new AdditionalService
            {
                Id = 1,
                Name = "Boat",
                Description = "Extra Boat From Beach"
            },
            new AdditionalService
            {
                Id = 2,
                Name = "Photo session",
                Description = "For Weddings"
            }
        );

        builder.Entity<AdditionalServicePrice>().HasData(
            new AdditionalServicePrice
            {
                Id = 1,
                UserTypeId = 1,
                ServiceId = 1,
                PricePerAdult = 250,
                PricePerChild = 150
            },
            new AdditionalServicePrice
            {
                Id = 2,
                UserTypeId = 2,
                ServiceId = 1,
                PricePerAdult = 350,
                PricePerChild = 200
            },
            new AdditionalServicePrice
            {
                Id = 3,
                UserTypeId = 1,
                ServiceId = 2,
                PricePerAdult = 800,
                PricePerChild = 50
            },
            new AdditionalServicePrice
            {
                Id = 4,
                UserTypeId = 2,
                ServiceId = 2,
                PricePerAdult = 400,
                PricePerChild = 130
            }
        );


        builder.Entity<GalleryImageType>().HasData(
            new GalleryImageType
            {
                Id = 1,
                Type = "Panoramic View"
            },
            new GalleryImageType
            {
                Id = 2,
                Type = "Relax"
            },
            new GalleryImageType
            {
                Id = 3,
                Type = "Joy and Fun"
            },
            new GalleryImageType
            {
                Id = 4,
                Type = "Dining"
            }
        );

        builder.Entity<ReservationPersonDetails>()
            .Property(bp => bp.Type)
            .HasConversion<string>();

        builder.Entity<Reservation>()
            .Property(bp => bp.AttendanceStatus)
            .HasConversion<string>()
            .HasDefaultValue(AttendanceStatus.Pending);

        builder.Entity<ContactUsMessage>()
            .Property(message => message.CreatedOn)
            .HasDefaultValueSql("getdate()");

        builder.Entity<Reservation>()
            .Property(reservation => reservation.BookedOn)
            .HasDefaultValueSql("getdate()");

        builder.Entity<Reservation>()
            .HasIndex(r => r.BookingDate);

        builder.Entity<ReservationPaymentDetails>()
            .HasOne<Reservation>(payment => payment.Reservation)
            .WithOne(r => r.PaymentDetails)
            .HasForeignKey<ReservationPaymentDetails>(payment => payment.ReservationId);

        // builder.Entity<SliderImage>()
        //     .HasIndex(image => image.Title);

        // builder.Entity<TicketImage>()
        //     .HasIndex(image => image.Title);

        builder.Entity<TicketDetails>()
            .HasIndex(t => t.CreatedOn);


        builder.Entity<TicketType>()
            .HasData(new TicketType
                {
                    Id = 1,
                    Type = "Single"
                },
                new TicketType
                {
                    Id = 2,
                    Type = "Double"
                }, new TicketType
                {
                    Id = 3,
                    Type = "Family"
                }, new TicketType
                {
                    Id = 4,
                    Type = "Group"
                });
    }

    // public virtual DbSet<CultureType> CultureTypes { get; set; }
    public virtual DbSet<UserType> UserTypes { get; set; }
    public virtual DbSet<Program> Programs { get; set; }
    public virtual DbSet<ProgramImage> ProgramImages { get; set; }
    public virtual DbSet<ProgramPlan> ProgramPlans { get; set; }
    public virtual DbSet<ProgramReview> ProgramReviews { get; set; }
    public virtual DbSet<ProgramPrice> ProgramPrices { get; set; }
    public virtual DbSet<ProgramWishlist> ProgramWishlists { get; set; }
    public virtual DbSet<GalleryImage> GalleryImages { get; set; }
    public virtual DbSet<GalleryImageType> GalleryImageTypes { get; set; }
    public virtual DbSet<GalleryVideo> GalleryVideos { get; set; }

    public virtual DbSet<DiningCategory> DiningCategories { get; set; }
    public virtual DbSet<DiningItem> DiningItems { get; set; }
    // public virtual DbSet<DiningImage> DiningImages { get; set; }
    public virtual DbSet<AdditionalService> AdditionalServices { get; set; }
    public virtual DbSet<AdditionalServicePrice> AdditionalServicePrices { get; set; }

    public virtual DbSet<Reservation> Reservations { get; set; }
    public virtual DbSet<ReservationAdditionalService> ReservationAdditionalServices { get; set; }
    public virtual DbSet<ProgramNote> ProgramNotes { get; set; }
    public virtual DbSet<ProgramIncluded> ProgramIncludedDetails { get; set; }
    public virtual DbSet<ProgramExcluded> ProgramExcludedDetails { get; set; }
    public virtual DbSet<ReservationPersonDetails> ReservationPersonsDetails { get; set; }
    public virtual DbSet<ContactUsMessage> ContactUsMessages { get; set; }
    public virtual DbSet<ReservationPaymentDetails> ReservationsPaymentDetails { get; set; }
    public virtual DbSet<SliderImage> SliderImages { get; set; }
    public virtual DbSet<TicketDetails> TicketsDetails { get; set; }
    public virtual DbSet<TicketImage> TicketImages { get; set; }
    public virtual DbSet<TicketType> TicketsTypes { get; set; }
    public virtual DbSet<CompanyImage> CompaniesImages { get; set; }
}