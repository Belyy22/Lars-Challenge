public class Reservacion
{
    public int Id { get; set; }
    public string NumeroDeHabitacion { get; set; }
    public DateTime FechaIngreso { get; set; }
    public DateTime FechaSalida { get; set; }
    public string NombreCliente { get; set; }
    public byte[] RowVersion { get; set; }
}