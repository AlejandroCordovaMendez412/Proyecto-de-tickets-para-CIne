namespace CinemaApi.Models.Entities;

public class SalaCine
{
    public int IdSala { get; set; }
    public required string Nombre { get; set; }
    public bool Estado { get; set; } = true;
    public ICollection<PeliculaSalaCine> Asignaciones { get; set; } = [];
}
