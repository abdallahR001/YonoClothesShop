using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.RateLimiting;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using YonoClothesShop.DTOs;
using YonoClothesShop.Interfaces;
using YonoClothesShop.Models;
using YonoClothesShop.Models.RequestModels;
using YonoClothesShop.TokenGenerator;
using YonoClothesShop.UnitOfWork;

namespace YonoClothesShop.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public UserService(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }
        public async Task<bool> AddProductToCart(int userId,int productId, int quantity)
        {
            if(quantity <= 0)
                return false;

            var product = await _unitOfWork.ProductsRepository.GetById(productId);

            if(product == null)
                return false;

            if(product.Count < quantity)
                return false;

            var user = await _unitOfWork.UsersRepository.GetById(userId);

            if(user == null)
                return false;

            var exsistingCart = await _unitOfWork.CartsRepository.Carts
            .FirstOrDefaultAsync(c => c.UserId == userId);

            if(exsistingCart == null)
            {
                exsistingCart = new Cart
                {
                    UserId = user.Id,
                    cartItems = new List<CartItem>()
                };

                await _unitOfWork.CartsRepository.Add(exsistingCart);
            }

            if(exsistingCart.cartItems == null)
                exsistingCart.cartItems = new List<CartItem>();

            var exsistingCartItem = await _unitOfWork.CartItemsRepository.CartItems
            .FirstOrDefaultAsync(c => c.CartId == exsistingCart.Id && c.ProductId == productId);

            if(exsistingCartItem != null)
            {
                exsistingCartItem.Quantity += quantity;
            }
            else
            {
                var cartItem = new CartItem
                {
                    ProductId = product.Id,
                    CartId = exsistingCart.Id,
                    Name = product.Name,
                    Quantity = quantity,
                    UnitPrice = product.Price,
                    ProductImage = product.Image,
                };
            
                
                exsistingCart.cartItems.Add(cartItem);

                if(user.Amount < exsistingCart.TotalPrice)
                    return false;

                await _unitOfWork.CartItemsRepository.Add(cartItem);
            }

            exsistingCart.TotalPrice = exsistingCart.cartItems
                .Sum(c => c.Quantity * c.UnitPrice);

            if(user.Amount < exsistingCart.TotalPrice)
                    return false;

            product.Count -= quantity;
                
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public Task<ReviewDTO> AddReview(string review, int rating)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Checkout(int id)
        {
            var user = await _unitOfWork.UsersRepository.GetById(id);

            if(user == null)
                return false;

            var cart = await _unitOfWork.CartsRepository.GetCartWithCartItems(user.Id);

            if(cart == null)
                return false;

            var cartItems = await _unitOfWork.CartItemsRepository.CartItems
            .Where(c => c.CartId == cart.Id).ToListAsync();

            if(!cartItems.Any())
                return false;

            var order = new Order
            {
                UserId = user.Id,
                Address = user.Address,
                CreatedAt = DateTime.UtcNow,
                Status = "done",
                PaymentMethod = "visa",
                OrderItems = new List<OrderItem>(),
            };

            foreach(var item in cartItems)
            {
                var orderItem = new OrderItem
                {
                    ProductId = item.ProductId,
                    Name = item.Name,
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    ProductImage = item.ProductImage,
                };

                order.OrderItems.Add(orderItem);

                order.ProductsCount++;
            }

            order.TotalPrice = order.OrderItems.Sum(o => o.UnitPrice * o.Quantity);

            if(order.TotalPrice > user.Amount)
                return false;
            await _unitOfWork.OrdersRepository.Add(order);

            user.Amount -= order.TotalPrice;

            user.OrdersCount++;

            _unitOfWork.CartItemsRepository.DeleteRange(cartItems);

            await _unitOfWork.CartsRepository.Delete(cart.Id);

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public Task<bool> ClearCart()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> CreateAccount(string name, string email, string password, string address, IFormFile profileImage)
        {
            if(string.IsNullOrEmpty(name) || string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password) || string.IsNullOrEmpty(address) || profileImage == null)
                return false;
            var userExists = await _unitOfWork.UsersRepository.GetByEmail(email);
            if(userExists != null)
                return false;
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var fileName = Guid.NewGuid() + Path.GetExtension(profileImage.FileName);
            var filePath = Path.Combine(folderPath, fileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await profileImage.CopyToAsync(stream);
            }
            var imagePath = Path.Combine("images", fileName);
            
            var user = new User
            {
                Name = name,
                Email = email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                Address = address,
                ProfileImage = fileName
            };
            await _unitOfWork.UsersRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public Task<bool> DeleteAccount(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> Deposit(int id, int amount)
        {

            if(amount <= 0)
                return false;

            var user = await _unitOfWork.UsersRepository.GetById(id);

            if(user == null)
                return false;

            user.Amount += amount;

            await _unitOfWork.SaveChangesAsync();

            return true;
        }

        public async Task<UserDTO> GetAccount(int id)
        {
            var user = await _unitOfWork.UsersRepository.GetById(id);

            if(user == null)
                return null;
            
            return new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Amount = user.Amount,
                OrdersCount = user.OrdersCount,
                ProfileImage = user.ProfileImage
            };
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

        public Task<bool> RemoveProductFromCart(int productId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> UpdateAccount(string name = null, string email = null, string password = null, string address = null, IFormFile profileImage = null)
        {
            throw new NotImplementedException();
        }

        public async Task<CartDTO> ViewCart(int id)
        {
            var cart = await _unitOfWork.UsersRepository.GetUserWithCart(id);

            if(cart == null)
                return null;

            return new CartDTO
            {
                Id = cart.Id,
                UserId = cart.UserId,
                TotalPrice = cart.TotalPrice,
            };
        }
    }
}