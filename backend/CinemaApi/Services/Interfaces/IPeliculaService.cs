using CinemaApi.Models.DTOs;

namespace CinemaApi.Services.Interfaces;

public interface IPeliculaService
{
    Task<List<PeliculaResponseDto>> GetAllAsync();
    Task<PeliculaResponseDto> GetByIdAsync(int id);
    Task<List<PeliculaResponseDto>> SearchByNameAsync(string nombre);
    Task<List<PeliculaPorFechaDto>> GetByPublicationDateAsync(DateOnly fecha);
    Task<PeliculaResponseDto> CreateAsync(PeliculaRequestDto request);
    Task UpdateAsync(int id, PeliculaRequestDto request);
    Task DeleteAsync(int id);
}
