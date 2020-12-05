using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FakeXiecheng.Api.Models;

namespace FakeXiecheng.Api.Dtos
{
    public class TouristRouteDto
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        // 计算方式  原价*折扣 
        public decimal Price { get; set; }

        //public decimal OriginalPrice { get; set; }

        //public double? DiscountPresent { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? UpdateTime { get; set; }

        /// <summary>
        /// 出发时间
        /// </summary>
        public DateTime? DepartureTime { get; set; }

        public string Features { get; set; }

        /// <summary>
        /// 费用
        /// </summary>
        public string Fees { get; set; }

        public string Notes { get; set; }

        public List<TouristRoutePictureDto> TouristRoutePictures { get; set; }

        /// <summary>
        /// 评级
        /// </summary>
        public double? Rating { get; set; }

        public string TravelDays { get; set; }

        public string TripType { get; set; }

        public string DepartureCity { get; set; }
    }
}
