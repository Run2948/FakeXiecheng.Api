using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FakeXiecheng.Api.Models.Dtos
{
    public class OrderDto
    {
        public Guid Id { get; set; }
        public string UserId { get; set; }
        public ICollection<LineItemDto> OrderItems { get; set; }
        public string State { get; set; }
        /// <summary>
        /// UTC
        /// </summary>
        public DateTime CreateDate { get; set; }
        public string TransactionMetadata { get; set; }
    }
}
