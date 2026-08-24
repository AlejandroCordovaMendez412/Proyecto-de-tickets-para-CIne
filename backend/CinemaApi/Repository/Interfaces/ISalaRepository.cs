using CinemaApi.Models.Entities;

namespace CinemaApi.Repository.Interfaces;

public interface ISalaRepository
{
    Task<List<SalaCine>> GetAllAsync();
    Task<SalaCine?> GetByIdAsync(int id);
    Task<bool> ExistsActiveByNameAsync(string nombre, int? excludingId = null);
    Task AddAsync(SalaCine sala);
    Task SaveChangesAsync();
    Task<DisponibilidadSalaResult?> GetAvailabilityAsync(string nombreSala);
}
