using System.Text.Json.Serialization;

namespace XERO.API.Models
{
    public class Product
    {
        public Product()
        {
            Id = Guid.NewGuid();
            IsNew = true;
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public decimal DeliveryPrice { get; set; }

        [JsonIgnore]
        public bool IsNew { get; }
    }
}
