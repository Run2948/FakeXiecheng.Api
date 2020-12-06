using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using FakeXiecheng.Api.Models.Dtos;
using FakeXiecheng.Api.Models.Requests;
using FakeXiecheng.Api.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace FakeXiecheng.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITouristRouteRepository _touristRouteRepository;
        private readonly IMapper _mapper;

        public OrdersController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor, ITouristRouteRepository touristRouteRepository, IMapper mapper)
        {
            _httpClientFactory = httpClientFactory;
            _httpContextAccessor = httpContextAccessor;
            _touristRouteRepository = touristRouteRepository;
            _mapper = mapper;
        }

        [HttpGet]
        [HttpHead]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Get([FromQuery] PaginationRequest request)
        {
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;
            var orders = await _touristRouteRepository.GetUserOrders(userId, request.PageNumber, request.PageSize);
            return Ok(_mapper.Map<IEnumerable<OrderDto>>(orders));
        }

        [HttpGet("{orderId}")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Get([FromRoute] Guid orderId)
        {
            var order = await _touristRouteRepository.GetOrder(orderId);
            return Ok(_mapper.Map<OrderDto>(order));
        }

        [HttpPost("{orderId}/placeOrder")]
        [Authorize(AuthenticationSchemes = "Bearer")]
        public async Task<IActionResult> Post([FromRoute] Guid orderId)
        {
            // 1. 获得当前用户
            var userId = _httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value;

            // 2. 开始处理支付
            var order = await _touristRouteRepository.GetOrder(orderId);
            order.PaymentProcessing();
            await _touristRouteRepository.SaveAsync();

            // 3. 向第三方提交支付请求
            var httpClient = _httpClientFactory.CreateClient();
            var url = @"http://localhost:61324/api/FakePaymentProcess?orderNumber={0}&returnFault={1}";
            var response = await httpClient.PostAsync(string.Format(url, order.Id, false), null);

            // 4. 提取支付结果，以及支付信息
            var isApproved = false;
            var transactionMetadata = "";
            if (response.IsSuccessStatusCode)
            {
                transactionMetadata = await response.Content.ReadAsStringAsync();
                var jsonObject = (JObject)JsonConvert.DeserializeObject(transactionMetadata);
                isApproved = jsonObject["approved"].Value<bool>();
            }

            // 5. 如果第三方支付成功. 完成订单
            if (isApproved)
            {
                order.PaymentApprove();
            }
            else
            {
                order.PaymentReject();
            }
            order.TransactionMetadata = transactionMetadata;
            await _touristRouteRepository.SaveAsync();

            return Ok(_mapper.Map<OrderDto>(order));
        }
    }
}
