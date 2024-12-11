public interface IEventQueue
{
    Task<Reservacion> DequeueAsync(CancellationToken cancellationToken);
}