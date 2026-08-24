using CinemaApi.Models.DTOs;

namespace CinemaApi.Services.Interfaces;

public interface IDashboardService
{
    Task<DashboardDto> GetAsync();
}
