namespace CinemaApi.Models.DTOs;

public record PeliculaResponseDto(int IdPelicula, string Nombre, int Duracion);
public record PeliculaPorFechaDto(int IdPelicula, string Nombre, int Duracion, DateOnly FechaPublicacion);
