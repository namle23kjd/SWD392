using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using Warehouse_Management.Helpers;
using Warehouse_Management.Models.Domain;
using Warehouse_Management.Models.DTO;
using Warehouse_Management.Models.DTO.Product;
using Warehouse_Management.Repositories.IRepository;
using Warehouse_Management.Services.IService;

namespace Warehouse_Management.Services.Service
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _configuration; 
        private readonly ILogger<UserService> _logger;
        private readonly IEmailService _emailService;
        public UserService(IUserRepository userRepository, IMapper mapper, UserManager<IdentityUser> userManager, IConfiguration configuration, ILogger<UserService> logger, IEmailService emailService)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
            _emailService = emailService;
        }

        public async Task<ApiResponse> ConfirmResetPasswordAsync(string email, string token, string newPassword)
        {
            var response = new ApiResponse();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                response.ErrorMessages.Add("Email không tồn tại.");
                return response;
            }

            var result = await _userManager.ResetPasswordAsync(user, token, newPassword);
            if (!result.Succeeded)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                foreach (var error in result.Errors)
                {
                    response.ErrorMessages.Add(error.Description);
                }
                return response;
            }

            response.StatusCode = HttpStatusCode.OK;
            response.IsSuccess = true;
            response.Result = "Mật khẩu đã được đặt lại thành công.";
            return response;
        }

        public async Task<ApiResponse> DeleteUserAsync(string userId)
        {
            var response = new ApiResponse();

            try
            {
                response = await _userRepository.DeleteUserAsync(userId);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ErrorMessages.Add($"Error occurred while deleting user: {ex.Message}");
            }

            return response;
        }

        public async Task<ApiResponse> EditUserAsync(string userId, EditUserDTO userDto)
        {
            var response = new ApiResponse();

            try
            {
                response = await _userRepository.EditUserAsync(userId, userDto);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ErrorMessages.Add($"Error occurred while editing user: {ex.Message}");
            }

            return response;
        }

        public async Task<ApiResponse> ModifyUserRoleAsync(string userId, string[] newRoles)
        {
            var response = new ApiResponse();

            try
            {
                response = await _userRepository.ModifyUserRoleAsync(userId, newRoles);
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ErrorMessages.Add($"Error occurred while modifying user roles: {ex.Message}");
            }

            return response;
        }

        //public async Task<IEnumerable<UserDTO>> GetAllUsersAsync()
        //{
        //    var response = new ApiResponse<IEnumerable<UserDTO>>();

        //    try
        //    {
        //        var users = await _userRepository.GetAllUsersAsync(); // Giả sử đây là danh sách người dùng trả về từ repo
        //        response.Result = users;
        //        response.StatusCode = HttpStatusCode.OK;
        //        response.IsSuccess = true;
        //    }
        //    catch (Exception ex)
        //    {
        //        response.IsSuccess = false;
        //        response.StatusCode = HttpStatusCode.InternalServerError;
        //        response.ErrorMessages.Add($"Error occurred while fetching all users: {ex.Message}");
        //    }

        //    return response;
        //}

        public async Task<LoginResponseDTO> GenerateJwtToken(IdentityUser user)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var userRoles = await _userManager.GetRolesAsync(user);
            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role.ToString()));
            }

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                expires: DateTime.Now.AddHours(1),
                claims: authClaims,
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return new LoginResponseDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            };
        }

        public async Task<ApiResponse> LoginAsync(LoginRequestDTO loginRequestDTO)
        {
            _logger.LogInformation("Login attempt for username: {Username}", loginRequestDTO.Username);
            var response = new ApiResponse();

            try
            {
                var user = await _userManager.FindByNameAsync(loginRequestDTO.Username);
                if (user == null)
                {
                    _logger.LogWarning("Login failed - User not found: {Username}", loginRequestDTO.Username);
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.Unauthorized;
                    response.ErrorMessages.Add("Invalid username or password.");
                    return response;
                }
                if (!await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password))
                {
                    _logger.LogWarning("Login failed - Invalid password for user: {Username}", loginRequestDTO.Username);
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.Unauthorized;
                    response.ErrorMessages.Add("Invalid username or password.");
                    return response;
                }

                var token = await GenerateJwtToken(user);
                _logger.LogInformation("Login successful for user: {Username}", loginRequestDTO.Username);
                response.IsSuccess = true;
                response.StatusCode = HttpStatusCode.OK;
                response.Result = token;
            }
            catch (Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ErrorMessages.Add(ex.Message);
            }
            return response;          
        }

        

        public async Task<ApiResponse> RegisterAsync(RegisterRequestDTO registerRequestDTO)
        {
            _logger.LogInformation("Registration attempt for username: {Username}", registerRequestDTO.Username);
            var response = new ApiResponse();

            try
            {
                var user = _mapper.Map<ApplicationUser>(registerRequestDTO);
                _logger.LogDebug("Mapped registration request to ApplicationUser for {Username}", registerRequestDTO.Username);
                var result = await _userRepository.RegisterAsync(user, registerRequestDTO.Password, registerRequestDTO.Roles);
                if (result)
                {
                    _logger.LogInformation("Registration successful for user: {Username}", registerRequestDTO.Username);
                    response.IsSuccess = result;
                    response.StatusCode = HttpStatusCode.OK;
                    response.Result = "User registered successfully";
                }
                else
                {
                    _logger.LogWarning("Registration failed for user: {Username}", registerRequestDTO.Username);
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.ErrorMessages.Add("Registration failed");
                }
            } 
            catch(Exception ex)
            {
                _logger.LogError(ex, "Error during registration for user {Username}: {ErrorMessage}",
                    registerRequestDTO.Username, ex.Message);
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ErrorMessages.Add(ex.Message);
            }
            return response;
        }

        public async Task<ApiResponse> ResetPasswordAsync(string email)
        {
            var response = new ApiResponse();

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                response.StatusCode = HttpStatusCode.NotFound;
                response.IsSuccess = false;
                response.ErrorMessages.Add("Email không tồn tại.");
                return response;
            }

            // Tạo token đặt lại mật khẩu
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            var encodedToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(resetToken));
            var resetUrl = $"https://yourfrontend.com/reset-password?token={resetToken}&email={email}";

            // Gửi email đặt lại mật khẩu
            var emailResponse = await _emailService.SendEmailAsync(email, "Reset Password",
                $"<p>Click vào link sau để đặt lại mật khẩu: <a href='{resetUrl}'>Đặt lại mật khẩu</a></p>");

            if (!emailResponse.IsSuccess)
            {
                return emailResponse;
            }

            response.StatusCode = HttpStatusCode.OK;
            response.IsSuccess = true;
            response.Result = "Email đặt lại mật khẩu đã được gửi.";
            return response;
        }

    
       
    }
}
