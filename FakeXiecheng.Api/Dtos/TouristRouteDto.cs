using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

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

        public IEnumerable<TouristRoutePictureDto> TouristRoutePictures { get; set; }

        /// <summary>
        /// 评级
        /// </summary>
        public double? Rating { get; set; }

        public string TravelDays { get; set; }

        public string TripType { get; set; }

        public string DepartureCity { get; set; }
    }

    // [TouristRouteTitleMustBeDifferentFromDescription]
    public class TouristRouteForCreationDto : IValidatableObject
    {
        [Required(ErrorMessage = "title不可为空")]
        [MaxLength(100)]
        public string Title { get; set; }
        [Required]
        [MaxLength(1500)]
        public string Description { get; set; }
        public decimal OriginalPrice { get; set; }
        public double? DiscountPresent { get; set; }
        public DateTime CreateTime { get; set; }
        public DateTime? UpdateTime { get; set; }
        public DateTime? DepartureTime { get; set; }
        public string Features { get; set; }
        public string Fees { get; set; }
        public string Notes { get; set; }
        public double? Rating { get; set; }
        public string TravelDays { get; set; }
        public string TripType { get; set; }
        public string DepartureCity { get; set; }

        public IEnumerable<TouristRoutePictureForCreationDto> TouristRoutePictures { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (Title == Description)
            {
                yield return new ValidationResult(
                    "路线名称必须与描述不同",
                    new[] { "TouristRouteForCreationDto" }
                );
            }
        }
    }

    public class TouristRouteTitleMustBeDifferentFromDescription : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var touristRouteDto = (TouristRouteForCreationDto)validationContext.ObjectInstance;
            if (touristRouteDto.Title == touristRouteDto.Description)
            {
                return new ValidationResult(
                    "路线名称必须与描述不同",
                    new[] { "TouristRouteForCreationDto" }
                );
            }

            return ValidationResult.Success;
        }
    }
}
