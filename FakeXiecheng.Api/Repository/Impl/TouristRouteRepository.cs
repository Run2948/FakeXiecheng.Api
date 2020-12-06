using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeXiecheng.Api.Common.Helper;
using FakeXiecheng.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace FakeXiecheng.Api.Repository.Impl
{
    public class TouristRouteRepository : ITouristRouteRepository
    {
        private readonly AppDbContext _context;

        public TouristRouteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PaginationList<TouristRoute>> GetTouristRoutesAsync(string keyword, string ratingOperator, int? ratingValue, int pageNumber, int pageSize)
        {
            IQueryable<TouristRoute> query = _context.TouristRoutes.Include(t => t.TouristRoutePictures);
            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim();
                query = query.Where(t => t.Title.Contains(keyword));
            }
            if (ratingValue != null && ratingValue >= 0)
            {
                query = ratingOperator switch
                {
                    "largerThan" => query.Where(t => t.Rating >= ratingValue),
                    "lessThan" => query.Where(t => t.Rating <= ratingValue),
                    _ => query.Where(t => t.Rating == ratingValue)
                };
            }
            return await PaginationList<TouristRoute>.CreateAsync(pageNumber, pageSize, query);
        }

        public async Task<IEnumerable<TouristRoute>> GetTouristRoutesAsync(IEnumerable<Guid> ids)
        {
            return await _context.TouristRoutes.Where(t => ids.Contains(t.Id)).ToListAsync();
        }

        public async Task<TouristRoute> GetTouristRouteAsync(Guid touristRouteId)
        {
            // return await _context.TouristRoutes.FindAsync(touristRouteId);
            return await _context.TouristRoutes.Include(t => t.TouristRoutePictures).FirstOrDefaultAsync(t => t.Id == touristRouteId);
        }

        public async Task<bool> TouristRouteExistsAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutes.AnyAsync(t => t.Id == touristRouteId);
        }

        public async Task<IEnumerable<TouristRoutePicture>> GetTouristRoutePicturesAsync(Guid touristRouteId)
        {
            return await _context.TouristRoutePictures.Where(p => p.TouristRouteId == touristRouteId).ToListAsync();
        }

        public async Task<TouristRoutePicture> GetTouristRoutePictureAsync(int pictureId)
        {
            // return await _context.TouristRoutePictures.FindAsync(pictureId);
            return await _context.TouristRoutePictures.Where(p => p.Id == pictureId).FirstOrDefaultAsync();
        }

        public void AddTouristRoute(TouristRoute touristRoute)
        {
            if (touristRoute == null)
            {
                throw new ArgumentNullException(nameof(touristRoute));
            }
            _context.TouristRoutes.Add(touristRoute);
            //_context.SaveChanges();
        }

        public void AddTouristRoutePicture(Guid touristRouteId, TouristRoutePicture touristRoutePicture)
        {
            if (touristRouteId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(touristRouteId));
            }
            if (touristRoutePicture == null)
            {
                throw new ArgumentNullException(nameof(touristRoutePicture));
            }
            touristRoutePicture.TouristRouteId = touristRouteId;
            _context.TouristRoutePictures.Add(touristRoutePicture);
        }

        public void DeleteTouristRoute(TouristRoute touristRoute)
        {
            _context.TouristRoutes.Remove(touristRoute);
        }

        public void DeleteTouristRoutes(IEnumerable<TouristRoute> touristRoutes)
        {
            _context.TouristRoutes.RemoveRange(touristRoutes);
        }

        public void DeleteTouristRoutePicture(TouristRoutePicture picture)
        {
            _context.TouristRoutePictures.Remove(picture);
        }

        public async Task<ShoppingCart> GetUserShoppingCart(string userId)
        {
            return await _context.ShoppingCarts.Include(s => s.User).Include(s => s.ShoppingCartItems)
                .ThenInclude(li => li.TouristRoute)
                .FirstOrDefaultAsync(s => s.UserId == userId);
        }

        public async Task CreateShoppingCart(ShoppingCart shoppingCart)
        {
            await _context.ShoppingCarts.AddAsync(shoppingCart);
        }

        public async Task AddShoppingCartItem(LineItem lineItem)
        {
            await _context.LineItems.AddAsync(lineItem);
        }

        public async Task<LineItem> GetShoppingCartItem(int lineItemId)
        {
            return await _context.LineItems.Where(li => li.Id == lineItemId).FirstOrDefaultAsync();
        }

        public void DeleteShoppingCartItem(LineItem lineItem)
        {
            _context.LineItems.Remove(lineItem);
        }

        public async Task<IEnumerable<LineItem>> GetShoppingCartItemsAsync(IEnumerable<int> lineItemIds)
        {
            return await _context.LineItems.Where(li => lineItemIds.Contains(li.Id)).ToListAsync();
        }

        public void DeleteShoppingCartItems(IEnumerable<LineItem> lineItems)
        {
            _context.LineItems.RemoveRange(lineItems);
        }

        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task<PaginationList<Order>> GetUserOrders(string userId, int pageNumber, int pageSize)
        {
            IQueryable<Order> result = _context.Orders.Where(o => o.UserId == userId);
            return await PaginationList<Order>.CreateAsync(pageNumber, pageSize, result);
        }

        public async Task<Order> GetOrder(Guid orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems).ThenInclude(oi => oi.TouristRoute)
                .Where(o => o.Id == orderId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> SaveAsync()
        {
            return await _context.SaveChangesAsync() >= 0;
        }
    }
}
