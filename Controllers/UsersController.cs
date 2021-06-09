using System.Threading.Tasks;
using Auth_API.Services.Users;
using Microsoft.AspNetCore.Mvc;
using Store_API.Services.Auth;
using Store_Shared.Dto;

namespace Store_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public UsersController(IUserService userService, IAuthService authService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var users = await _userService.GetAll();

            if (users == null) return BadRequest(ModelState);

            return Ok(users);
        }


        [HttpGet("Detail")]
        public async Task<IActionResult> GetUser(string id)
        {
            // if (ModelState.IsValid) return BadRequest(ModelState);

            var result = await _userService.Get(id);

            if (result is null) return BadRequest(result.Message);

            return Ok(result);
        }


        [HttpPost("Add")]
        public async Task<IActionResult> AddUser([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            // var usermodel =
            //Register User and Get the Result
            var result = await _authService.RegisterAsync(model);

            //In case Some error => BadRequest
            if (!result.IsAuthenticated) return BadRequest(result.Message);

            //In case success return Ok with the result OR Object with some info (if Needed!)
            //return Ok(new {Token = result.Token , ExpiresOn= result.ExpiresOn});
            return Ok(result);
        }


        [HttpPut("Edit")]
        public async Task<IActionResult> EditUser(string userId, EditUserModel user)
        {
            if (!ModelState.IsValid || userId != user.Id) return BadRequest(ModelState);


            var result = await _userService.Put(user);

            if (!string.IsNullOrEmpty(result.Message)) return BadRequest(result.Message);
            return Ok(result);
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            if (!await _userService.Delete(id)) return BadRequest(new {Message = "Couldn't Delete User !"});

            return Ok(new {Message = "User deleted successfully."});
        }


        #region Get UserList and RoleList

        [HttpGet("Roles")]
        public async Task<IActionResult> UsersAndRolesAsync()
        {
            var roleList = await _userService.GetRolesList();
            var usersList = await _userService.GetUsersList();

            if (roleList.Count < 0 || usersList.Count < 0) return BadRequest();

            return Ok(new {Roles = roleList, Users = usersList});
        }

        #endregion


        #region Add User To Role Method

        [HttpPost("ManageRole")]
        public async Task<IActionResult> AddToRoleAsync([FromBody] AddRoleModel model)
        {
            //Check the Model State(Annotaions)
            if (!ModelState.IsValid) return BadRequest(ModelState);

            //AddRole and Get the Result
            var result = await _userService.AddRoleAsync(model);

            //In case Some error => BadRequest
            if (result.Length < 0) return BadRequest(result.Length);

            //In case success return Ok with the result OR Object with some info (Needed!)
            //return Ok(new {Token = result.Token , ExpiresOn= result.ExpiresOn});
            return Ok(result);
        }

        #endregion
    }
}