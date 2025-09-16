using MediBook.Application.Dtos;
using MediBook.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MediBook.Api.Controllers;

[ApiController]
[Route("api/appointments")]
public class AppointmentsController(IAppointmentService appointmentService) : ControllerBase
{
    private readonly IAppointmentService _appointmentService = appointmentService;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<AppointmentDto>>> GetAppointments([FromQuery] int? userId, [FromQuery] int? doctorId, CancellationToken cancellationToken)
    {
        var appointments = await _appointmentService.GetAsync(userId, doctorId, cancellationToken);
        return Ok(appointments);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<AppointmentDto>> GetAppointment(int id, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentService.GetByIdAsync(id, cancellationToken);
        return appointment is null ? NotFound() : Ok(appointment);
    }

    [HttpPost]
    public async Task<ActionResult<AppointmentDto>> CreateAppointment([FromBody] CreateAppointmentDto dto, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentService.CreateAsync(dto, cancellationToken);
        if (appointment is null)
        {
            return Conflict(new { message = "This time slot is already booked." });
        }
        return CreatedAtAction(nameof(GetAppointment), new { id = appointment.Id }, appointment);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<AppointmentDto>> UpdateAppointment(int id, [FromBody] UpdateAppointmentDto dto, CancellationToken cancellationToken)
    {
        var appointment = await _appointmentService.UpdateAsync(id, dto, cancellationToken);
        if (appointment is null)
        {
            return Conflict(new { message = "Unable to update appointment." });
        }
        return Ok(appointment);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAppointment(int id, CancellationToken cancellationToken)
    {
        var success = await _appointmentService.DeleteAsync(id, cancellationToken);
        return success ? NoContent() : NotFound();
    }
}
