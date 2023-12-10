using Core.DTOS;
using Core.DTOS.Doctor;
using Core.DTOS.Patient;
using Core.Servicses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {

        private readonly IAccountServicse _accountServicse;


        public AccountController(IAccountServicse accountServicse)
        {

            _accountServicse = accountServicse;
        }



        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromForm] LoginDto loginDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var token = await _accountServicse.Login(loginDto);
                    if (token is not null)
                        return Ok(token);
                }
                return Unauthorized();
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }
        }


        //Register Doctor by Admin
        [Authorize(Roles = "Admin")]
        [HttpPost("Admin/Add/Doctor")]
        public async Task<IActionResult> AddDoctor([FromForm] RegisterDoctorDto registerDoctorDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _accountServicse.AddDoctor(registerDoctorDto))
                        return Ok();
                    else return StatusCode(501);
                }
                else
                    return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }

        }


        [HttpPost("Patient/Register")]
        public async Task<IActionResult> Register([FromForm] RegisterPatientDto registerPatientDto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (await _accountServicse.Register(registerPatientDto))
                        return Ok();
                }
                return BadRequest(ModelState);
            }
            catch (Exception e)
            {
                return StatusCode(500, e.Message);
            }


        }


        // IF Admin Need to Add Role

        /*
        [HttpPost("Admin/Add/Role")]
        public async Task<IActionResult> AddRole(string roleName)
        {
            if (await _accountServicse.AddRole(roleName))
                return Ok();
            else
                return StatusCode(501);

        }

        [HttpPost("Admin/Add")]
        public async Task<IActionResult> AddAdmin(AdminDto adminDto)
        {
            if (await _accountServicse.AddAdmin(adminDto))
                return Ok();
            else
                return StatusCode(501);


        }

        */





    }
}
