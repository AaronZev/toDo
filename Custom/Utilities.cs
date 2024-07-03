using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Runtime.Intrinsics.Arm;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using toDoList.Models;

namespace toDoList.Custom
{
    public class Utilities
    {
        private readonly IConfiguration _configuration;
        public Utilities(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string encrypt(string texto)
        {
            using (SHA256 sha256Hash = SHA256.Create()) {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto));

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++){
                    sb.Append(bytes[i].ToString("x2"));
                }

                return sb.ToString();
            }
        }

        public string generateJwt(W_USER model)
        {
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,model.C_ID_USER.ToString()),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var jwtConfig = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }
    }
}
