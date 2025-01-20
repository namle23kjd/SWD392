using AutoMapper;
using System.Net;
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
        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _mapper = mapper;
            _userRepository = userRepository;
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
