using CinemaApi.Models.Entities;

namespace CinemaApi.Repository.Interfaces;

public interface IAsignacionRepository
{
    Task<List<PeliculaSalaCine>> GetAllAsync();
    Task AddAsync(PeliculaSalaCine asignacion);
    Task SaveChangesAsync();
}
