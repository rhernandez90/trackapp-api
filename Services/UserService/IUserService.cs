using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.Entities;
using WebApi.Services.UserService.Dto;

namespace WebApi.Services.UserService
{

    public interface IUserService
    {
        Task<UserDto> Authenticate(string username, string password);
        Task<List<RegisterUserDto>> GetAll();
        Task<User> GetById(int id);
        Task<User> Create(RegisterUserDto user);
        Task Update(User user, string password = null);
        Task Delete(int id);
        int GetUserId();
    }

}