using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ScriveAPI.Helpers
{
    public class TokenValidator
    {
        private readonly string _jwtSecret;

        public TokenValidator(string jwtSecret)
        {
            _jwtSecret = jwtSecret;
        }

        public ClaimsPrincipal ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtSecret);
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                return principal;
            }
            catch (Exception ex)
            {
                throw new Exception("Invalid or expired token", ex);
            }
        }

        public string ExtractUserIdFromToken(string token)
        {
            var claimsPrincipal = ValidateToken(token);
            var userClaim = claimsPrincipal.Claims.FirstOrDefault(c => c.Type == "user_id");
            if (userClaim == null)
            {
                throw new Exception("Invalid or expired token");
            }

            return userClaim.Value;
        }
    }
}
