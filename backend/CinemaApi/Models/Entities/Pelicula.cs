namespace CinemaApi.Models.Entities;

public class Pelicula
{
    public int IdPelicula { get; set; }
    public required string Nombre { get; set; }
    public int Duracion { get; set; }
    public bool Activo { get; set; } = true;
    public ICollection<PeliculaSalaCine> Asignaciones { get; set; } = [];
}
