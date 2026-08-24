using CinemaApi.Data;
using CinemaApi.Models.Entities;
using CinemaApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaApi.Repository.Implementations;

public class AsignacionRepository(CinemaDbContext context) : IAsignacionRepository
{
    public Task<List<PeliculaSalaCine>> GetAllAsync() => context.PeliculasSalasCine
        .AsNoTracking()
        .Include(x => x.Pelicula)
        .Include(x => x.SalaCine)
        .Where(x => x.Activo && x.Pelicula.Activo && x.SalaCine.Estado)
        .OrderByDescending(x => x.FechaPublicacion)
        .ToListAsync();

    public Task AddAsync(PeliculaSalaCine asignacion) =>
        context.PeliculasSalasCine.AddAsync(asignacion).AsTask();

    public Task SaveChangesAsync() => context.SaveChangesAsync();
}
