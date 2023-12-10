using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Core.DTOS;
using Core.Identity;
using Core.Servicses;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
    public class TokenServicse : ITokenServicse
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly IConfiguration _config;


        public TokenServicse(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _config = config;
        }


        public async Task<tokenDto> CreateToken(ApplicationUser user)
        {

            // Add Claims 
            var claims = new List<Claim>();

            claims.Add(new Claim(ClaimTypes.Name, user.Email));
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));

            //get role
            var roles = await _userManager.GetRolesAsync(user);
            foreach (var itemRole in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, itemRole));
            }

            SecurityKey securityKey =
                      new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));

            SigningCredentials signincred =
                      new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);


            //Create token
            JwtSecurityToken mytoken = new JwtSecurityToken(
                issuer: _config["JWT:ValidIssuer"],//url web api
                audience: _config["JWT:ValidAudiance"],//url consumer angular
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: signincred
                );
            tokenDto tokenDto = new tokenDto()
            {
                token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                userId = user.Id,
                epxpiration = mytoken.ValidTo

            };

            return tokenDto;




        }



    }
}
