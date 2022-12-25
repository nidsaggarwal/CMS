using CMSApplication.Auth;
using CMSApplication.Data.Entity;
using CMSApplication.Enums;
using CMSApplication.Models;
using CMSApplication.Models.BindingModel;
using CMSApplication.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace CMSApplication.Controllers
{


    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly ILogger<UserController> _logger;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly JWTConfig _jWTConfig;
        private readonly IJwtProvider _jwtProvider;

        public UserController(ILogger<UserController> logger, UserManager<User> userManager, SignInManager<User> signManager, IOptions<JWTConfig> jwtConfig, RoleManager<ApplicationRole> roleManager, IJwtProvider jwtProvider)
        {
            _userManager = userManager;
            _signInManager = signManager;
            _roleManager = roleManager;
            _logger = logger;
            _jWTConfig = jwtConfig.Value;
            _jwtProvider = jwtProvider;

        }

        [HttpPost("RegisterUser")]
        public async Task<object> RegisterUser([FromBody] AddUserBindingModel model)
        {
            try
            {
                if (model.Roles == null)
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Roles are missing", null));
                }
                foreach (var role in model.Roles)
                {

                    if (!await _roleManager.RoleExistsAsync(role))
                    {

                        return await Task.FromResult(new ResponseModel(ResponseCode.Error, "Role does not exist", null));
                    }
                }


                var user = new User() { FullName = model.FullName, Email = model.Email, UserName = model.Email, CreatedDate = DateTime.UtcNow, ModifiedDate = DateTime.UtcNow };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var tempUser = await _userManager.FindByEmailAsync(model.Email);
                    foreach (var role in model.Roles)
                    {
                        await _userManager.AddToRoleAsync(tempUser, role);
                    }
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "User has been Registered", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "", result.Errors.Select(x => x.Description).ToArray()));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }



        [HttpGet("GetAllUser")]
        public async Task<object> GetAllUser()
        {
            try
            {
                List<UserDTO> allUserDTO = new List<UserDTO>();
                var users = _userManager.Users.ToList();
                foreach (var user in users)
                {
                    var roles = (await _userManager.GetRolesAsync(user)).ToList();

                    allUserDTO.Add(new UserDTO(user.FullName, user.Email, user.UserName, user.CreatedDate, roles));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", allUserDTO));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }


        [HttpGet("GetUserList")]
        public async Task<object> GetUserList()
        {
            try
            {
                List<UserDTO> allUserDTO = new List<UserDTO>();
                var users = _userManager.Users.ToList();
                foreach (var user in users)
                {
                    var role = (await _userManager.GetRolesAsync(user)).ToList();
                    if (role.Any(x => x == "User"))
                    {
                        allUserDTO.Add(new UserDTO(user.FullName, user.Email, user.UserName, user.CreatedDate, role));
                    }
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", allUserDTO));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }



        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<object> Login([FromBody] LoginBindingModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        var User = await _userManager.FindByEmailAsync(model.Email);
                        var roles = (await _userManager.GetRolesAsync(User)).ToList();
                        var user = new UserDTO(User.FullName, User.Email, User.UserName, User.CreatedDate, roles);
                        user.Token = _jwtProvider.GenerateToken(User, roles);

                        return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", user));

                    }
                }

                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "invalid Email or password", null));

            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }

        [HttpGet("GetRoles")]
        public async Task<object> GetRoles()
        {
            try
            {

                var roles = _roleManager.Roles.Select(x => x.Name).ToList();

                return await Task.FromResult(new ResponseModel(ResponseCode.OK, "", roles));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }
        [HttpPost("AddRole")]
        public async Task<object> AddRole([FromBody] AddRoleBindingModel model)
        {
            try
            {
                if (model == null || model.Role == "")
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.Error, "parameters are missing", null));

                }
                if (await _roleManager.RoleExistsAsync(model.Role))
                {
                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Role already exist", null));

                }
                var role = new ApplicationRole();
                role.Name = model.Role;
                role.NormalizedName = model.Role;
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {

                    return await Task.FromResult(new ResponseModel(ResponseCode.OK, "Role added successfully", null));
                }
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, "something went wrong please try again later", null));
            }
            catch (Exception ex)
            {
                return await Task.FromResult(new ResponseModel(ResponseCode.Error, ex.Message, null));
            }
        }


    }
}
