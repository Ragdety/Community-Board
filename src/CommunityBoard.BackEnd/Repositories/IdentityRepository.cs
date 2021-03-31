using CommunityBoard.BackEnd.Data;
using CommunityBoard.BackEnd.Options;
using CommunityBoard.BackEnd.Utilities;
using CommunityBoard.Core.DomainObjects;
using CommunityBoard.Core.DTOs;
using CommunityBoard.Core.Interfaces.Repositories;
using CommunityBoard.Core.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
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
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly ApplicationDbContext _db;

		public IdentityRepository(
			UserManager<User> userManager,
			JwtSettings jwtSettings,
			TokenValidationParameters tokenValidationParameters,
			ApplicationDbContext db)
		{
			_userManager = userManager;
			_jwtSettings = jwtSettings;
			_tokenValidationParameters = tokenValidationParameters;
			_db = db;
		}

		public async Task<AuthenticationResult> LoginAsync(string emailOrUserName, string password)
        {
            User user;
            if (UserUtilities.IsValidEmail(emailOrUserName))
                user = await _userManager.FindByEmailAsync(emailOrUserName);
            else
                user = await _userManager.FindByNameAsync(emailOrUserName);

            if (user == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "User does not exist" }
                };
            }

            var validPassword = await _userManager.CheckPasswordAsync(user, password);

            if (!validPassword)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "Incorrect Password" }
                };
            }

            return await GenerateAuthResultAsync(user);
        }

        public async Task<AuthenticationResult> RegisterAsync(
            string firstName, string lastName,
            string userName, string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
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

            var createdUserRole = await _userManager.AddToRoleAsync(newUser, "User");
            if(!createdUserRole.Succeeded)
            {
                return new AuthenticationResult
                {
                    Errors = createdUserRole.Errors.Select(e => e.Description)
                };
            }

            return await GenerateAuthResultAsync(newUser);
        }

        public async Task<UserDto> FindUserById(int userId)
        {
            var user = await _userManager.FindByIdAsync(userId.ToString());

            if (user == null)
                return null;

            var userDto = new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email
            };

            return userDto;
        }

        public async Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken)
        {
            var validationToken = GetPrincipalFromToken(token);

            //Check if it's a valid token
            if(validationToken == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "Invalid Token" }
                };
            }

            var expiryDateUnix = 
                long.Parse(validationToken.Claims.Single(x => 
                    x.Type == JwtRegisteredClaimNames.Exp).Value);

            var expiryDateUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                .AddSeconds(expiryDateUnix);

            //Check if token has expired
            if(expiryDateUtc > DateTime.UtcNow)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "This token hasn't expired yet" }
                };
            }

            var jti = validationToken.Claims.Single(x =>
                    x.Type == JwtRegisteredClaimNames.Jti).Value;

            var storedRefreshToken = 
                await _db.RefreshTokens.SingleOrDefaultAsync(x => x.Token == refreshToken);

            //Check if refresh token exists
            if (storedRefreshToken == null)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "This token doesn't exist" }
                };
            }

            //Check if refresh token has expired
            if(DateTime.UtcNow > storedRefreshToken.ExpiryDate)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "This refresh token has expired" }
                };
            }

            if(storedRefreshToken.Invalidated)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "This refresh token has been invalidated" }
                };
            }

            if(storedRefreshToken.Used)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "This refresh token has been used" }
                };
            }

            if(storedRefreshToken.JwtId != jti)
            {
                return new AuthenticationResult
                {
                    Errors = new[] { "This refresh token does not match this jwt" }
                };
            }

            storedRefreshToken.Used = true;
            _db.RefreshTokens.Update(storedRefreshToken);
            await _db.SaveChangesAsync();

            var user = await _userManager.FindByIdAsync(
                    validationToken.Claims.Single(x => x.Type == "id").Value);

            return await GenerateAuthResultAsync(user);
        }

        private async Task<IEnumerable<string>> GetUserRoles(User user)
		{
            return await _userManager.GetRolesAsync(user);
		}

        private ClaimsPrincipal GetPrincipalFromToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            try
            {
                var principal = tokenHandler.ValidateToken(token, _tokenValidationParameters, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                    return null;
                return principal;
            }
            catch
            {
                return null;
            }
        }

        private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                    jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, 
                    StringComparison.InvariantCultureIgnoreCase);
        }

        private async Task<AuthenticationResult> GenerateAuthResultAsync(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim("id", user.Id.ToString()),
                    new Claim("username", user.UserName.ToString()),
                    new Claim("firstName", user.FirstName.ToString()),
                    new Claim("lastName", user.LastName.ToString()),
                }),
                Expires = DateTime.UtcNow.Add(_jwtSettings.TokenLifetime),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };

			foreach (string role in await GetUserRoles(user))
			{
                tokenDescriptor.Subject.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            var token = tokenHandler.CreateToken(tokenDescriptor);

            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                JwtId = token.Id,
                UserId = user.Id,
                CreationDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };

            await _db.RefreshTokens.AddAsync(refreshToken);
            await _db.SaveChangesAsync();

            return new AuthenticationResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
                RefreshToken = refreshToken.Token
            };
        }	
	}
}