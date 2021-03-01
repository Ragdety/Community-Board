using CommunityBoard.BackEnd.Options;
using CommunityBoard.Core.DomainObjects;
using CommunityBoard.Core.Interfaces;
using CommunityBoard.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CommunityBoard.BackEnd.Repositories
{
    public class IdentityRepository : IIdentityRepository
    {
        private readonly UserManager<User> _userManager;
        private readonly JwtSettings _jwtSettings;

        public IdentityRepository(
            UserManager<User> userManager, 
            JwtSettings jwtSettings)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
        }

        public async Task<AuthenticationResult> RegisterAsync(
            string firstName, string lastName,
            string userName, string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if(existingUser != null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User with this email address already exists" }
                };
            }

            existingUser = await _userManager.FindByNameAsync(userName);
            if (existingUser != null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User with this username already exists" }
                };
            }

            var newUser = new User
            { 
                FirstName = firstName,
                LastName = lastName,
                UserName = userName,
                Email = email,
                DateRegistered = DateTime.UtcNow
            };

            //This will auto-hash password
            var createdUser = await _userManager.CreateAsync(newUser, password);

            if(!createdUser.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = createdUser.Errors.Select(e => e.Description)
                };
            }

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                    new Claim("id", newUser.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}