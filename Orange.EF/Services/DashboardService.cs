using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Orange_Bay.DTOs;
using Orange_Bay.DTOs.AdditionalServices;
using Orange_Bay.DTOs.Dashboard;
using Orange_Bay.DTOs.Profile;
using Orange_Bay.DTOs.Shared;
using Orange_Bay.Exceptions;
using Orange_Bay.Interfaces.Services;
using Orange_Bay.Models.AdditionalServices;
using Orange_Bay.Models.Auth;
using Orange_Bay.Models.Booking;
using Orange_Bay.Models.Programs;
using Orange_Bay.Utils;

namespace Orange.EF.Services;

public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _dbContext;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly Jwt _jwt;
    private readonly RoleManager<IdentityRole<int>> _roleManager;

    public DashboardService(ApplicationDbContext dbContext,
        UserManager<ApplicationUser> userManager,
        IOptions<Jwt> jwt,
        RoleManager<IdentityRole<int>> roleManager)
    {
        _dbContext = dbContext;
        _userManager = userManager;
        _jwt = jwt.Value;
        _roleManager = roleManager;
    }

    public async Task<DashboardDailyOverviewResponseDto> GetDashboardDailyOverviewAsync(DateTime date)
    {
        var reservations = await _dbContext.Reservations
            .Include(r => r.ReservationAdditionalServices)
            .Include(r => r.ReservationPersonsDetails)
            .Include(r => r.User)
            .AsSplitQuery()
            .Where(reservation => reservation.BookingDate.Date == date.Date)
            .ToListAsync();

        var numberOfAdults = reservations
            .Where(reservation => !reservation.IsCancelled)
            .Sum(reservation => reservation.NumberOfAdults);

        var numberOfChild = reservations
            .Where(reservation => !reservation.IsCancelled)
            .Sum(reservation => reservation.NumberOfChild);

        var totalAdultsPrice = reservations
            .Where(reservation => !reservation.IsCancelled)
            .Sum(reservation => reservation.AdultTotalPrice);

        var totalChildPrice = reservations
            .Where(reservation => !reservation.IsCancelled)
            .Sum(reservation => reservation.ChildTotalPrice);

        var numberOfAdditionalServices =
            reservations
                .Where(reservation => !reservation.IsCancelled)
                .Sum(reservation => reservation.ReservationAdditionalServices?.Count ?? 0);

        var totalPriceOfAdditionalServices = reservations
            .Where(reservation => !reservation.IsCancelled)
            .Sum(reservation => reservation.TotalAdditionalServicesPrice ?? 0);

        var numberOfBookingByGuest = reservations.Count(reservation => reservation.User.UserTypeId == 1);
        var numberOfBookingByCompany = reservations.Count(reservation => reservation.User.UserTypeId == 2);
        var numberOfBookingByEmployee = reservations.Count(reservation => reservation.User.UserTypeId == 3);

        var totalPriceOfReservationsByCompany = reservations
            .Where(reservation => reservation.User.UserTypeId == 2)
            .Where(reservation => !reservation.IsCancelled)
            .Sum(reservation => reservation.TotalReservationPrice);

        var totalPriceOfReservationsByGuest = reservations
            .Where(reservation => reservation.User.UserTypeId == 1)
            .Where(reservation => !reservation.IsCancelled)
            .Sum(reservation => reservation.TotalReservationPrice);

        var totalPriceOfReservationsByEmployee = reservations
            .Where(reservation => reservation.User.UserTypeId == 3)
            .Where(reservation => !reservation.IsCancelled)
            .Sum(reservation => reservation.TotalReservationPrice);

        var totalNumberOfBookingCancellation = reservations.Count(reservation => reservation.IsCancelled);

        var totalPriceOfBookingCancellation = reservations
            .Where(reservation => reservation.IsCancelled)
            .Sum(reservation => reservation.TotalReservationPrice);

        var totalPriceOfReservations = reservations
            .Where(reservation => !reservation.IsCancelled)
            .Sum(reservation => reservation.TotalReservationPrice);


        return new DashboardDailyOverviewResponseDto(
            numberOfAdults,
            numberOfChild,
            totalAdultsPrice,
            totalChildPrice,
            numberOfAdditionalServices,
            totalPriceOfAdditionalServices,
            numberOfBookingByCompany,
            numberOfBookingByGuest,
            numberOfBookingByEmployee,
            totalPriceOfReservationsByCompany,
            totalPriceOfReservationsByGuest,
            totalPriceOfReservationsByEmployee,
            totalNumberOfBookingCancellation,
            totalPriceOfBookingCancellation,
            totalPriceOfReservations
        );
    }

    public async Task<PaginatedResponseDto<DashboardReservationOverviewResponseDto>> GetReservationsOverviewAsync(
        DateTime date,
        int page)
    {
        var reservations = await _dbContext.Reservations
            .Include(r => r.ReservationAdditionalServices)
            .Include(r => r.Program)
            .Include(r => r.User)
            .ThenInclude(user => user.UserType)
            .AsSplitQuery()
            .Where(r => r.BookingDate.Date == date.Date)
            .OrderBy(p => p.Id)
            .Skip((page - 1) * AppUtils.NumberOfReservationsPerPage)
            .Take(AppUtils.NumberOfReservationsPerPage)
            .ToListAsync();

        var reservationsCount = _dbContext.Reservations.Count(r => r.BookingDate.Date == date.Date);
        var pages = (int)Math.Ceiling((double)reservationsCount / AppUtils.NumberOfReservationsPerPage);


        var reservationsDtos = reservations.Select(reservation =>
        {
            var clientName = reservation.User.FullName;
            var clientType = reservation.User.UserType.Name;

            var programName = reservation.Program.Name;

            var bookingDate = reservation.BookingDate;

            var bookedOn = reservation.BookedOn;

            var hasAdditionalServices = reservation.ReservationAdditionalServices is { Count: > 0 };

            var numberOfAdults = reservation.NumberOfAdults;
            var numberOfChilds = reservation.NumberOfChild;

            var isCancelled = reservation.IsCancelled;

            var attendanceStatus = reservation.AttendanceStatus;
            var isMissed = reservation.IsMissed;

            return new DashboardReservationOverviewResponseDto(
                reservation.Id,
                clientName,
                clientType,
                programName ?? "NA",
                bookingDate,
                bookedOn,
                hasAdditionalServices,
                numberOfAdults,
                numberOfChilds,
                isCancelled,
                attendanceStatus.ToString(),
                isMissed
            );
        }).ToList();


        return new PaginatedResponseDto<DashboardReservationOverviewResponseDto>
        {
            Items = reservationsDtos,
            Pages = pages,
            CurrentPage = page
        };
    }

    public async Task<DashboardReservationDetailsResponseDto> GetReservationDetailsByIdAsync(int id)
    {
        var reservation = await _dbContext.Reservations
            .Include(r => r.ReservationAdditionalServices)
            .Include(r => r.Program)
            .Include(r => r.ReservationPersonsDetails)
            .Include(r => r.User)
            .ThenInclude(user => user.UserType)
            .AsSplitQuery()
            .FirstOrDefaultAsync(r => r.Id == id);

        if (reservation is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Reservation with ID : {id}");
        }

        var clientName = reservation.User.FullName;
        var clientPhone = reservation.User.PhoneNumber;
        var clientEmail = reservation.User.Email;
        var clientType = reservation.User.UserType.Name;

        var persons = reservation.ReservationPersonsDetails?.ToList() ?? new List<ReservationPersonDetails>();

        var bookingDate = reservation.BookingDate;

        var bookedOn = reservation.BookedOn;

        var program = reservation.Program.Name;

        var services = reservation.ReservationAdditionalServices?.Select(service => new AdditionalServiceResponseDto(
            service.AdditionalServiceId,
            service.Name,
            "NA",
            service.PricePerChild,
            service.PricePerAdult
        )).ToList() ?? new List<AdditionalServiceResponseDto>();

        return new DashboardReservationDetailsResponseDto(
            reservation.Id,
            program ?? "NA",
            clientName,
            clientPhone,
            clientEmail,
            clientType,
            persons,
            bookingDate,
            bookedOn,
            reservation.IsCancelled,
            reservation.AttendanceStatus.ToString(),
            reservation.IsMissed,
            reservation.TotalProgramPrice,
            reservation.TotalAdditionalServicesPrice ?? 0.0,
            reservation.TotalReservationPrice,
            reservation.NumberOfAdults,
            reservation.NumberOfChild,
            services
        );
    }

    public async Task<DashboardAuthModelResponseDto> RegisterOnDashboardAsync(DashboardRegisterRequestDto dto)
    {
        if (await _userManager.Users.AnyAsync(user => user.Email == dto.Email))
        {
            throw new CustomExceptionWithStatusCode(400, "There is already user with that Email !!");
        }


        var appUser = new ApplicationUser
        {
            UserName = dto.Email.Replace(" ", ""),
            Email = dto.Email,
            FullName = dto.Email,
            CultureTypeId = 1,
            UserTypeId = (int)dto.UserType
        };

        var role = dto.UserType == DashboardRegisterOption.Admin
            ? "admin"
            : "user";

        var userType = await _dbContext.UserTypes.FirstOrDefaultAsync(type => type.Id == (int)dto.UserType);

        var roleExists = await _roleManager.RoleExistsAsync(role);
        if (!roleExists)
        {
            await _roleManager.CreateAsync(new IdentityRole<int>(role));
        }

        var result = await _userManager.CreateAsync(appUser, dto.Password);

        if (!result.Succeeded)
        {
            throw new CustomExceptionWithStatusCode(
                400,
                $"Something went wrong {result.Errors.First().Description} , please try again !!"
            );
        }

        await _userManager.AddToRoleAsync(appUser, role);

        var jwtToken = await CreateJwtToken(appUser);

        await _userManager.UpdateAsync(appUser);

        return new DashboardAuthModelResponseDto(
            appUser.Id,
            appUser.Email,
            new JwtSecurityTokenHandler().WriteToken(jwtToken),
            jwtToken.ValidTo,
            userType?.Name ?? null
        );
    }

    public async Task<DashboardAuthModelResponseDto> LoginAsync(DashboardLoginRequestDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);

        if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
        {
            throw new CustomExceptionWithStatusCode(404, "Not Found User or not correct password !!");
        }


        var jwtToken = await CreateJwtToken(user);
        var tokenString = new JwtSecurityTokenHandler().WriteToken(jwtToken);
        var tokenExpirationDate = jwtToken.ValidTo;
        var userType = await _dbContext.UserTypes.FirstOrDefaultAsync(type => type.Id == user.UserTypeId);

        return new DashboardAuthModelResponseDto(
            user.Id,
            user.Email,
            tokenString,
            tokenExpirationDate,
            userType?.Name ?? null
        );
    }

    public async Task<List<DailyStaticsResponseDto>> GetMonthlyReservationsStatisticsAsync(int year,
        int month)
    {
        // Ensure valid month and year
        if (month < 1 || month > 12 || year < 1)
        {
            throw new CustomExceptionWithStatusCode(400,
                "Invalid month or year.");
        }

        // Calculate the first and last days of the specified month
        var from = new DateTime(year, month, 1);
        var to = from.AddMonths(1).AddDays(-1);


        // Create a sequence of all days in the month
        var allDaysInMonth = Enumerable.Range(1, DateTime.DaysInMonth(year, month))
            .Select(day => new DateTime(year, month, day))
            .ToList();

        // Filter reservations based on the 'from' and 'to' dates
        var reservations = await _dbContext.Reservations
            .Where(r =>
                r.BookingDate.Date >= from.Date
                && r.BookingDate <= to.Date
                && r.AttendanceStatus == AttendanceStatus.Attended)
            .ToListAsync();

        // Calculate daily statistics
        var dailyStatistics = CalculateDailyStatistics(allDaysInMonth, reservations);

        return dailyStatistics;
    }

    public async Task<AdditionalService> AddAdditionalServiceAsync(DashboardAdditionalServiceRequestDto dto)
    {
        var additionalService = new AdditionalService
        {
            Name = dto.Name,
            Description = dto.Description,
            AdditionalServicePrices = dto.Prices.Select(price => new AdditionalServicePrice
            {
                UserTypeId = price.UserTypeId,
                PricePerChild = price.PricePerChild,
                PricePerAdult = price.PricePerAdult
            }).ToList()
        };

        additionalService = (await _dbContext.AdditionalServices.AddAsync(additionalService)).Entity;
        await _dbContext.SaveChangesAsync();

        return additionalService;
    }

    public async Task<PaginatedResponseDto<AdditionalService>> GetAllAdditionalServicesWithAllPricesAsync(int page)
    {
        var services = await _dbContext
            .AdditionalServices
            .Include(service => service.AdditionalServicePrices)
            .OrderBy(service => service.Id)
            .Skip((page - 1) * AppUtils.NumberOfAdditionalServicesPerPage)
            .Take(AppUtils.NumberOfAdditionalServicesPerPage)
            .ToListAsync();

        var servicesCount = _dbContext.AdditionalServices.Count();
        var pages = (int)Math.Ceiling((double)servicesCount / AppUtils.NumberOfAdditionalServicesPerPage);

        return new PaginatedResponseDto<AdditionalService>
        {
            Items = services,
            Pages = pages,
            CurrentPage = page
        };
    }

    public async Task<AdditionalService> DeleteAdditionalServiceByIdAsync(int id)
    {
        var service = await _dbContext.AdditionalServices.FindAsync(id);
        if (service is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Additional Service with ID : {id}");
        }

        var isServiceIncludedInReservations =
            await _dbContext.ReservationAdditionalServices
                .AnyAsync(r => r.AdditionalServiceId == id);

        if (isServiceIncludedInReservations)
        {
            throw new CustomExceptionWithStatusCode(400,
                "This Service can not be deleted because it is included in reservations !!");
        }

        _dbContext.AdditionalServices.Remove(service);
        await _dbContext.SaveChangesAsync();

        return service;
    }

    public async Task<AdditionalService> UpdateAdditionalServiceByIdAsync(int id,
        DashboardAdditionalServiceRequestDto dto)
    {
        var service = await _dbContext.AdditionalServices
            .Include(additionalService => additionalService.AdditionalServicePrices)
            .FirstOrDefaultAsync(service => service.Id == id);

        if (service is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Additional Service with ID : {id}");
        }


        var newPrices = dto.Prices.Select(p => new AdditionalServicePrice
        {
            UserTypeId = p.UserTypeId,
            PricePerChild = p.PricePerChild,
            PricePerAdult = p.PricePerAdult,
            ServiceId = service.Id
        }).ToList();

        service.Name = dto.Name;
        service.Description = dto.Description;
        _dbContext.AdditionalServicePrices.RemoveRange(service.AdditionalServicePrices);
        service.AdditionalServicePrices = newPrices;

        _dbContext.AdditionalServices.Update(service);
        await _dbContext.SaveChangesAsync();

        return service;
    }

    public async Task<Program> AddProgramAsync(DashboardProgramRequestDto dto)
    {
        var program = new Program
        {
            Description = dto.Description,
            Name = dto.Name,
            Location = dto.Location,
            InternalNotes = dto.InternalNotes,
            MaxCapacity = dto.MaxCapacity,
            SpecialRequirements = dto.SpecialRequirements,
            DaysBeforeCancellation = dto.DaysBeforeCancellation,
            DurationInHours = dto.DurationInHours,
            ProgramNotes = dto.ProgramNotes.Select(note => new ProgramNote
            {
                Note = note
            }).ToList(),
            ProgramPlans = dto.ProgramPlans.Select(p => new ProgramPlan
            {
                Description = p.Description,
                Time = p.Time
            }).ToList(),
            ProgramPrices = dto.ProgramPrices.Select(p => new ProgramPrice
            {
                PricePerAdult = p.PricePerAdult,
                PricePerChild = p.PricePerChild,
                UserTypeId = p.UserTypeId,
                FromDate = p.FromDate,
                ToDate = p.ToDate
            }).ToList(),
            ProgramExcludedDetails = dto.ProgramExcludedDetails.Select(details => new ProgramExcluded
            {
                Description = details
            }).ToList(),
            ProgramIncludedDetails = dto.ProgramIncludedDetails.Select(details => new ProgramIncluded
            {
                Description = details
            }).ToList()
        };

        program = (await _dbContext.Programs.AddAsync(program)).Entity;
        await _dbContext.SaveChangesAsync();

        return program;
    }

    public async Task<PaginatedResponseDto<Program>> GetAllProgramsAsync(int page)
    {
        var programs = await _dbContext.Programs
            .Include(p => p.ProgramNotes)
            .Include(p => p.ProgramImages)
            .Include(p => p.ProgramPlans)
            .Include(p => p.ProgramPrices)
            .Include(p => p.ProgramExcludedDetails)
            .Include(p => p.ProgramIncludedDetails).AsSplitQuery()
            .OrderBy(p => p.Id)
            .Skip((page - 1) * AppUtils.NumberOfProgramsPerPage)
            .Take(AppUtils.NumberOfProgramsPerPage)
            .ToListAsync();

        var programsCount = _dbContext.Programs.Count();
        var pages = (int)Math.Ceiling((double)programsCount / AppUtils.NumberOfProgramsPerPage);

        return new PaginatedResponseDto<Program>
        {
            Items = programs,
            Pages = pages,
            CurrentPage = page
        };
    }

    public async Task<bool> DeleteProgramByIdAsync(int id)
    {
        var program = await _dbContext.Programs.FindAsync(id);

        if (program is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Program with ID : {id} ");
        }

        var isIncludedProgramInReservation = await _dbContext.Reservations.AnyAsync(r => r.ProgramId == id);
        if (isIncludedProgramInReservation)
        {
            throw new CustomExceptionWithStatusCode(400,
                "This Program can not be deleted because it is included in reservations !!");
        }

        _dbContext.Programs.Remove(program);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<Program> UpdateProgramByIdAsync(int id, DashboardProgramRequestDto dto)
    {
        var program = await _dbContext.Programs
            .Include(p => p.ProgramNotes)
            .Include(p => p.ProgramImages)
            .Include(p => p.ProgramPlans)
            .Include(p => p.ProgramPrices)
            .Include(p => p.ProgramExcludedDetails)
            .Include(p => p.ProgramIncludedDetails).AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (program is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Program with ID : {id}");
        }


        program.Description = dto.Description;
        program.Location = dto.Location;
        program.Name = dto.Name;
        program.InternalNotes = dto.InternalNotes;
        program.MaxCapacity = dto.MaxCapacity;
        program.SpecialRequirements = dto.SpecialRequirements;
        program.DaysBeforeCancellation = dto.DaysBeforeCancellation;
        program.DurationInHours = dto.DurationInHours;

        if (program.ProgramNotes != null)
            _dbContext.ProgramNotes.RemoveRange(program.ProgramNotes);

        if (program.ProgramPlans != null)
            _dbContext.ProgramPlans.RemoveRange(program.ProgramPlans);

        if (program.ProgramPrices != null)
            _dbContext.ProgramPrices.RemoveRange(program.ProgramPrices);

        if (program.ProgramExcludedDetails != null)
            _dbContext.ProgramExcludedDetails.RemoveRange(program.ProgramExcludedDetails);

        if (program.ProgramIncludedDetails != null)
            _dbContext.ProgramIncludedDetails.RemoveRange(program.ProgramIncludedDetails);


        program.ProgramNotes = dto.ProgramNotes.Select(n => new ProgramNote
        {
            Note = n,
            ProgramId = program.Id
        }).ToList();

        program.ProgramPlans = dto.ProgramPlans.Select(plan => new ProgramPlan
        {
            Description = plan.Description,
            Time = plan.Time,
            ProgramId = program.Id
        }).ToList();

        program.ProgramPrices = dto.ProgramPrices.Select(price => new ProgramPrice
        {
            ToDate = price.ToDate,
            UserTypeId = price.UserTypeId,
            PricePerChild = price.PricePerChild,
            PricePerAdult = price.PricePerAdult,
            FromDate = price.FromDate,
            ProgramId = program.Id
        }).ToList();

        program.ProgramExcludedDetails = dto.ProgramExcludedDetails.Select(detail => new ProgramExcluded
        {
            Description = detail,
            ProgramId = program.Id
        }).ToList();

        program.ProgramIncludedDetails = dto.ProgramIncludedDetails.Select(detail => new ProgramIncluded
        {
            Description = detail,
            ProgramId = program.Id
        }).ToList();


        _dbContext.Programs.Update(program);
        await _dbContext.SaveChangesAsync();

        return program;
    }

    public async Task<List<UserType>> GetAllUserTypesAsync()
    {
        return await _dbContext.UserTypes.ToListAsync();
    }

    public async Task<DashboardProgramResponseDto> GetProgramByIdAsync(int id)
    {
        var program = await _dbContext.Programs
            .Include(p => p.ProgramNotes)
            .Include(p => p.ProgramImages)
            .Include(p => p.ProgramPlans)
            .Include(p => p.ProgramPrices)
            .Include(p => p.ProgramExcludedDetails)
            .Include(p => p.ProgramIncludedDetails).AsSplitQuery()
            .FirstOrDefaultAsync(p => p.Id == id);

        if (program is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Program with ID : {id}");
        }

        return new DashboardProgramResponseDto(
            program.Id,
            program.Name,
            program.Description,
            program.InternalNotes,
            program.SpecialRequirements,
            program.MaxCapacity,
            program.Location,
            program.DurationInHours,
            program.DaysBeforeCancellation,
            program.ProgramPlans?.Select(plan => new ProgramPlanResponseDto(
                plan.Id,
                plan.Time,
                plan.Description
            )).ToList() ??
            new List<ProgramPlanResponseDto>(),
            program.ProgramPrices?.Select(price => new ProgramPriceResponseDto(
                price.Id,
                price.PricePerChild,
                price.PricePerAdult,
                price.FromDate,
                price.ToDate,
                price.UserTypeId
            )).ToList() ??
            new List<ProgramPriceResponseDto>(),
            program.ProgramIncludedDetails?.Select(included => included.Description).ToList() ?? new List<string>(),
            program.ProgramExcludedDetails?.Select(exc => exc.Description).ToList() ?? new List<string>(),
            program.ProgramNotes?.Select(note => note.Note).ToList() ?? new List<string>()
        );
    }

    public async Task<AdditionalService> GetAdditionalServiceByIdAsync(int id)
    {
        var service = await _dbContext.AdditionalServices
            .Include(service => service.AdditionalServicePrices)
            .FirstOrDefaultAsync(service => service.Id == id);

        if (service is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Service with ID : {id}");
        }

        return service;
    }

    public async Task<PaginatedResponseDto<ProfileResponseDto>> GetAllUsersByUserTypeIdAsync(int? userTypeId, int page)
    {
        if (userTypeId != null && !await _dbContext.UserTypes.AnyAsync(type => type.Id == userTypeId))
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found User Type ID : {userTypeId}");
        }

        var users = await _userManager.Users
            .Where(user =>
                userTypeId == null || user.UserTypeId == userTypeId)
            .Select(user => new ProfileResponseDto
            {
                Id = user.Id,
                Email = user.Email ?? "NA",
                UserTypeId = user.UserTypeId,
                FullName = user.FullName,
                UserName = user.UserName ?? "NA",
                Phone = user.PhoneNumber ?? "NA",
                PhotoUrl = user.PhotoUrl
            })
            .OrderBy(user => user.Id)
            .Skip((page - 1) * AppUtils.NumberOfUsersPerPage)
            .Take(AppUtils.NumberOfUsersPerPage)
            .ToListAsync();

        var usersCount = _dbContext.Users.Count(user =>
            userTypeId == null || user.UserTypeId == userTypeId);
        var pages = (int)Math.Ceiling((double)usersCount / AppUtils.NumberOfUsersPerPage);

        return new PaginatedResponseDto<ProfileResponseDto>
        {
            Items = users,
            Pages = pages,
            CurrentPage = page
        };
    }

    public async Task<PaginatedResponseDto<DashboardReservationOverviewResponseDto>> GetWeeklyReservationsOverviewAsync(
        DateTime from, DateTime to, int page)
    {
        if (from.Date >= to.Date)
        {
            throw new CustomExceptionWithStatusCode(400, "-from- Date must be before -to- Date !!");
        }

        var reservations = await _dbContext.Reservations
            .Include(r => r.ReservationAdditionalServices)
            .Include(r => r.Program)
            .Include(r => r.User)
            .ThenInclude(user => user.UserType)
            .AsSplitQuery()
            .Where(r => r.BookingDate.Date >= from.Date && r.BookingDate.Date <= to.Date)
            .OrderBy(p => p.BookingDate)
            .Skip((page - 1) * AppUtils.NumberOfReservationsPerPage)
            .Take(AppUtils.NumberOfReservationsPerPage)
            .ToListAsync();

        var reservationsCount =
            _dbContext.Reservations.Count(r =>
                r.BookingDate.Date >= from.Date
                && r.BookingDate.Date <= to.Date);

        var pages = (int)Math.Ceiling((double)reservationsCount / AppUtils.NumberOfReservationsPerPage);


        var reservationsDtos = reservations.Select(reservation =>
        {
            var clientName = reservation.User.FullName;
            var clientType = reservation.User.UserType.Name;

            var programName = reservation.Program.Name;

            var bookingDate = reservation.BookingDate;

            var bookedOn = reservation.BookedOn;

            var hasAdditionalServices = reservation.ReservationAdditionalServices is { Count: > 0 };

            var numberOfAdults = reservation.NumberOfAdults;
            var numberOfChilds = reservation.NumberOfChild;

            var isCancelled = reservation.IsCancelled;

            return new DashboardReservationOverviewResponseDto(
                reservation.Id,
                clientName,
                clientType,
                programName ?? "NA",
                bookingDate,
                bookedOn,
                hasAdditionalServices,
                numberOfAdults,
                numberOfChilds,
                isCancelled,
                reservation.AttendanceStatus.ToString(),
                reservation.IsMissed
            );
        }).ToList();


        return new PaginatedResponseDto<DashboardReservationOverviewResponseDto>
        {
            Items = reservationsDtos,
            Pages = pages,
            CurrentPage = page
        };
    }

    public async Task<ProgramImage> DeleteProgramImageByIdAsync(int id)
    {
        var img = await _dbContext.ProgramImages.FindAsync(id);
        if (img is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Program Image with ID : {id}");
        }

        _dbContext.ProgramImages.Remove(img);
        await _dbContext.SaveChangesAsync();

        return img;
    }

    public async Task<bool> ApplyReservationAttendanceAsync(int id)
    {
        var reservation = await _dbContext.Reservations.FindAsync(id);
        if (reservation is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Reservation with ID : {id}");
        }

        reservation.AttendanceStatus = AttendanceStatus.Attended;
        _dbContext.Reservations.Update(reservation);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    private static List<DailyStaticsResponseDto> CalculateDailyStatistics(List<DateTime> allDaysInMonth,
        IReadOnlyCollection<Reservation> reservations)
    {
        // Group reservations by date and count attended reservations for each day
        var dailyStatistics = new List<DailyStaticsResponseDto>();

        allDaysInMonth.ForEach(day =>
        {
            var numberOfAttendanceForDay = reservations.Count(r => r.BookingDate.Date == day.Date);
            dailyStatistics.Add(new DailyStaticsResponseDto(day.Date, numberOfAttendanceForDay));
        });

        return dailyStatistics;
    }

    private async Task<SecurityToken> CreateJwtToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);
        var roleClaims = roles.Select(role => new Claim("roles", role)).ToList();

        var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim("uid", user.Id.ToString())
            }
            .Union(userClaims)
            .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
            issuer: _jwt.Issuer,
            audience: _jwt.Audience,
            claims: claims,
            expires: DateTime.Now.AddDays(_jwt.DurationInDays),
            signingCredentials: signingCredentials);

        return jwtSecurityToken;
    }
}