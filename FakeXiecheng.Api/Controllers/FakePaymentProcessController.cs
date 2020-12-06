using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentProcessController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post([FromQuery] Guid orderNumber, [FromQuery] bool returnFault = false)
        {
            // 假装在处理
            await Task.Delay(3000);

            // if returnFault is true, 返回支付失败
            if (returnFault)
            {
                return Ok(new
                {
                    id = Guid.NewGuid(),
                    created = DateTime.UtcNow,
                    approved = false,
                    message = "Reject",
                    payment_metohd = "信用卡支付",
                    order_number = orderNumber,
                    card = new
                    {
                        card_type = "信用卡",
                        last_four = "1234"
                    }
                });
            }

            return Ok(new
            {
                id = Guid.NewGuid(),
                created = DateTime.UtcNow,
                approved = true,
                message = "Reject",
                payment_metohd = "信用卡支付",
                order_number = orderNumber,
                card = new
                {
                    card_type = "信用卡",
                    last_four = "1234"
                }
            });
        }
    }
}
