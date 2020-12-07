using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeXiecheng.Api.Common.Helper;
using FakeXiecheng.Api.Models;

namespace FakeXiecheng.Api.Repository
{
    public interface ITouristRouteRepository
    {
        Task<PaginationList<TouristRoute>> GetTouristRoutesAsync(string keyword, string ratingOperator, int? ratingValue, int pageNumber, int pageSize, string orderBy);

        Task<IEnumerable<TouristRoute>> GetTouristRoutesAsync(IEnumerable<Guid> ids);

        Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId);

        Task<bool> TouristRouteExistsAsync(Guid touristRouteId);

        Task<IEnumerable<TouristRoutePicture>> GetTouristRoutePicturesAsync(Guid touristRouteId);

        Task<TouristRoutePicture> GetTouristRoutePictureAsync(int pictureId);

        void AddTouristRoute(TouristRoute touristRoute);

        void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture);

        void DeleteTouristRoute(TouristRoute touristRoute);

        void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes);

        void DeleteTouristRoutePicture(TouristRoutePicture picture);

        Task<ShoppingCart> GetUserShoppingCart(string userId);

        Task CreateShoppingCart(ShoppingCart shoppingCart);

        Task AddShoppingCartItem(LineItem lineItem);

        Task<LineItem> GetShoppingCartItem(int lineItemId);

        void DeleteShoppingCartItem(LineItem lineItem);

        Task<IEnumerable<LineItem>> GetShoppingCartItemsAsync(IEnumerable<int> lineItemIds);

        void DeleteShoppingCartItems(IEnumerable<LineItem> lineItems);

        Task AddOrderAsync(Order order);

        Task<PaginationList<Order>> GetUserOrders(string userId, int pageNumber, int pageSize);

        Task<Order> GetOrder(Guid orderId);

        Task<bool> SaveAsync();
    }
}
