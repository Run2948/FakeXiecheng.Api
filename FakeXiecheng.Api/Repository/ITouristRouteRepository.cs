using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeXiecheng.Api.Helper;
using FakeXiecheng.Api.Models;

namespace FakeXiecheng.Api.Repository
{
    public interface ITouristRouteRepository
    {
        Task<PaginationList<TouristRoute>> GetTouristRoutesAsync(string keyword, string ratingOperator, int? ratingValue, int pageNumber, int pageSize);

        Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId);

        Task<bool> TouristRouteExistsAsync(Guid touristRouteId);

        Task<IEnumerable<TouristRoutePicture>> GetTouristRoutePicturesAsync(Guid touristRouteId);

        Task<TouristRoutePicture> GetTouristRoutePictureAsync(int pictureId);

        void AddTouristRoute(TouristRoute touristRoute);

        void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture);

        void DeleteTouristRoute(TouristRoute touristRoute);

        void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes);

        void DeleteTouristRoutePicture(TouristRoutePicture picture);

        // Task<ShoppingCart> GetShoppingCartByUserId(string userId);
        // Task CreateShoppingCart(ShoppingCart shoppingCart);
        // Task AddShoppingCartItem(LineItem lineItem);
        // Task<LineItem> GetShoppingCartItemByItemId(int lineItemId);
        // void DeleteShoppingCartItem(LineItem lineItem);
        // Task<IEnumerable<LineItem>> GetShoppingCartsByIdListAsync(IEnumerable<int> lineItemIds);
        // void DeleteShoppingCartItems(IEnumerable<LineItem> lineItems);
        // Task AddOrderAsync(Order order);
        // Task<PaginationList<Order>> GetOrdersByUserId(string userId, int pageNumber, int pageSize);
        // Task<Order> GetOrderByOrderId(Guid orderId);

        Task<bool> SaveAsync();
    }
}
