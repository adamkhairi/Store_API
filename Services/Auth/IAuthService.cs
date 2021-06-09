using System.Collections.Generic;
using System.Threading.Tasks;
using Store_Shared.Dto;

namespace Store_API.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthModel> RegisterAsync(RegisterModel model);
        Task<AuthModel> GetTokenAsync(TokenRequestModel model);
        Task<string> AddRoleAsync(AddRoleModel model);
        Task<List<object>> GetRolesList();
        Task<List<object>> GetUsersList();
    }

    //public class AddRoleModel
    //{
    //}
}