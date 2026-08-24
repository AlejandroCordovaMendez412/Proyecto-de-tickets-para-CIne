using CinemaApi.Models.DTOs;
using CinemaApi.Models.Entities;

namespace CinemaApi.Repository.Interfaces;

public interface IPeliculaRepository
{
    Task<List<Pelicula>> GetAllAsync();
    Task<Pelicula?> GetByIdAsync(int id);
    Task<List<Pelicula>> SearchByNameAsync(string nombre);
    Task<List<PeliculaPorFechaDto>> GetByPublicationDateAsync(DateOnly fecha);
    Task<bool> ExistsActiveByNameAsync(string nombre, int? excludingId = null);
    Task AddAsync(Pelicula pelicula);
    Task SaveChangesAsync();
}
