using YonoClothesShop.Interfaces.ServicesInterfaces;
using YonoClothesShop.UnitOfWork;
using YonoClothesShop.DTOs;
using Microsoft.EntityFrameworkCore;
using YonoClothesShop.Models;
namespace YonoClothesShop.Services
{
    public class CartService : ICartService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CartService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
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
                    cart = exsistingCart
                };
            
                exsistingCart.cartItems.Add(cartItem);

                if(user.Amount < exsistingCart.TotalPrice)
                    return false;

                await _unitOfWork.CartItemsRepository.Add(cartItem);
            }

            exsistingCart.TotalPrice += exsistingCart.cartItems
                .Sum(c => c.Quantity * c.UnitPrice);

            if(user.Amount < exsistingCart.TotalPrice)
                    return false;

            product.Count -= quantity;
                
            await _unitOfWork.SaveChangesAsync();

            return true;
        }
        public async Task<CartDTO> ViewCart(int id)
        {
            var cart = await _unitOfWork.UsersRepository.GetUserCart(id);

            if(cart == null)
                return null;

            return new CartDTO
            {
                Id = cart.Id,
                UserId = cart.UserId,
                TotalPrice = cart.TotalPrice,
            };
        }
        public async Task<int> RemoveProductFromCart(int userId,int productId)
        {
            var cart = await _unitOfWork.UsersRepository.GetUserCart(userId);

            if(cart == null)
                return 0;

            var cartItems = await _unitOfWork.CartItemsRepository.GetCartItems(cart.Id);

            if(cartItems == null || !cartItems.Any())
                return -1;

            var cartItem = cartItems.FirstOrDefault(c => c.ProductId == productId);

            if(cartItem == null)
                return -2;

            var product = await _unitOfWork.ProductsRepository.GetById(cartItem.ProductId);

            product.Count += cartItem.Quantity;

            await _unitOfWork.CartItemsRepository.Delete(cartItem.Id);

            cart.cartItems.Remove(cartItem);

            cart.TotalPrice = cart.cartItems.Sum(c => c.Quantity * c.UnitPrice);

            if(cart.TotalPrice <= 0)
                await _unitOfWork.CartsRepository.Delete(cart.Id);

            await _unitOfWork.SaveChangesAsync();

            return 1;
        }
        public async Task<bool> ClearCart(int id)
        {
            var cart = await _unitOfWork.UsersRepository.GetUserCart(id);

            if(cart == null)
                return false;

            var cartItems = await _unitOfWork.CartItemsRepository.GetCartItems(cart.Id);

            foreach (var item in cartItems)
            {
                var product = await _unitOfWork.ProductsRepository.GetById(item.ProductId);

                product.Count += item.Quantity;
            }

            _unitOfWork.CartItemsRepository.DeleteRange(cartItems);

            await _unitOfWork.CartsRepository.Delete(cart.Id);

            await _unitOfWork.SaveChangesAsync();

            return true;
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
    }
}