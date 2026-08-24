using CinemaApi.Models.DTOs;
using CinemaApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SalasController(ISalaService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<SalaResponseDto>>> GetAll() => Ok(await service.GetAllAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<SalaResponseDto>> GetById(int id) => Ok(await service.GetByIdAsync(id));

    [HttpGet("disponibilidad")]
    public async Task<ActionResult<DisponibilidadSalaDto>> GetAvailability([FromQuery] string nombreSala) =>
        Ok(await service.GetAvailabilityAsync(nombreSala));

    [HttpPost]
    public async Task<ActionResult<SalaResponseDto>> Create(SalaRequestDto request)
    {
        var created = await service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.IdSala }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, SalaRequestDto request)
    {
        await service.UpdateAsync(id, request);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await service.DeleteAsync(id);
        return NoContent();
    }
}
