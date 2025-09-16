using MediBook.Application.Dtos;
using MediBook.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MediBook.Api.Controllers;

[ApiController]
[Route("api/doctors")]
public class DoctorsController(IDoctorService doctorService) : ControllerBase
{
    private readonly IDoctorService _doctorService = doctorService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<DoctorDto>>> GetDoctors(CancellationToken cancellationToken)
    {
        var doctors = await _doctorService.GetAsync(cancellationToken);
        return Ok(doctors);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DoctorDto>> GetDoctor(int id, CancellationToken cancellationToken)
    {
        var doctor = await _doctorService.GetByIdAsync(id, cancellationToken);
        return doctor is null ? NotFound() : Ok(doctor);
    }

    [HttpPost]
    public async Task<ActionResult<DoctorDto>> CreateDoctor([FromBody] CreateDoctorDto dto, CancellationToken cancellationToken)
    {
        var doctor = await _doctorService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetDoctor), new { id = doctor!.Id }, doctor);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<DoctorDto>> UpdateDoctor(int id, [FromBody] UpdateDoctorDto dto, CancellationToken cancellationToken)
    {
        var doctor = await _doctorService.UpdateAsync(id, dto, cancellationToken);
        return doctor is null ? NotFound() : Ok(doctor);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteDoctor(int id, CancellationToken cancellationToken)
    {
        var success = await _doctorService.DeleteAsync(id, cancellationToken);
        return success ? NoContent() : NotFound();
    }
}
