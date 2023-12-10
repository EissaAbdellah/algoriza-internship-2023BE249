using Core.DTOS;
using Core.Identity;

namespace Core.Servicses
{
    public interface ITokenServicse
    {
        public Task<tokenDto> CreateToken(ApplicationUser user);
    }
}
