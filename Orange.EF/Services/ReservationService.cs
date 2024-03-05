using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Orange_Bay.DTOs.Reservation;
using Orange_Bay.Exceptions;
using Orange_Bay.Interfaces.Services;
using Orange_Bay.Models.AdditionalServices;
using Orange_Bay.Models.Auth;
using Orange_Bay.Models.Booking;
using Orange_Bay.Models.Programs;
using Orange_Bay.Utils;

namespace Orange.EF.Services;

public class ReservationService : IReservationService
{
    private readonly IAuthService _authService;
    private readonly ApplicationDbContext _dbContext;

    public ReservationService(IAuthService authService, ApplicationDbContext dbContext)
    {
        _authService = authService;
        _dbContext = dbContext;
    }

    public async Task<ReservationResponseDto> AddReservationAsync(AddReservationRequestDto dto)
    {
        var user = await FindUserOrThrowNotFound(dto.UserId);

        var reservation = await SaveReservationToDb(dto);

        await AddAdditionalServicesToReservation(dto.AdditionalServices, user, reservation);

        await AddPersonsDetailsToReservation(dto, reservation);

        var finalReservationInfo = await FindReservationById(reservation.Id);

        return BuildReservationResponseDto(finalReservationInfo!);
    }

    private async Task AddPersonsDetailsToReservation(AddReservationRequestDto dto, Reservation reservation)
    {
        var reservationPersonsDetails = GenerateReservationPersonsDetails(dto, reservation);
        await _dbContext.ReservationPersonsDetails.AddRangeAsync(reservationPersonsDetails);
        await _dbContext.SaveChangesAsync();
    }

    private static List<ReservationPersonDetails> GenerateReservationPersonsDetails(AddReservationRequestDto dto,
        Reservation reservation)
    {
        var reservationPersonsDetails = dto.Persons.Select(p =>
        {
            var person = new ReservationPersonDetails
            {
                ReservationId = reservation.Id,
                Type = p.Type == 0 ? PersonType.Child : PersonType.Adult,
                Name = p.Name,
                Email = p.Email,
                Phone = p.Phone
            };

            return person;
        }).ToList();
        return reservationPersonsDetails;
    }

    private async Task<Reservation> FindReservationById(int reservationId)
    {
        var reservation = await _dbContext.Reservations
            .Include(r => r.ReservationAdditionalServices)
            .Include(r => r.ReservationPersonsDetails)
            .AsSplitQuery()
            .FirstOrDefaultAsync(r => r.Id == reservationId);

        if (reservation is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Reservation with id : {reservationId}");
        }

        return reservation;
    }

    private async Task AddAdditionalServicesToReservation(
        IEnumerable<AdditionalServiceRequestDto> services,
        ApplicationUser user,
        Reservation reservation
    )
    {
        try
        {
            var reservationAdditionalServices =
                services.Select(async s =>
                        await BuildAdditionalServiceForReservation(s, user.UserTypeId, reservation.Id))
                    .Select(task => task.Result);

            // var reservationAdditionalServicesResults = await Task.WhenAll(reservationAdditionalServicesTasks);
            await _dbContext.ReservationAdditionalServices.AddRangeAsync(reservationAdditionalServices);
            await _dbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            if (e.InnerException is CustomExceptionWithStatusCode)
            {
                throw e.InnerException;
            }
        }
    }

    private async Task<Reservation> SaveReservationToDb(AddReservationRequestDto dto)
    {
        var reservation = (await _dbContext.Reservations.AddAsync(await BuildReservation(dto))).Entity;
        await _dbContext.SaveChangesAsync();
        return reservation;
    }

    private async Task<ReservationAdditionalService> BuildAdditionalServiceForReservation(
        AdditionalServiceRequestDto serviceDto,
        int userTypeId,
        int reservationId)
    {
        // Find service by ID
        var service = await FindServiceByIdOrThrowNotFound(serviceDto.ServiceId, userTypeId);

        // Extract Price (Child - Adult)
        var servicePricePerChild = FindServicePricePerChildForUserType(userTypeId, service);
        var servicePricePerAdult = FindSerServicePricePerAdultForUserType(userTypeId, service);

        return new ReservationAdditionalService
        {
            Name = service.Name,
            PricePerChild = (double)servicePricePerChild!,
            NumberOfChild = serviceDto.NumberOfChild,
            PricePerAdult = (double)servicePricePerAdult!,
            NumberOfAdults = serviceDto.NumberOfAdults,
            ReservationId = reservationId,
            AdditionalServiceId = serviceDto.ServiceId
        };
    }

    private static double? FindSerServicePricePerAdultForUserType(int userTypeId, AdditionalService service)
    {
        return service.AdditionalServicePrices?
            .FirstOrDefault(price => price.UserTypeId == userTypeId)?.PricePerAdult;
    }

    private static double? FindServicePricePerChildForUserType(int userTypeId, AdditionalService service)
    {
        return service.AdditionalServicePrices?
            .FirstOrDefault(price => price.UserTypeId == userTypeId)?.PricePerChild;
    }

    private async Task<AdditionalService> FindServiceByIdOrThrowNotFound(int serviceId, int userTypeId)
    {
        var service = await _dbContext.AdditionalServices
            .Include(ser =>
                ser.AdditionalServicePrices.Where(price => price.UserTypeId == userTypeId))
            .FirstOrDefaultAsync(ser => ser.Id == serviceId);
        if (service is null)
        {
            throw new CustomExceptionWithStatusCode(404,
                $"Not Found Additional Service with id : {serviceId}");
        }

        if (service.AdditionalServicePrices is null || service.AdditionalServicePrices.Count == 0)
        {
            throw new CustomExceptionWithStatusCode(404,
                "There is no Pricing Plan for this Additional Services for you yet ..!!");
        }

        return service;
    }

    private async Task<Reservation> BuildReservation(AddReservationRequestDto dto)
    {
        var user = await FindUserOrThrowNotFound(dto.UserId);

        var program = await FindProgramWithPriceRelatedToUserTypeIdOrThrowNotFound(dto.ProgramId, user.UserTypeId);

        ValidateBookingDate(dto, program);

        // Validate User inputs
        ValidateUserInputs(dto);

        // Validate Maximum Capacity of Program
        ValidateMaximumCapacityOfProgram(dto.NumberOfChild + dto.NumberOfAdults, program.MaxCapacity);

        // Extract Program Price (Child - Adult)
        var programPricePerChild = FindProgramPricePerChildForUserTypeId(program, user.UserTypeId);
        var programPricePerAdult = FindProgramPricePerAdultForUserTypeId(program, user.UserTypeId);

        return new Reservation
        {
            BookingDate = dto.BookingDate,
            IsPaid = false,
            ProgramId = dto.ProgramId,
            UserId = dto.UserId,
            NumberOfAdults = dto.NumberOfAdults,
            NumberOfChild = dto.NumberOfChild,
            PricePerAdult = (double)programPricePerAdult!,
            PricePerChild = (double)programPricePerChild!,
            CancellationDeadlineDate =
                dto.BookingDate.Subtract(TimeSpan.FromDays((double)program.DaysBeforeCancellation!)),
            IsCancelled = false,
            BookedOn = DateTime.Today
        };
    }

    private static void ValidateBookingDate(AddReservationRequestDto dto, Program program)
    {
        if (dto.BookingDate < program.ProgramPrices?.First().FromDate ||
            dto.BookingDate > program.ProgramPrices?.First().ToDate)
        {
            throw new CustomExceptionWithStatusCode(400,
                $"Your booking date is not included in the program duration from : {program.ProgramPrices?.First().FromDate} to : {program.ProgramPrices?.First().ToDate}");
        }
    }

    private static double? FindProgramPricePerAdultForUserTypeId(Program program, int userTypeId)
    {
        return program.ProgramPrices
            ?.FirstOrDefault(programPrice => programPrice.UserTypeId == userTypeId)?.PricePerAdult;
    }

    private static double? FindProgramPricePerChildForUserTypeId(Program program, int userTypeId)
    {
        return program.ProgramPrices
            ?.FirstOrDefault(programPrice => programPrice.UserTypeId == userTypeId)?.PricePerChild;
    }

    private static void ValidateMaximumCapacityOfProgram(int givenNumber, int programMaximumCapacity)
    {
        if (givenNumber > programMaximumCapacity)
        {
            throw new CustomExceptionWithStatusCode(400, $"Program Capacity is : {programMaximumCapacity} only !!");
        }
    }

    private static void ValidateUserInputs(AddReservationRequestDto dto)
    {
        if (NotValidDtoMembers(dto))
        {
            throw new CustomExceptionWithStatusCode(400, "Check your input values !!");
        }
    }

    private async Task<Program> FindProgramWithPriceRelatedToUserTypeIdOrThrowNotFound(int programId,
        int userTypeId)
    {
        // Validate Program ID
        var program = await _dbContext.Programs
            .Include(program => program.ProgramImages)
            .Include(
                program => program.ProgramPrices!.Where(programPrice => programPrice.UserTypeId == userTypeId))
            .AsSplitQuery()
            .FirstOrDefaultAsync(program => program.Id == programId);

        if (program is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Program with id : {programId}");
        }

        // For validating if the program doesn't have any pricing plan for the user type id
        // To avoid null reference exception
        if (program.ProgramPrices is null || program.ProgramPrices.Count == 0)
        {
            throw new CustomExceptionWithStatusCode(404, "There is no Pricing Plan for this program for you yet ..!!");
        }

        return program;
    }

    private async Task<ApplicationUser> FindUserOrThrowNotFound(int userId)
    {
        // Validate User ID
        var user = await _authService.FindUserByIdAsync(userId);
        if (user is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found User with id : {userId}");
        }

        return user;
    }

    public async Task<List<ReservationResponseDto>> GetAllReservationsByUserId(ReservationStatus reservationStatus,
        int userId)
    {
        if (!await _authService.AnyUserWithIdAsync(userId))
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found User with id {userId}");
        }


        Func<Reservation, bool> predicateForReservationStatus =
            reservationStatus == ReservationStatus.Past
                ? PredicateForPastReservation
                : PredicateForUpcomingReservation;


        var reservations = _dbContext.Reservations
            .Include(r => r.ReservationAdditionalServices)
            .Include(r => r.Program)
            .ThenInclude(program => program.ProgramImages)
            .Include(r => r.ReservationPersonsDetails)
            .AsSplitQuery()
            .Where(r => r.UserId == userId).AsEnumerable()
            .Where(predicateForReservationStatus)
            .ToList();

        return reservations.Select(BuildReservationResponseDto).ToList();
    }

    private static ReservationResponseDto BuildReservationResponseDto(Reservation reservation)
    {
        return new ReservationResponseDto(
            reservation.Id,
            reservation.BookingDate,
            reservation.BookedOn,
            reservation.CancellationDeadlineDate,
            reservation.CanBeCancelOrEdit,
            reservation.TotalReservationPrice,
            reservation.IsPaid,
            new ReservationProgramResponseDto(
                reservation.Program.Id,
                reservation.Program.Name!,
                reservation.Program.ProgramImages!.Select(p => p.PhotoUrl).ToList(),
                reservation.NumberOfChild,
                reservation.ChildTotalPrice,
                reservation.NumberOfAdults,
                reservation.AdultTotalPrice
            ),
            reservation.ReservationAdditionalServices
                .Select(s => new ReservationAdditionalServiceResponseDto(
                    s.AdditionalServiceId,
                    s.Name,
                    s.NumberOfChild,
                    s.ChildTotalPrice,
                    s.NumberOfAdults,
                    s.AdultTotalPrice)
                ).ToList(),
            reservation.TotalProgramPrice,
            (double)reservation.TotalAdditionalServicesPrice!,
            (reservation.ReservationPersonsDetails
             ?? new List<ReservationPersonDetails>()).ToList().Select(person =>
                new ReservationPersonDetailsResponseDto(
                    person.Name,
                    person.Email,
                    person.Phone,
                    person.Type.ToString(),
                    person.ReservationId
                )).ToList()
        );
    }

    public async Task<ReservationResponseDto> CancelReservationByIdAsync(int reservationId)
    {
        var reservation = await _dbContext.Reservations
            .Include(r => r.ReservationAdditionalServices)
            .Include(r => r.Program)
            .ThenInclude(program => program.ProgramImages)
            .FirstOrDefaultAsync(r => r.Id == reservationId);

        if (reservation is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Reservation with id : {reservationId}");
        }

        if (!reservation.CanBeCancelOrEdit)
        {
            throw new CustomExceptionWithStatusCode(400, "This Reservation can not be cancelled !!");
        }

        reservation.IsCancelled = true;
        _dbContext.Reservations.Update(reservation);
        await _dbContext.SaveChangesAsync();

        return BuildReservationResponseDto(reservation);
    }

    public async Task<ReservationResponseDto> UpdateReservationAsync(int reservationId, AddReservationRequestDto dto)
    {
        var reservation = await FindReservationById(reservationId);

        var user = await FindUserOrThrowNotFound(dto.UserId);

        var program = await FindProgramWithPriceRelatedToUserTypeIdOrThrowNotFound(dto.ProgramId, user.UserTypeId);

        ValidateUserInputs(dto);

        ValidateMaximumCapacityOfProgram(dto.NumberOfChild + dto.NumberOfAdults, program.MaxCapacity);

        var programPricePerChild = FindProgramPricePerChildForUserTypeId(program, user.UserTypeId);
        var programPricePerAdult = FindProgramPricePerAdultForUserTypeId(program, user.UserTypeId);

        reservation.NumberOfAdults = dto.NumberOfAdults;
        reservation.PricePerAdult = (double)programPricePerAdult!;
        reservation.NumberOfChild = dto.NumberOfChild;
        reservation.PricePerChild = (double)programPricePerChild!;
        reservation.BookingDate = dto.BookingDate;
        reservation.CancellationDeadlineDate =
            reservation.BookingDate.Subtract(TimeSpan.FromDays(program.DaysBeforeCancellation ?? 0));
        reservation.ProgramId = program.Id;
        reservation.BookedOn = DateTime.Today;

        _dbContext.Reservations.Update(reservation);

        RemoveAdditionalServicesFromReservation(reservation);

        RemovePersonsDetailsFromReservation(reservation);

        await AddAdditionalServicesToReservation(dto.AdditionalServices, user, reservation);

        await AddPersonsDetailsToReservation(dto, reservation);

        var reservationAfterUpdating = await FindReservationById(reservationId);

        return BuildReservationResponseDto(reservationAfterUpdating);
    }

    public async Task<ReservationPaymentDetails> AddReservationPaymentAsync(AddReservationPaymentRequestDto dto)
    {
        var reservation = await _dbContext.Reservations
            .Include(r => r.ReservationAdditionalServices)
            .FirstOrDefaultAsync(r => r.Id == dto.ReservationId);

        if (reservation is null)
        {
            throw new CustomExceptionWithStatusCode(404, $"Not Found Reservation with ID : {dto.ReservationId}");
        }

        var reservationPaymentDetails = new ReservationPaymentDetails
        {
            ReservationId = dto.ReservationId,
            PayedOn = DateTime.Now.Date,
            TotalPrice = reservation.TotalReservationPrice,
            TransactionId = dto.TransactionId
        };

        await _dbContext.ReservationsPaymentDetails.AddAsync(reservationPaymentDetails);
        reservation.IsPaid = true;
        _dbContext.Reservations.Update(reservation);
        await _dbContext.SaveChangesAsync();

        return reservationPaymentDetails;
    }

    private void RemovePersonsDetailsFromReservation(Reservation reservation)
    {
        if (reservation.ReservationPersonsDetails == null) return;
        foreach (var person in reservation.ReservationPersonsDetails)
        {
            _dbContext.ReservationPersonsDetails.Remove(person);
        }
    }

    private void RemoveAdditionalServicesFromReservation(Reservation reservation)
    {
        foreach (var service in reservation.ReservationAdditionalServices)
        {
            _dbContext.ReservationAdditionalServices.Remove(service);
        }
    }

    private static bool PredicateForPastReservation(Reservation r)
    {
        return r.BookingDate <= DateTime.Today && !r.IsCancelled;
    }

    private static bool PredicateForUpcomingReservation(Reservation r)
    {
        return r.BookingDate > DateTime.Today && !r.IsCancelled;
    }


    private static bool NotValidDtoMembers(AddReservationRequestDto dto)
    {
        return dto.NumberOfAdults < 0 || dto.NumberOfChild < 0 || dto.BookingDate < DateTime.Now ||
               dto.AdditionalServices.Any(service => service.NumberOfChild < 0 || service.NumberOfAdults < 0);
    }
}