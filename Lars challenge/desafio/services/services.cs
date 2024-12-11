public class ManejoEventoReserva : BackgroundService
{
    private readonly IEventQueue _queue;
    private readonly IEventService _eventService;

    public ManejoEventoReserva(IEventQueue queue, IEventService eventService)
    {
        _queue = queue;
        _eventService = eventService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            var reservationEvent = await _queue.DequeueAsync(stoppingToken);
            await _eventService.SendConfirmationEmail(reservationEvent);
        }
    }
}