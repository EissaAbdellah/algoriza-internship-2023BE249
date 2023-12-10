using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Core.DTOS.Patient;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Roles = "Patient")]
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly IPatientRepository _patientRepository;




        public PatientController(IPatientRepository patientRepository)
        {
            _patientRepository = patientRepository;

        }



        // ALLow for all user even is not loged in
        [AllowAnonymous]
        [HttpGet("SearchDoctors/GetAll")]
        public async Task<IActionResult> DisplayAllAppointmentToPatient(int page = 1, int pageSize = 10, string search = "")
        {
            try
            {
                return Ok(await _patientRepository.DisplayAllAppointmentToPatient(page, pageSize, search));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpPost("SearchDoctors/Booking")]
        public async Task<IActionResult> BookAppontment([Required] PatientBookingDto bookingDto, string discountCode = "")
        {
            try
            {
                var patientId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (patientId != null)
                {
                    if (await _patientRepository.BookAppointment(patientId, bookingDto, discountCode))
                        return Ok();
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }


        [HttpGet("Bookings/GetAll")]
        public async Task<IActionResult> GetAllBookings()
        {
            try
            {
                var patientId = HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                if (patientId != null)
                    return Ok(await _patientRepository.GetAllBooking(patientId));
                return BadRequest("User ID not found");
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("Booking/Cancel")]
        public async Task<IActionResult> CancelBooking(int bookingId)
        {
            try
            {
                if (await _patientRepository.CancelBooking(bookingId))
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
