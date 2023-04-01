using FleetCore.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FleetCore.Services
{
    public interface IJWTManager
    {
        Dictionary<int, string>? ValidateToken(string token);
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

        public Dictionary<int, string>? ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_iconfiguration["JWT:Key"]);
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = _iconfiguration["JWT:Issuer"],
                    ValidAudience = _iconfiguration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;

                var fullname = jwtToken.Claims.First(x => x.Type == "Name").Value;
                var role = jwtToken.Claims.First(x => x.Type == "Role").Value;
                var userId = jwtToken.Claims.First(x => x.Type == "UserId").Value;

                Dictionary<int, string> datas = new Dictionary<int, string>
                {
                    { 0, fullname },
                    { 1, role },
                    { 2, userId }
                };

                return datas;
            }
            catch
            {
                return null;
            }
        }
    }
}
