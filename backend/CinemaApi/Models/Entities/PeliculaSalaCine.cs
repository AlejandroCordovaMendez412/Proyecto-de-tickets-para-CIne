namespace CinemaApi.Models.Entities;

public class PeliculaSalaCine
{
    public int IdPeliculaSala { get; set; }
    public int IdSalaCine { get; set; }
    public int IdPelicula { get; set; }
    public DateOnly FechaPublicacion { get; set; }
    public DateOnly? FechaFin { get; set; }
    public bool Activo { get; set; } = true;
    public Pelicula Pelicula { get; set; } = null!;
    public SalaCine SalaCine { get; set; } = null!;
}
