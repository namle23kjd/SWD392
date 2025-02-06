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
        public UserService(IUserRepository userRepository, IMapper mapper, UserManager<IdentityUser> userManager, IConfiguration configuration)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _userManager = userManager;
            _configuration = configuration;
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
            var response = new ApiResponse();

            try
            {
                var user = await _userManager.FindByNameAsync(loginRequestDTO.Username);
                if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequestDTO.Password))
                {
                    response.IsSuccess = false;
                    response.StatusCode = HttpStatusCode.Unauthorized;
                    response.ErrorMessages.Add("Invalid username or password.");
                    return response;
                }
                var token = await GenerateJwtToken(user);

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
            var response = new ApiResponse();

            try
            {
                var user = _mapper.Map<ApplicationUser>(registerRequestDTO);

                var result = await _userRepository.RegisterAsync(user, registerRequestDTO.Password, registerRequestDTO.Roles);

                response.IsSuccess = result;
                response.StatusCode = HttpStatusCode.OK;
                response.Result = "User registered successfully";
            } 
            catch(Exception ex)
            {
                response.IsSuccess = false;
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.ErrorMessages.Add(ex.Message);
            }
            return response;
        }
    }
}
