using System.ComponentModel.DataAnnotations;
using Core.DTOS;
using Core.DTOS.Doctor;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {

        private readonly IAdminRepository _adminRepository;




        public AdminController(IAdminRepository adminRepository)
        {
            _adminRepository = adminRepository;

        }


        //Authorized (Role)Admin

        #region Admin Dashboard


        [HttpGet("Dashboard/NumOfDoctors")]
        public async Task<IActionResult> NumOfDoctors()
        {

            try
            {
                return Ok(await _adminRepository.NumOfDoctors());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }


        [HttpGet("Dashboard/NumOfPatients")]
        public async Task<IActionResult> NumOfPatients()
        {
            try
            {
                return Ok(await _adminRepository.NumOfPatients());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }


        [HttpGet("Dashboard/NumOfRequests")]
        public async Task<IActionResult> NumOfRequests()
        {

            try
            {
                return Ok(await _adminRepository.NumOfRequests());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpGet("Dashboard/TopFiveSpecializations")]
        public async Task<IActionResult> TopFiveSpecializations()
        {
            try
            {
                return Ok(await _adminRepository.TopFiveSpecializations());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }




        [HttpGet("Dashboard/TopTenDocotors")]
        public async Task<IActionResult> TopTenDocotors()
        {

            try
            {
                return Ok(await _adminRepository.TopTenDotors());
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }




        #endregion



        #region Admin Doctors


        [HttpGet("Doctors/GetAll")]
        public async Task<IActionResult> GetAllDoctors(int page = 1, int pageSize = 10, string search = "")
        {
            try
            {
                return Ok(await _adminRepository.GetAllDoctors(page, pageSize, search));
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }

        [HttpGet("Doctors/GetByID")]
        public async Task<IActionResult> GetDoctorById([Required] string id)
        {
            try
            {
                var doctor = await _adminRepository.GetDoctorById(id);
                if (doctor is not null)
                    return Ok(doctor);
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        [HttpPut("Doctors/Edit")]
        public async Task<IActionResult> EditDoctor([FromForm] EditDoctotDto editDoctotDto, [Required] string doctorId)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _adminRepository.EditDoctor(editDoctotDto, doctorId))
                    {
                        return Ok();
                    }

                    return StatusCode(500);
                }
                else return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }


        [HttpDelete("Doctors/Delete")]
        public async Task<IActionResult> DeleteDoctor([Required] string doctorId)
        {
            try
            {

                if (await _adminRepository.DeleteDoctor(doctorId))
                {
                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        #endregion


        #region Admin Patients

        [HttpGet("Patients/GetAll")]
        public async Task<IActionResult> GetAllPatients(int page = 1, int pageSize = 10, string search = "")
        {
            try
            {
                return Ok(await _adminRepository.GetAllPatients(page, pageSize, search));
            }
            catch (Exception e)
            {
                return StatusCode(501, e.Message);
            }
        }

        [HttpGet("Patients/GetByID")]
        public async Task<IActionResult> GetPatientById(string id)
        {
            try
            {
                var patient = await _adminRepository.GetPatientById(id);
                if (patient is not null)
                    return Ok(patient);
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }



        #endregion


        #region Settings Coupon
        [HttpPost("Setting/Coupon/Add")]
        public async Task<IActionResult> AddCoupon([Required] CouponDto couponDto)
        {
            try
            {
                if (await _adminRepository.AddCoupon(couponDto))
                    return Ok();
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }


        [HttpPut("Setting/Coupon/Update")]
        public async Task<IActionResult> UpdateCoupon([Required] CouponDto couponDto, [Required] int couponId)
        {
            try
            {
                if (await _adminRepository.UpdateCoupon(couponId, couponDto))
                    return Ok();
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpDelete("Setting/Coupon/Delete")]
        public async Task<IActionResult> DeleteCoupon([Required] int couponId)
        {
            try
            {
                if (await _adminRepository.DeleteCoupon(couponId))
                    return Ok();
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }

        [HttpPut("Setting/Coupon/Deactive")]
        public async Task<IActionResult> DeactiveCoupon([Required] int couponId)
        {
            try
            {
                if (await _adminRepository.DeactiveCoupon(couponId))
                    return Ok();
                return BadRequest();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }


        #endregion



    }
}
