using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Core.DTOS.Doctor;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Roles = "Doctor")]
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly IDoctorRepository _doctorRepository;
        public DoctorController(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }


        [HttpGet("Booking/GetAll")]
        public async Task<IActionResult> GetAll(int page = 1, int pageSize = 10, [Required] DayOfWeek date = DayOfWeek.Tuesday)
        {
            try
            {
                var doctorId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (doctorId != null)
                    return Ok(await _doctorRepository.GetAll(page, pageSize, doctorId, date));
                // the case if the user ID claim is not found
                return BadRequest("User ID not found");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpPost("Booking/ConfirmCheckUp")]
        public async Task<IActionResult> ConfirmCheckUp([Required] int bookingId)
        {
            try
            {
                if (await _doctorRepository.ConfirmCheckUp(bookingId))
                    return Ok();
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPost("Setting/Add")]                   //Time format as timeSpane in Integer as next 14,01,23,21,17 ets values from 00 to 23 hours
        public async Task<IActionResult> AddAppointment([Required] AddAppointmentDto appointmentDto)
        {
            try
            {
                var doctorId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (doctorId != null)
                {
                    if (await _doctorRepository.AddAppointment(appointmentDto, doctorId))
                        return Ok();
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }


        [HttpPut("Setting/Update")]
        public async Task<IActionResult> UpdateppointmentTime([Required] int AppointmentTimeId, [Required] UpdateAppointmnetTimeDto timeDto)
        {
            try
            {
                if (await _doctorRepository.UpdateAppontment(AppointmentTimeId, timeDto))
                    return Ok();
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("Setting/Delete")]
        public async Task<IActionResult> DeleteppointmentTime([Required] int AppointmentTimeId)
        {
            try
            {
                if (await _doctorRepository.DeleteAppontment(AppointmentTimeId))
                    return Ok();
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }







    }
}
