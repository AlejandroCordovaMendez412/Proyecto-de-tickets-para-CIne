using CinemaApi.Data;
using CinemaApi.Models.DTOs;
using CinemaApi.Models.Entities;
using CinemaApi.Repository.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace CinemaApi.Repository.Implementations;

public class PeliculaRepository(CinemaDbContext context) : IPeliculaRepository
{
    public Task<List<Pelicula>> GetAllAsync() => context.Peliculas
        .AsNoTracking().Where(x => x.Activo).OrderBy(x => x.Nombre).ToListAsync();

    public Task<Pelicula?> GetByIdAsync(int id) => context.Peliculas
        .FirstOrDefaultAsync(x => x.IdPelicula == id && x.Activo);

    public Task<List<Pelicula>> SearchByNameAsync(string nombre) => context.Peliculas
        .AsNoTracking()
        .Where(x => x.Activo && x.Nombre.Contains(nombre))
        .OrderBy(x => x.Nombre)
        .ToListAsync();

    public Task<List<PeliculaPorFechaDto>> GetByPublicationDateAsync(DateOnly fecha) =>
        context.PeliculasSalasCine
            .AsNoTracking()
            .Where(x => x.Activo && x.Pelicula.Activo && x.SalaCine.Estado && x.FechaPublicacion == fecha)
            .Select(x => new PeliculaPorFechaDto(
                x.Pelicula.IdPelicula, x.Pelicula.Nombre, x.Pelicula.Duracion, x.FechaPublicacion))
            .Distinct()
            .ToListAsync();

    public Task<bool> ExistsActiveByNameAsync(string nombre, int? excludingId = null) => context.Peliculas
        .AnyAsync(x => x.Activo && x.Nombre == nombre && (!excludingId.HasValue || x.IdPelicula != excludingId));

    public Task AddAsync(Pelicula pelicula) => context.Peliculas.AddAsync(pelicula).AsTask();

    public Task SaveChangesAsync() => context.SaveChangesAsync();
}
