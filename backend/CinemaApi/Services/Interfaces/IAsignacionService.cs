using CinemaApi.Models.DTOs;

namespace CinemaApi.Services.Interfaces;

public interface IAsignacionService
{
    Task<List<AsignacionResponseDto>> GetAllAsync();
    Task<AsignacionResponseDto> CreateAsync(AsignacionRequestDto request);
}
