using CinemaApi.Models.DTOs;

namespace CinemaApi.Services.Interfaces;

public interface ISalaService
{
    Task<List<SalaResponseDto>> GetAllAsync();
    Task<SalaResponseDto> GetByIdAsync(int id);
    Task<SalaResponseDto> CreateAsync(SalaRequestDto request);
    Task UpdateAsync(int id, SalaRequestDto request);
    Task DeleteAsync(int id);
    Task<DisponibilidadSalaDto> GetAvailabilityAsync(string nombreSala);
}
