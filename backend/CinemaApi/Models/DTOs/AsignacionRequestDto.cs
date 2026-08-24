using System.ComponentModel.DataAnnotations;

namespace CinemaApi.Models.DTOs;

public class AsignacionRequestDto
{
    [Range(1, int.MaxValue, ErrorMessage = "Seleccione una película válida.")]
    public int IdPelicula { get; set; }

    [Range(1, int.MaxValue, ErrorMessage = "Seleccione una sala válida.")]
    public int IdSalaCine { get; set; }

    [Required(ErrorMessage = "La fecha de publicación es obligatoria.")]
    public DateOnly? FechaPublicacion { get; set; }

    public DateOnly? FechaFin { get; set; }
}

public record AsignacionResponseDto(
    int IdAsignacion,
    int IdPelicula,
    string Pelicula,
    int IdSala,
    string Sala,
    DateOnly FechaPublicacion,
    DateOnly? FechaFin);
