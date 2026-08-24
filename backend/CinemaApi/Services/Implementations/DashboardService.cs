using CinemaApi.Models.DTOs;
using CinemaApi.Repository.Interfaces;
using CinemaApi.Services.Interfaces;

namespace CinemaApi.Services.Implementations;

public class DashboardService(IDashboardRepository repository) : IDashboardService
{
    public async Task<DashboardDto> GetAsync()
    {
        var totalSalas = await repository.CountActiveRoomsAsync();
        var totalDisponibles = await repository.CountAvailableRoomsAsync();
        var totalPeliculas = await repository.CountActiveMoviesAsync();
        return new(totalSalas, totalDisponibles, totalPeliculas);
    }
}
