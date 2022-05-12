using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using Final_Project_Assignment;

namespace Final_Project_Assignment
{
    public class JwtAuthenticationManager
    {
        private readonly string key;

        private readonly IDictionary<string, string> users = new Dictionary<string, string>()
        { {"test", "password"}, {"1234567891", "passwordA"}, {"user", "12345"}, {"", ""} };

        public JwtAuthenticationManager(string key)
        {
            this.key = key;
        }

        public string Authenticate(string username, string password)
        {
            try
            {
                var isNumeric = int.TryParse(password, out int n);
                if (isNumeric)
                {
                    return "Password cannot contain numbers";
                }
                else
                {
                    if (username.Length >= 10)
                    {
                        return "Username is too long";
                    }
                    else
                    {
                        if (!users.Any(u => u.Key == username && u.Value == password))
                        {
                            return null;
                        }
                    }
                }
            }
            catch (ArgumentNullException ex)
            {
                throw new ArgumentNullException("username and password cannot be null", ex);
            }

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.ASCII.GetBytes(key);
            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, username)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(tokenKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
