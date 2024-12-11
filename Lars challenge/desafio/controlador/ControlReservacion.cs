using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

[ApiController]
[Route("api/[controller]")]
public class ControlReservacionController : ControllerBase
{
    private readonly DbContextApp _context;
    private readonly IEventService _eventService;

    public ControlReservacionController(DbContextApp context, IEventService eventService)
    {
        _context = context;
        _eventService = eventService;
    }

    [HttpPost("reserve-room")]
    public async Task<IActionResult> ReserveRoom([FromBody] ReservacionDto reservacionDto)
    {
        var reservacion = new Reservacion
        {
            NumeroDeHabitacion = reservacionDto.NumeroDeHabitacion,
            FechaIngreso = reservacionDto.FechaIngreso,
            FechaSalida = reservacionDto.FechaSalida,
            NombreCliente = reservacionDto.NombreCliente
        };

        using var transaccion = await _context.Database.BeginTransactionAsync();

        try
        {
            _context.Reservaciones.Add(reservacion);
            await _context.SaveChangesAsync();

            await _eventService.SendConfirmationEmail(reservacion);

            await transaccion.CommitAsync();

            return Ok(new { Message = "Reserva confirmada." });
        }
        catch (DbUpdateConcurrencyException)
        {
            return Conflict(new { Message = "La habitación ya fue reservada, por favor intente nuevamente." });
        }
        catch (Exception ex)
        {
            await transaccion.RollbackAsync();
            return StatusCode(500, new { Message = ex.Message });
        }
    }
}