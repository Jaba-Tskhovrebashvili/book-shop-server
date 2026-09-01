using FirstProject.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FirstProject.Helper
{
    public class AuthHelper
    {
        private readonly IConfiguration _configuration;
        public  AuthHelper(IConfiguration configuration) { 
        this._configuration= configuration;
        }

        public async Task<string> GenerateTokenRegister(Admin admin)
        {
            var claims = new List<Claim> {
         new Claim("Name", admin.Name),
         new Claim("Surname", admin.Surname),
         new Claim("Email",admin.Email)
        };

            var jwtToken = new JwtSecurityToken(
               claims: claims,
               notBefore: DateTime.UtcNow,
               expires: DateTime.UtcNow.AddMinutes(5),
               signingCredentials: new SigningCredentials(
                   new SymmetricSecurityKey(
                      Encoding.UTF8.GetBytes(this._configuration["JWT:Key"])
                       ),
                   SecurityAlgorithms.HmacSha256Signature)
               );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

        public async Task<Admin> VerifyJWTToken(string token)
        {


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]); 
            try
            {
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ClockSkew = TimeSpan.Zero // Set clockskew to zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                var Name = jwtToken.Claims.First(x => x.Type == "Name").Value; 
                var Surname = jwtToken.Claims.First(x => x.Type == "Surname").Value;
                var Email = jwtToken.Claims.First(x => x.Type == "Email").Value;


                var obj = new Admin
                {
                    Name= Name,
                    Surname = Surname,
                    Email= Email
                };

                return obj;

            }
            catch(Exception ex)
            {
                throw new Exception("ტოკენი არასწორია ან ვადა გასულია.");

            }

        }

        public async Task<string> GenerateAdminToken(int adminId)
        {
            var claims = new List<Claim> {
         new Claim("Id", adminId.ToString()),
         new Claim(ClaimTypes.Role, "Admin")
        };

            var jwtToken = new JwtSecurityToken(
               claims: claims,
               notBefore: DateTime.UtcNow,
               expires: DateTime.UtcNow.AddDays(20),
               signingCredentials: new SigningCredentials(
                   new SymmetricSecurityKey(
                      Encoding.UTF8.GetBytes(this._configuration["JWT:Key"])
                       ),
                   SecurityAlgorithms.HmacSha256Signature)
               );
            return new JwtSecurityTokenHandler().WriteToken(jwtToken);
        }

    }
}
