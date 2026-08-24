using CinemaApi.Models.DTOs;
using CinemaApi.Models.Entities;
using CinemaApi.Repository.Interfaces;
using CinemaApi.Services.Interfaces;

namespace CinemaApi.Services.Implementations;

public class SalaService(ISalaRepository repository) : ISalaService
{
    public async Task<List<SalaResponseDto>> GetAllAsync() =>
        (await repository.GetAllAsync()).Select(Map).ToList();

    public async Task<SalaResponseDto> GetByIdAsync(int id) =>
        Map(await repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Sala no encontrada."));

    public async Task<SalaResponseDto> CreateAsync(SalaRequestDto request)
    {
        var nombre = NormalizeName(request.Nombre);
        if (await repository.ExistsActiveByNameAsync(nombre))
            throw new ArgumentException("Ya existe una sala activa con el mismo nombre.");
        var sala = new SalaCine { Nombre = nombre, Estado = true };
        await repository.AddAsync(sala);
        await repository.SaveChangesAsync();
        return Map(sala);
    }

    public async Task UpdateAsync(int id, SalaRequestDto request)
    {
        var sala = await repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Sala no encontrada.");
        var nombre = NormalizeName(request.Nombre);
        if (await repository.ExistsActiveByNameAsync(nombre, id))
            throw new ArgumentException("Ya existe una sala activa con el mismo nombre.");
        sala.Nombre = nombre;
        await repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var sala = await repository.GetByIdAsync(id) ?? throw new KeyNotFoundException("Sala no encontrada.");
        sala.Estado = false;
        await repository.SaveChangesAsync();
    }

    public async Task<DisponibilidadSalaDto> GetAvailabilityAsync(string nombreSala)
    {
        if (string.IsNullOrWhiteSpace(nombreSala))
            throw new ArgumentException("Ingrese el nombre de la sala.");
        var result = await repository.GetAvailabilityAsync(nombreSala.Trim())
            ?? throw new KeyNotFoundException("Sala no encontrada.");
        return new(result.NombreSala, result.CantidadPeliculas, result.Mensaje);
    }

    private static string NormalizeName(string nombre)
    {
        var value = nombre.Trim();
        if (value.Length == 0) throw new ArgumentException("El nombre es obligatorio.");
        return value;
    }

    private static SalaResponseDto Map(SalaCine x) => new(x.IdSala, x.Nombre, x.Estado);
}
