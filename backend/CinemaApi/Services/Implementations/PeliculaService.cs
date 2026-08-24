using CinemaApi.Models.DTOs;
using CinemaApi.Models.Entities;
using CinemaApi.Repository.Interfaces;
using CinemaApi.Services.Interfaces;

namespace CinemaApi.Services.Implementations;

public class PeliculaService(IPeliculaRepository repository) : IPeliculaService
{
    public async Task<List<PeliculaResponseDto>> GetAllAsync() =>
        (await repository.GetAllAsync()).Select(Map).ToList();

    public async Task<PeliculaResponseDto> GetByIdAsync(int id) =>
        Map(await repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Película no encontrada."));

    public async Task<List<PeliculaResponseDto>> SearchByNameAsync(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("Ingrese un nombre para realizar la búsqueda.");
        return (await repository.SearchByNameAsync(nombre.Trim())).Select(Map).ToList();
    }

    public Task<List<PeliculaPorFechaDto>> GetByPublicationDateAsync(DateOnly fecha) =>
        repository.GetByPublicationDateAsync(fecha);

    public async Task<PeliculaResponseDto> CreateAsync(PeliculaRequestDto request)
    {
        var nombre = NormalizeName(request.Nombre);
        if (await repository.ExistsActiveByNameAsync(nombre))
            throw new ArgumentException("Ya existe una película activa con el mismo nombre.");

        var pelicula = new Pelicula { Nombre = nombre, Duracion = request.Duracion, Activo = true };
        await repository.AddAsync(pelicula);
        await repository.SaveChangesAsync();
        return Map(pelicula);
    }

    public async Task UpdateAsync(int id, PeliculaRequestDto request)
    {
        var pelicula = await repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Película no encontrada.");
        var nombre = NormalizeName(request.Nombre);
        if (await repository.ExistsActiveByNameAsync(nombre, id))
            throw new ArgumentException("Ya existe una película activa con el mismo nombre.");

        pelicula.Nombre = nombre;
        pelicula.Duracion = request.Duracion;
        await repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var pelicula = await repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Película no encontrada.");
        pelicula.Activo = false;
        await repository.SaveChangesAsync();
    }

    private static string NormalizeName(string nombre)
    {
        var value = nombre.Trim();
        if (value.Length == 0) throw new ArgumentException("El nombre es obligatorio.");
        return value;
    }

    private static PeliculaResponseDto Map(Pelicula x) => new(x.IdPelicula, x.Nombre, x.Duracion);
}
