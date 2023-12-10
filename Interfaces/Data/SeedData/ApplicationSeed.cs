using Core.Entities;
using Core.Identity;
using Infrastructure.Data.Contexts;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Data.SeedData
{
    public class ApplicationSeed
    {


        public static async Task seedAdminData(UserManager<ApplicationUser> _userManager)
        {
            if (!_userManager.Users.Any())
            {
                ApplicationUser user = new ApplicationUser()
                {
                    Email = "admin@vezeeta.com",
                    UserName = "admin@vezeeta.com",
                    FirstName = "Eissa",
                    LastName = "Abdellah",
                    DateOfBirth = new DateTime(1996, 12, 5),
                    PhoneNumber = "01113741294",
                    Gender = 0


                };
                IdentityResult result = await _userManager.CreateAsync(user, "Aa@123456789");
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, "Admin");

                }



            }
        }



        public static async Task seedSpecialization(ApplicationDbContext _context)
        {
            if (!_context.Specializations.Any())
            {
                var specializations = new Specialization[]
                 {
                        new Specialization { Name = "Cardiology" },
                        new Specialization { Name = "Orthopedics" },
                        new Specialization { Name = "Neurology" },
                        new Specialization { Name = "Dermatology" },
                        new Specialization { Name = "Obstetrics and Gynecology (OB/GYN)" },
                        new Specialization { Name = "Pediatrics" },
                        new Specialization { Name = "Ophthalmology" },
                        new Specialization { Name = "Urology" },
                        new Specialization { Name = "Gastroenterology" },
                        new Specialization { Name = "Oncology" },
                        new Specialization { Name = "Endocrinology" },
                        new Specialization { Name = "Rheumatology" },
                        new Specialization { Name = "Pulmonology" },
                        new Specialization { Name = "Hematology" },
                        new Specialization { Name = "Nephrology" },
                        new Specialization { Name = "Infectious Disease" },
                        new Specialization { Name = "Allergy and Immunology" },
                        new Specialization { Name = "Psychiatry" },
                        new Specialization { Name = "Radiology" }

                  };

                await _context.AddRangeAsync(specializations);

                await _context.SaveChangesAsync();

            }
        }








    }
}
