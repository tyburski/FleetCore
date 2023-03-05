using FleetCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FleetCore.Services
{
    public interface IJWTManager
    {
        Tokens Authenticate(LoginModel model);
    }
    public class JWTManager : IJWTManager
    {

        private readonly IConfiguration _iconfiguration;
        private readonly FleetCoreDbContext _dbContext;

        public JWTManager(IConfiguration iconfiguration, FleetCoreDbContext dbContext)
        {
            _iconfiguration = iconfiguration;
            _dbContext = dbContext;
        }
        public Tokens Authenticate(LoginModel model)
        {
            var user = _dbContext
                .Users
                .Include(x => x.Organization)
                .FirstOrDefault(x => x.UserName == model.UserName && x.Password == model.Password);

            if (user is null)
            {
                return null;
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(_iconfiguration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                     new Claim(ClaimTypes.Name, user.FullName),
                     new Claim(ClaimTypes.Role, user.Role),
                     new Claim("Organization", user.Organization.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new Tokens { Token = tokenHandler.WriteToken(token) };

        }
    }
}
