using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ToDoList.Context;
using ToDoList.DTO;

namespace ToDoList.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TodoDBContext _context;
        public readonly AppSettingDTO _appsettings;

        public UserRepository(IOptions<AppSettingDTO> appsettings,
            TodoDBContext context)
        {
            _appsettings = appsettings.Value;
            _context = context;
        }

        public bool GetToken(UserDTO user)
        {
            var userInDB = _context.Users.FirstOrDefault(x => x.Username == user.Username && x.Password == user.Password);
            int Variable = userInDB != null ? 1 : 0;

            if (Variable == 1)
            {
                var tokenhandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
                var keys = System.Text.Encoding.ASCII.GetBytes(_appsettings.key);
                var tokendescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
                {
                    Subject = new System.Security.Claims.ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.Role,"Admin"),
                new Claim(ClaimTypes.Version,"v2.1")
                }),
                    Expires = DateTime.UtcNow.AddHours(3),
                    SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(keys), Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)

                };
                var tokens = tokenhandler.CreateToken(tokendescriptor);

                user.Token = tokenhandler.WriteToken(tokens);
                user.Password = null;
                return Variable == 1;

            }
            return false;
        }
    }
}
