using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Entities;
using WebApi.Helpers;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Options;
using WebApi.Services.UserService.Dto;
using WebApi.Services.PersonService;
using System.Threading.Tasks;
using WebApi.Services.PersonService.Dto;

namespace WebApi.Services.UserService
{


    public class UserService : IUserService
    {
        private DataContext _context;
        private readonly AppSettings _appSettings;
        private readonly IPersonService _personService;
        public UserService(DataContext context, IOptions<AppSettings> appSettings, IPersonService personService)
        {
            _context = context;
            _appSettings = appSettings.Value;
            _personService = personService;
        }

        public async Task<UserDto> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                return null;

            var user = _context.Users
                .Include( c => c.RoleUsers)
                .ThenInclude( r => r.Role)
                .SingleOrDefault(x => x.Username == username);

            string[] roles = new string[]{}; 
            if(user.RoleUsers.Any())
                roles = user.RoleUsers.Select( x => x.Role ).Select( S => S.RoleName).ToArray();

            if (user == null)
                return null;

            if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;



            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.Name, user.Id.ToString()));
            foreach (var item in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, item));
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            // authentication successful
            var userDto = new UserDto();
            userDto.Id = user.Id;
            userDto.FirstName  = user.FirstName;
            userDto.LastName   = user.LastName;
            userDto.Username   = user.Username;
            userDto.Token = tokenString;
            return userDto;
        }

        public async Task<List<RegisterUserDto>> GetAll()
        {
            var users = _context.Users.Select( x => new RegisterUserDto
            {
                FirstName = x.FirstName,
                LastName = x.LastName,
                Username = x.Username
            }).ToList();
            return users;
        }

        public async Task<User> GetById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task<User> Create(RegisterUserDto UserData)
        {
            if (string.IsNullOrWhiteSpace(UserData.Password))
                throw new AppException("Password is required");

            if (_context.Users.Any(x => x.Username == UserData.Username))
                throw new AppException("Username \"" + UserData.Username + "\" is already taken");

            var role = _context.Roles.FirstOrDefault(x => x.RoleName == UserData.Role);
            if (role == null)
                throw new AppException("Role '" + UserData.Role + "' does not exist");

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(UserData.Password, out passwordHash, out passwordSalt);

            var user = new User
            {
                FirstName = UserData.FirstName,
                LastName = UserData.LastName,
                Username = UserData.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _context.Users.Add(user);
            _context.SaveChanges();

            var roleUser = new RoleUser(){
                UserId = user.Id,
                RoleId = role.Id
            };
            _context.RoleUsers.Add(roleUser);
            _context.SaveChanges();
            await CreateRelatedPerson(user);

            return user;
        }

        private async Task CreateRelatedPerson(User user)
        {
            var person = new PersonDto() { 
                FirstName = user.FirstName,
                LastName = user.LastName,
            };

            await _personService.Create(person);
        }


        public async Task Update(User userParam, string password = null)
        {
            var user = await _context.Users.FindAsync(userParam.Id);

            if (user == null)
                throw new AppException("User not found");

            // update username if it has changed
            if (!string.IsNullOrWhiteSpace(userParam.Username) && userParam.Username != user.Username)
            {
                // throw error if the new username is already taken
                if (_context.Users.Any(x => x.Username == userParam.Username))
                    throw new AppException("Username " + userParam.Username + " is already taken");

                user.Username = userParam.Username;
            }

            // update user properties if provided
            if (!string.IsNullOrWhiteSpace(userParam.FirstName))
                user.FirstName = userParam.FirstName;

            if (!string.IsNullOrWhiteSpace(userParam.LastName))
                user.LastName = userParam.LastName;

            // update password if provided
            if (!string.IsNullOrWhiteSpace(password))
            {
                byte[] passwordHash, passwordSalt;
                CreatePasswordHash(password, out passwordHash, out passwordSalt);

                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;
            }

            _context.Users.Update(user);
            _context.SaveChanges();
        }

        public async Task Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        // private helper methods

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");

            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private static bool VerifyPasswordHash(string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new ArgumentNullException("password");
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            if (storedHash.Length != 64) throw new ArgumentException("Invalid length of password hash (64 bytes expected).", "passwordHash");
            if (storedSalt.Length != 128) throw new ArgumentException("Invalid length of password salt (128 bytes expected).", "passwordHash");

            using (var hmac = new System.Security.Cryptography.HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (int i = 0; i < computedHash.Length; i++)
                {
                    if (computedHash[i] != storedHash[i]) return false;
                }
            }

            return true;
        }

        public int GetUserId(){

            var claims = ClaimsPrincipal.Current.Identities.First().Claims.ToList();   
            var UserId = claims?.FirstOrDefault(x => x.Type.Equals(ClaimTypes.Name, StringComparison.OrdinalIgnoreCase))?.Value;
            return int.Parse(UserId);
        }


    }
}