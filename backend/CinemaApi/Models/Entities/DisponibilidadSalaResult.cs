namespace CinemaApi.Models.Entities;

public class DisponibilidadSalaResult
{
    public int IdSala { get; set; }
    public string NombreSala { get; set; } = string.Empty;
    public int CantidadPeliculas { get; set; }
    public string Mensaje { get; set; } = string.Empty;
}
