using System.Globalization;
using CinemaApi.Models.DTOs;
using CinemaApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PeliculasController(IPeliculaService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(List<PeliculaResponseDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<List<PeliculaResponseDto>>> GetAll() => Ok(await service.GetAllAsync());

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PeliculaResponseDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PeliculaResponseDto>> GetById(int id) => Ok(await service.GetByIdAsync(id));

    [HttpGet("buscar")]
    public async Task<ActionResult<List<PeliculaResponseDto>>> Search([FromQuery] string nombre) =>
        Ok(await service.SearchByNameAsync(nombre));

    [HttpGet("por-fecha")]
    public async Task<ActionResult<List<PeliculaPorFechaDto>>> GetByDate([FromQuery] string fecha)
    {
        if (!DateOnly.TryParseExact(fecha, "yyyy-MM-dd", CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var parsedDate))
            return BadRequest(new { message = "La fecha ingresada no es válida. Utilice el formato YYYY-MM-DD." });

        return Ok(await service.GetByPublicationDateAsync(parsedDate));
    }

    [HttpPost]
    [ProducesResponseType(typeof(PeliculaResponseDto), StatusCodes.Status201Created)]
    public async Task<ActionResult<PeliculaResponseDto>> Create(PeliculaRequestDto request)
    {
        var created = await service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.IdPelicula }, created);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Update(int id, PeliculaRequestDto request)
    {
        await service.UpdateAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}
