using CinemaApi.Data;
using CinemaApi.Models.Entities;
using CinemaApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaApi.Repository.Implementations;

public class SalaRepository(CinemaDbContext context) : ISalaRepository
{
    public Task<List<SalaCine>> GetAllAsync() => context.SalasCine
        .AsNoTracking().Where(x => x.Estado).OrderBy(x => x.Nombre).ToListAsync();

    public Task<SalaCine?> GetByIdAsync(int id) => context.SalasCine
        .FirstOrDefaultAsync(x => x.IdSala == id && x.Estado);

    public Task<bool> ExistsActiveByNameAsync(string nombre, int? excludingId = null) => context.SalasCine
        .AnyAsync(x => x.Estado && x.Nombre == nombre && (!excludingId.HasValue || x.IdSala != excludingId));

    public Task AddAsync(SalaCine sala) => context.SalasCine.AddAsync(sala).AsTask();

    public Task SaveChangesAsync() => context.SaveChangesAsync();

    public async Task<DisponibilidadSalaResult?> GetAvailabilityAsync(string nombreSala)
    {
        var results = await context.DisponibilidadesSala
            .FromSqlInterpolated($"EXEC sp_ObtenerDisponibilidadSala @NombreSala={nombreSala}")
            .AsNoTracking()
            .ToListAsync();
        return results.SingleOrDefault();
    }
}
