using CinemaApi.Models.DTOs;
using CinemaApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace CinemaApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AsignacionesController(IAsignacionService service) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<AsignacionResponseDto>>> GetAll() => Ok(await service.GetAllAsync());

    [HttpPost]
    public async Task<ActionResult<AsignacionResponseDto>> Create(AsignacionRequestDto request)
    {
        var created = await service.CreateAsync(request);
        return CreatedAtAction(nameof(GetAll), null, created);
    }
}
