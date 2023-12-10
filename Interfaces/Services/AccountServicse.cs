using Core.DTOS;
using Core.DTOS.Doctor;
using Core.DTOS.Patient;
using Core.Identity;
using Core.Servicses;
using Infrastructure.Data.Contexts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace Infrastructure.Services
{
    public class AccountServicse : IAccountServicse
    {
        private readonly ApplicationDbContext _context;

        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly ITokenServicse _tokenServicse;

        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _webHostEnvironment;

        private readonly IImageServicse _imageServicse;

        private readonly IMailServicse _mailServicse;



        public AccountServicse(ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config, SignInManager<ApplicationUser> signInManager, ITokenServicse tokenServicse, IWebHostEnvironment webHostEnvironment, IImageServicse imageServicse, IMailServicse mailServicse)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
            _signInManager = signInManager;
            _tokenServicse = tokenServicse;
            _webHostEnvironment = webHostEnvironment;
            _imageServicse = imageServicse;
            _mailServicse = mailServicse;
        }




        public async Task<bool> AddDoctor(RegisterDoctorDto registerDoctorDto)
        {

            ApplicationUser user = new ApplicationUser();

            user.UserName = registerDoctorDto.Email;
            user.Email = registerDoctorDto.Email;
            user.FirstName = registerDoctorDto.FirstName;
            user.LastName = registerDoctorDto.LastName;
            user.PhoneNumber = registerDoctorDto.PhoneNumber;
            user.DateOfBirth = registerDoctorDto.DateOfBirth;
            user.Gender = registerDoctorDto.Gender;
            user.RoleType = Core.Enums.RoleType.Doctor;
            user.SpecializationId = registerDoctorDto.SpecializationId;


            // save image

            var imagePath = await _imageServicse.saveImage(registerDoctorDto.ImageFile, "doctors");

            user.Image = imagePath;



            IdentityResult result = await _userManager.CreateAsync(user, registerDoctorDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Doctor");

                /*
                //PLEASE UNComment this section to try Sending Email 
                //SomeTimes EMAIL STMP Doesn't work correctlly for password issues
                // sending email
                string body = $"Dear {user.FirstName} {user.LastName}," +
                    $"\r\n\r\nThank you for your interest in Vezeeta.\r\n\r\nYour profile has now been created and is ready for you to add appointments." +
                    $"\r\n\r\nYou can log in using the below Email and Password.\r\n \r\n\r\n " +
                    $"Email:{user.Email} \r\nPassword:{registerDoctorDto.Password}\r\nKind Regards,\r\n\r\nVezeeta Team";
                string subject = "Vezeeta Doctor Account";
                string emilTo = registerDoctorDto.Email;


                await _mailServicse.sendEmail(emilTo, subject, body);
                */

                return true;
            }
            return false;




        }


        public async Task<bool> Register(RegisterPatientDto registerPatientDto)
        {
            ApplicationUser user = new ApplicationUser();

            user.UserName = registerPatientDto.Email;
            user.Email = registerPatientDto.Email;
            user.FirstName = registerPatientDto.FirstName;
            user.LastName = registerPatientDto.LastName;
            user.PhoneNumber = registerPatientDto.PhoneNumber;
            user.DateOfBirth = registerPatientDto.DateOfBirth;
            user.Gender = registerPatientDto.Gender;
            user.RoleType = Core.Enums.RoleType.Patient;

            //check if found Image

            if (registerPatientDto.ImageFile is not null)
            {
                var imagePath = await _imageServicse.saveImage(registerPatientDto.ImageFile, "patients");

                user.Image = imagePath;
            }
            else
            {
                user.Image = null;
            }


            IdentityResult result = await _userManager.CreateAsync(user, registerPatientDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Patient");
                return true;
            }
            return false;
        }


        public async Task<tokenDto> Login(LoginDto loginDto)
        {

            ApplicationUser user = await _userManager.FindByEmailAsync(loginDto.Email);
            if (user is not null)
            {

                bool found = await _userManager.CheckPasswordAsync(user, loginDto.Password);
                if (found)
                {

                    tokenDto tokenDto = await _tokenServicse.CreateToken(user);
                    if (tokenDto is not null)
                        return tokenDto;

                }
            }

            return null;
        }

        #region Add Admin and roles

        /*
        public async Task<bool> AddRole(string roleName)
        {


            IdentityRole role = new IdentityRole(roleName);
            IdentityResult result = await _roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }


        public async Task<bool> AddAdmin(AdminDto adminDto)
        {

            ApplicationUser user = new ApplicationUser();

            user.UserName = adminDto.Email;
            user.Email = adminDto.Email;
            user.FirstName = adminDto.FirstName;
            user.LastName = adminDto.LastName;
            user.PhoneNumber = adminDto.PhoneNumber;
            user.DateOfBirth = adminDto.DateOfBirth;
            user.Gender = adminDto.Gender;
            user.RoleType = Core.Enums.RoleType.Admin;

            IdentityResult result = await _userManager.CreateAsync(user, adminDto.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Admin");
                return true;
            }
            return false;
        }


        */

        #endregion

    }
}
