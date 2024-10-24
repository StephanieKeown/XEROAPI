using System.Text.Json.Serialization;
using XERO.API.Models;

namespace XERO.API.NewFolder
{
    using System.Collections.Generic;
    using XERO.API.Models;

    namespace XERO.API.Models
    {
        public class Products
        {
            public List<Product> Items { get; set; }

            public Products()
            {
                Items = new List<Product>();
            }
        }
    }

}
