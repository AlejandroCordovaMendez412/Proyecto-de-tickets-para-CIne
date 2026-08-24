using CinemaApi.Data;
using CinemaApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaApi.Repository.Implementations;

public class DashboardRepository(CinemaDbContext context) : IDashboardRepository
{
    public Task<int> CountActiveMoviesAsync() => context.Peliculas.CountAsync(x => x.Activo);

    public Task<int> CountActiveRoomsAsync() => context.SalasCine.CountAsync(x => x.Estado);

    public Task<int> CountAvailableRoomsAsync() => context.SalasCine
        .Where(sala => sala.Estado)
        .CountAsync(sala => sala.Asignaciones.Count(a => a.Activo && a.Pelicula.Activo) < 3);
}
