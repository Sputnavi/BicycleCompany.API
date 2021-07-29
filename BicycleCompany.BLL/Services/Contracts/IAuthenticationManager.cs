using BicycleCompany.Models.Request;
using System.Threading.Tasks;

namespace BicycleCompany.BLL.Services.Contracts
{
    public interface IAuthenticationManager
    {
        Task<bool> ValidateUser(UserForAuthenticationModel user);
        string CreateToken();
    }
}
