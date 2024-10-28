using System.ComponentModel.DataAnnotations;

namespace XERO.API.Models
{
    public class Product
    {
        [Required(ErrorMessage = "Name is required.")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters.")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        /// <summary>
        /// The product price as a decimal string (e.g., "99.99").
        /// </summary>
        /// <example>99.99</example>
        [Required(ErrorMessage = "Price is required.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Price must be a valid number with up to two decimal places.")]
        public string Price { get; set; }

        /// <summary>
        /// The delivery price as a decimal string (e.g., "9.99").
        /// </summary>
        /// <example>9.99</example>
        [Required(ErrorMessage = "Delivery price is required.")]
        [RegularExpression(@"^\d+(\.\d{1,2})?$", ErrorMessage = "Delivery price must be a valid number with up to two decimal places.")]
        public string DeliveryPrice { get; set; }

    }
}