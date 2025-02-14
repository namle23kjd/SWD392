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
        public UserService(IUserRepository userRepository, IMapper mapper, UserManager<IdentityUser> userManager, IConfiguration configuration, ILogger<UserService> logger)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _userManager = userManager;
            _configuration = configuration;
            _logger = logger;
        }

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
    }
}
