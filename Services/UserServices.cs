using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using ScriveAPI.Data;
using ScriveAPI.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net.Mail;
using System.Net;

namespace ScriveAPI.Services
{
    public class UserServices
    {
        private readonly UserContext _dbContext;
        private readonly IMongoCollection<User> _users;
        private readonly IConfiguration _configuration;

        public UserServices(UserContext dbContext, IMongoCollection<User> users, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _users = users;
            _configuration = configuration;
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
        public async Task<User> GetUser(string id)
        {
            var user = await _users.FindAsync(u => u.Id == id).Result.FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("User not found");
            }

            user.Password = String.Empty;

            return user;
        }
        public async Task<User> UpdateUser(string id, string username, string email)
        {
            var user = await _users.FindAsync(u => u.Id == id).Result.FirstOrDefaultAsync();

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

            await _users.ReplaceOneAsync(u => u.Id == id, user);

            return user;
        }
        public async Task<User> DeleteUser(string id)
        {
            var user = await _users.FindAsync(u => u.Id == id).Result.FirstOrDefaultAsync();

            if (user == null)
            {
                throw new Exception("User not found");
            }

            await _users.DeleteOneAsync(u => u.Id == id);

            return user;
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