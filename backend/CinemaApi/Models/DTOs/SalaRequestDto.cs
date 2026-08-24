using System.ComponentModel.DataAnnotations;

namespace CinemaApi.Models.DTOs;

public class SalaRequestDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [StringLength(150, ErrorMessage = "El nombre no puede superar 150 caracteres.")]
    public string Nombre { get; set; } = string.Empty;
}
