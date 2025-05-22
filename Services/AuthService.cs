using YonoClothesShop.TokenGenerator;
using YonoClothesShop.Interfaces.ServicesInterfaces;
using YonoClothesShop.Models;
using YonoClothesShop.UnitOfWork;
using Microsoft.EntityFrameworkCore;
namespace YonoClothesShop.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<Token> Login(string email, string password)
        {
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = await _unitOfWork.UsersRepository.GetByEmail(email);

            if(user == null)
                return null;

            var isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password,user.PasswordHash);

            if(!isPasswordCorrect)
                return null;

            var accessToken = UserTokenGenerator.GenerateToken(user.Id,email,_configuration);

            var refreshToken = UserTokenGenerator.GenerateRefreshToken();

            var existingToken = await _unitOfWork.TokensRepository.Tokens
            .FirstOrDefaultAsync(t => t.UserId == user.Id);
            if(existingToken != null)
            {
                existingToken.AccessToken = accessToken;

                existingToken.RefreshToken = refreshToken;

                existingToken.AccessTokenExpiration = DateTime.UtcNow
                .AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"]));

                existingToken.RefreshTokenExpiration = DateTime.UtcNow
                .AddDays(5);

                await _unitOfWork.SaveChangesAsync();

                return existingToken;
            }
            var authResponse = new Token
            {
                UserId = user.Id,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiration = DateTime.UtcNow
                .AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"])),
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(5)
            };

            await _unitOfWork.TokensRepository.Add(authResponse);

            await _unitOfWork.SaveChangesAsync();

            return authResponse;
        }

        public async Task<Token> LoginAsAdmin(string email, string password)
        {
            if(string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
                return null;

            var user = await _unitOfWork.AdminRepository.GetByEmail(email);

            if(user == null)
                return null;

            var isPasswordCorrect = BCrypt.Net.BCrypt.Verify(password,user.Password);

            if(!isPasswordCorrect)
                return null;

            var accessToken = AdminTokenGenerator.GenerateToken(user.Id,email,_configuration);

            var refreshToken = AdminTokenGenerator.GenerateRefreshToken();

            var existingToken = await _unitOfWork.TokensRepository.Tokens
            .FirstOrDefaultAsync(t => t.UserId == user.Id);
            if(existingToken != null)
            {
                existingToken.AccessToken = accessToken;

                existingToken.RefreshToken = refreshToken;

                existingToken.AccessTokenExpiration = DateTime.UtcNow
                .AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"]));

                existingToken.RefreshTokenExpiration = DateTime.UtcNow
                .AddDays(5);

                await _unitOfWork.SaveChangesAsync();

                return existingToken;
            }
            var authResponse = new Token
            {
                UserId = user.Id,
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiration = DateTime.UtcNow
                .AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"])),
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(5)
            };

            await _unitOfWork.TokensRepository.Add(authResponse);

            await _unitOfWork.SaveChangesAsync();

            return authResponse;
        }
        public async Task<bool> LogOut(int id)
        {
            var user = await _unitOfWork.UsersRepository.GetById(id);

            if(user == null)
                return false;

            var token = await _unitOfWork.TokensRepository.Tokens
            .FirstOrDefaultAsync(t => t.UserId == user.Id);

            if(token == null || token.RefreshTokenExpiration < DateTime.UtcNow)
                return false;
            
            await _unitOfWork.TokensRepository.Delete(token.RefreshToken);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<Token> RefreshToken(int id, string refreshToken)
        {
            var user = await _unitOfWork.UsersRepository.GetById(id);

            if(user == null)
                return null;
            
            var token = await _unitOfWork.TokensRepository.Find(refreshToken);

            if(token == null || token.RefreshTokenExpiration < DateTime.UtcNow)
                return null;

            var newToken = new Token
            {
                AccessToken = UserTokenGenerator.GenerateToken(id,user.Email,_configuration),
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["Jwt:DurationInMinutes"])),
                RefreshToken = UserTokenGenerator.GenerateRefreshToken(),
                RefreshTokenExpiration = DateTime.UtcNow.AddDays(5),
                UserId = user.Id,
            };

            await _unitOfWork.TokensRepository.Delete(token.RefreshToken);

            await _unitOfWork.TokensRepository.Add(newToken);

            await _unitOfWork.SaveChangesAsync();

            return newToken;
        }
    }
}