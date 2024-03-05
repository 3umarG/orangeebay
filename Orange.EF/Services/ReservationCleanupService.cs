using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Orange.EF.Services;

public class ReservationCleanupService : BackgroundService
{
    private readonly TimeSpan _period = TimeSpan.FromHours(12);
    private int _executionCount;
    private readonly IServiceScopeFactory _factory;
    private readonly ILogger<ReservationCleanupService> _logger;

    public ReservationCleanupService(IServiceScopeFactory factory, ILogger<ReservationCleanupService> logger)
    {
        _factory = factory;
        _logger = logger;
    }


    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        // await Task.Delay(5000, stoppingToken);
        // var numberOfDeletedReservations = await CleanupExpiredReservationsAsync(stoppingToken);
        // _executionCount++;
        // _logger.LogWarning(
        //     "Executed ReservationCleanupService - Count: #{count} --- Number of deleted reservations : #{reservationsCount}",
        //     _executionCount, numberOfDeletedReservations);

        using var timer = new PeriodicTimer(_period);
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var numberOfDeletedReservations = await CleanupExpiredReservationsAsync(stoppingToken);
                _executionCount++;
                _logger.LogWarning(
                    "Executed ReservationCleanupService - Count: #{count} --- Number of deleted reservations : #{reservationsCount}",
                    _executionCount, numberOfDeletedReservations);
                await timer.WaitForNextTickAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    "Failed to execute ReservationCleanupService with exception message #{message}. Good luck next round!",
                    ex.Message);
            }
        }
    }

    private async Task<int> CleanupExpiredReservationsAsync(CancellationToken stoppingToken)
    {
        await using var asyncScope = _factory.CreateAsyncScope();
        var dbContext = asyncScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        var reservationsToRemove = dbContext.Reservations
            .Where(r =>
                !r.IsPaid
                && !r.IsCancelled
                && r.CancellationDeadlineDate != null
                && r.CancellationDeadlineDate.Value.Date < DateTime.Now.Date
            )
            .ToList();

        dbContext.Reservations.RemoveRange(reservationsToRemove);
        await dbContext.SaveChangesAsync(stoppingToken);
        Log.Information($"Removed : {reservationsToRemove.Count} Reservations");

        return reservationsToRemove.Count;
    }
}