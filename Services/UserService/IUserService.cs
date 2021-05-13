using System.Collections.Generic;
using WebApi.Entities;
using WebApi.Services.UserService.Dto;

namespace WebApi.Services.UserService
{

        public interface IUserService
    {
        UserDto Authenticate(string username, string password);
        IEnumerable<User> GetAll();
        User GetById(int id);
        User Create(RegisterUserDto user);
        void Update(User user, string password = null);
        void Delete(int id);
        int GetUserId();
    }

}