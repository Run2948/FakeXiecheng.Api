using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.Api.Models.Dtos
{
    public class LineItemDto
    {
        public int Id { get; set; }
        public Guid TouristRouteId { get; set; }
        public TouristRouteDto TouristRoute { get; set; }
        public Guid? ShoppingCartId { get; set; }
        public Guid? OrderId { get; set; }
        public decimal OriginalPrice { get; set; }
        public double? DiscountPresent { get; set; }
    }

    public class ShoppingCartDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public ICollection<LineItemDto> ShoppingCartItems { get; set; }
    }

    public class AddShoppingCartItemDto
    {
        public Guid TouristRouteId { get; set; }
    }
}
