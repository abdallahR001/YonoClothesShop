using YonoClothesShop.DTOs;
namespace YonoClothesShop.Interfaces.ServicesInterfaces
{
    public interface ICartService
    {
        public Task<bool> AddProductToCart(int userId,int productId, int quantity);
        public Task<int> RemoveProductFromCart(int userId,int productId);
        public Task<CartDTO> ViewCart(int id);
        public Task<bool> ClearCart(int id);
        public Task<bool> Checkout(int id);
    }
}