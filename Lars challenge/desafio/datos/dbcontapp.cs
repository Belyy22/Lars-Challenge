using Microsoft.EntityFrameworkCore;

public class DbContextApp : DbContext
{
    public DbSet<Reservacion> Reservaciones { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Reservacion>()
            .Property(r => r.RowVersion)
            .IsRowVersion();
    }
}