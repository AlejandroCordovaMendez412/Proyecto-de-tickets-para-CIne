using System.ComponentModel.DataAnnotations;

namespace CinemaApi.Models.DTOs;

public class PeliculaRequestDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(150, ErrorMessage = "El nombre no puede superar 150 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "La duración debe ser mayor que cero.")]
    public int Duracion { get; set; }
}
