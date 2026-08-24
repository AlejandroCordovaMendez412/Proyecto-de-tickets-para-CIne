using CinemaApi.Models.DTOs;
using CinemaApi.Models.Entities;
using CinemaApi.Repository.Interfaces;
using CinemaApi.Services.Interfaces;

namespace CinemaApi.Services.Implementations;

public class AsignacionService(
    IAsignacionRepository repository,
    IPeliculaRepository peliculaRepository,
    ISalaRepository salaRepository) : IAsignacionService
{
    public async Task<List<AsignacionResponseDto>> GetAllAsync() =>
        (await repository.GetAllAsync()).Select(Map).ToList();

    public async Task<AsignacionResponseDto> CreateAsync(AsignacionRequestDto request)
    {
        var pelicula = await peliculaRepository.GetByIdAsync(request.IdPelicula)
            ?? throw new ArgumentException("La película no existe o está inactiva.");
        var sala = await salaRepository.GetByIdAsync(request.IdSalaCine)
            ?? throw new ArgumentException("La sala no existe o está inactiva.");
        var inicio = request.FechaPublicacion
            ?? throw new ArgumentException("La fecha de publicación es obligatoria.");
        if (request.FechaFin.HasValue && request.FechaFin.Value < inicio)
            throw new ArgumentException("La fecha fin debe ser igual o posterior a la fecha de publicación.");

        var asignacion = new PeliculaSalaCine
        {
            IdPelicula = pelicula.IdPelicula,
            IdSalaCine = sala.IdSala,
            FechaPublicacion = inicio,
            FechaFin = request.FechaFin,
            Activo = true,
            Pelicula = pelicula,
            SalaCine = sala
        };
        await repository.AddAsync(asignacion);
        await repository.SaveChangesAsync();
        return Map(asignacion);
    }

    private static AsignacionResponseDto Map(PeliculaSalaCine x) => new(
        x.IdPeliculaSala, x.IdPelicula, x.Pelicula.Nombre,
        x.IdSalaCine, x.SalaCine.Nombre, x.FechaPublicacion, x.FechaFin);
}
