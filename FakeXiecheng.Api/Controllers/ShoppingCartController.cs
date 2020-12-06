using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiecheng.Api.Common.Helper;
using FakeXiecheng.Api.Models;
using FakeXiecheng.Api.Models.Dtos;
using FakeXiecheng.Api.Models.Enums;
using FakeXiecheng.Api.Repository;
using Microsoft.AspNetCore.Authorization;

namespace FakeXiecheng.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingCartController : ControllerBase
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public ShoppingCartController(IHttpContextAccessor httpContextAccessor, ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _httpContextAccessor = httpContextAccessor;
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Get()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var shoppingCarts = await _touristRouteRepository.GetUserShoppingCart(userId);
            return Ok(_mapper.Map<ShoppingCartDto>(shoppingCarts));
        }

        [HttpPost("items")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> AddShoppingCartItem([FromBody] AddShoppingCartItemDto addShoppingCartItemDto)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var shoppingCart = await _touristRouteRepository.GetUserShoppingCart(userId);
            var touristRouteFromRepo = await _touristRouteRepository.GetTouristRouteAsync(addShoppingCartItemDto.TouristRouteId);
            if (touristRouteFromRepo == null)
                return NotFound($"旅游路线{addShoppingCartItemDto.TouristRouteId}不存在");

            var lineItem = new LineItem()
            {
                TouristRouteId = addShoppingCartItemDto.TouristRouteId,
                ShoppingCartId = shoppingCart.Id,
                OriginalPrice = touristRouteFromRepo.OriginalPrice,
                DiscountPresent = touristRouteFromRepo.DiscountPresent
            };

            // 4 添加LineItem，并保存数据库
            await _touristRouteRepository.AddShoppingCartItem(lineItem);
            await _touristRouteRepository.SaveAsync();

            return Ok(_mapper.Map<ShoppingCartDto>(shoppingCart));
        }

        [HttpDelete("items/{itemId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteShoppingCartItem([FromRoute] int itemId)
        {
            var lineItem = await _touristRouteRepository.GetShoppingCartItem(itemId);
            if (lineItem == null)
                return NotFound($"购物车{itemId}商品找不到");
            _touristRouteRepository.DeleteShoppingCartItem(lineItem);
            await _touristRouteRepository.SaveAsync();
            return NoContent();
        }

        [HttpDelete("items/({itemIds})")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> DeleteShoppingCartItems([ModelBinder(BinderType = typeof(ArrayModelBinder))][FromRoute] IEnumerable<int> itemIds)
        {
            var lineItems = await _touristRouteRepository.GetShoppingCartItemsAsync(itemIds);
            _touristRouteRepository.DeleteShoppingCartItems(lineItems);
            await _touristRouteRepository.SaveAsync();
            return NoContent();
        }

        [HttpPost("checkout")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Checkout()
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var shoppingCart = await _touristRouteRepository.GetUserShoppingCart(userId);

            // 3 创建订单
            var order = new Order()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                State = OrderStateEnum.Pending,
                OrderItems = shoppingCart.ShoppingCartItems,
                CreateDate = DateTime.UtcNow,
            };
            shoppingCart.ShoppingCartItems = null;

            // 4 保存数据
            await _touristRouteRepository.AddOrderAsync(order);
            await _touristRouteRepository.SaveAsync();

            return Ok(_mapper.Map<OrderDto>(order));
        }
    }
}
