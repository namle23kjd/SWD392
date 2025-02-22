using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using Warehouse_Management.Helpers;
using Warehouse_Management.Models.DTO;
using Warehouse_Management.Models.DTO.Product;
using Warehouse_Management.Services.IService;
using Warehouse_Management.Services.Service;

namespace Warehouse_Management.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly IMapper _mapper;

        public AuthController(IUserService userService, UserManager<IdentityUser> userManager,IEmailService emailService,IMapper mapper)
        {
            _userService = userService;
            _userManager = userManager;
            _emailService = emailService;
            _mapper = mapper;
        }
        //Post:/ api/auth/register
        /// <summary>
        ///  
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("register")]
        //[Authorize]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDTO request)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _userService.RegisterAsync(request);
            return StatusCode((int)response.StatusCode, response);
        }

        //Post:/ api/auth/login
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _userService.LoginAsync(request);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO model)
        {
            var response = await _userService.ResetPasswordAsync(model.Email);
            return StatusCode((int)response.StatusCode, response);
        }

        [HttpPost("confirm-reset-password")]
        public async Task<IActionResult> ConfirmResetPassword([FromBody] ConfirmResetPasswordRequest model)
        {
            var response = await _userService.ConfirmResetPasswordAsync(model.Email, model.Token, model.NewPassword);
            return StatusCode((int)response.StatusCode, response);
        }

        // POST: /api/auth/edit-user
        /// <summary>
        /// Chỉnh sửa thông tin người dùng
        /// </summary>
        /// <param name="userId">ID người dùng</param>
        /// <param name="userDto">Thông tin chỉnh sửa</param>
        /// <returns>Trạng thái chỉnh sửa</returns>
        [HttpPut("edit-user/{userId}")]
        public async Task<IActionResult> EditUser(string userId, [FromBody] EditUserDTO userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _userService.EditUserAsync(userId, userDto);
            return StatusCode((int)response.StatusCode, response);
        }

        // PUT: /api/auth/modify-role
        /// <summary>
        /// Thay đổi vai trò của người dùng
        /// </summary>
        /// <param name="userId">ID người dùng</param>
        /// <param name="roles">Danh sách vai trò mới</param>
        /// <returns>Trạng thái thay đổi vai trò</returns>
        [HttpPut("modify-role/{userId}")]
        public async Task<IActionResult> ModifyUserRole(string userId, [FromBody] string[] roles)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var response = await _userService.ModifyUserRoleAsync(userId, roles);
            return StatusCode((int)response.StatusCode, response);
        }

        // DELETE: /api/auth/delete-user
        /// <summary>
        /// Xóa người dùng
        /// </summary>
        /// <param name="userId">ID người dùng</param>
        /// <returns>Trạng thái xóa người dùng</returns>
        [HttpDelete("delete-user/{userId}")]
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var response = await _userService.DeleteUserAsync(userId);
            return StatusCode((int)response.StatusCode, response);
        }



        [HttpGet]
        public async Task<IActionResult> GetAllUser()
        {
            var response = await _userService.GetAllUserAsync();
            return StatusCode((int)response.StatusCode, response);
        }
    }
}
