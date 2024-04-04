using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using ScriveAPI.Data;
using ScriveAPI.Models;
using MongoDB.Bson;
using ScriveAPI.Helpers;

namespace ScriveAPI.Services
{
    public class UserServices
    {
        private readonly UserContext _dbContext;
        private readonly IMongoCollection<User> _users;
        private readonly IConfiguration _configuration;
        private readonly TokenValidator _tokenValidator;

        public UserServices(UserContext dbContext, IMongoCollection<User> users, IConfiguration configuration, TokenValidator tokenValidator)
        {
            _dbContext = dbContext;
            _users = users;
            _configuration = configuration;
            _tokenValidator = tokenValidator;
        }
        public async Task<User> Register(string username, string email, string password, string profilePicture)
        {
            var existingUser = await _users.FindAsync(u => u.Email == email).Result.FirstOrDefaultAsync();
            var existingUsername = await _users.FindAsync(u => u.Username == username).Result.FirstOrDefaultAsync();

            if (existingUser != null)
            {
                throw new Exception("User already exists");
            }

            if (existingUsername != null)
            {
                throw new Exception("Username already exists");
            }

            var salt = BCrypt.Net.BCrypt.GenerateSalt();
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(password, salt);

            var user = new User
            {
                Id = ObjectId.GenerateNewId().ToString(),
                Username = username,
                Email = email,
                Password = hashedPassword,
                ProfilePicture = profilePicture
            };

            await _users.InsertOneAsync(user);

            return user;
        }

        public async Task<AuthResponse> Login(string email, string password)
        {
            var users = await _users.FindAsync(u => u.Email == email).Result.ToListAsync();
            var user = users.FirstOrDefault();

            if (user == null)
            {
                throw new Exception("Invalid credentials");
            }

            var isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.Password);

            if (!isPasswordValid)
            {
                throw new Exception("Invalid credentials");
            }

            var payload = new { user = new { Id = user.Id } };
            var secretKey = Encoding.UTF8.GetBytes(_configuration["JwtSecret"]);
            var token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                issuer: "ScriveAPI",
                audience: "ScriveAPI",
                claims: new[] { new Claim("user_id", user.Id.ToString()) },
                expires: DateTime.UtcNow.AddHours(7),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256)
            ));

            return new AuthResponse { User = user, Token = token };
        }
        public async Task<User> GetUser(string token)
        {
            string userId = String.Empty;

            try
            {
                userId = _tokenValidator.ExtractUserIdFromToken(token);
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            var user = await _users.FindAsync(u => u.Id == userId).Result.FirstOrDefaultAsync();

            if(user == null)
            {
                throw new Exception("User not Found!");
            }

            user.Password = String.Empty;

            return user;
        }
        public async Task<User> UpdateUser(string token, string username, string email)
        {
            try
            {
                var userId = _tokenValidator.ExtractUserIdFromToken(token);
                if (userId == null)
                {
                    throw new Exception("Invalid user ID in token");
                }

                var user = await _users.FindAsync(u => u.Id == userId).Result.FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                if (!string.IsNullOrEmpty(username))
                {
                    user.Username = username;
                }

                if (!string.IsNullOrEmpty(email))
                {
                    user.Email = email;
                }

                await _users.ReplaceOneAsync(u => u.Id == userId, user);

                user.Password = String.Empty;

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<User> DeleteUser(string token)
        {
            try
            {
                var userId = _tokenValidator.ExtractUserIdFromToken(token);
                var user = await _users.FindAsync(u => u.Id == userId).Result.FirstOrDefaultAsync();

                if (user == null)
                {
                    throw new Exception("User not found");
                }

                await _users.DeleteOneAsync(u => u.Id == userId);

                user.Password = String.Empty;

                return user;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async Task<User> ForgotPassword(string email)
        {
            var user = await _users.FindAsync(u => u.Email == email).Result.FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("User not found");
            }

            var payload = new
            {
                user = new
                {
                    Id = user.Id.ToString()
                }
            };

            var secretKey = Encoding.UTF8.GetBytes(_configuration["JwtSecret"]);
            var token = new JwtSecurityTokenHandler().WriteToken(new JwtSecurityToken(
                issuer: "ScriveAPI",
                audience: "ScriveAPI",
                claims: new[] { new Claim("user_id", user.Id.ToString()) },
                expires: DateTime.UtcNow.AddMinutes(10),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256)
            ));

            var callbackUrl = $"{_configuration["AppUrl"]}/reset-password/{token}";

            return user;
        }
    }
}