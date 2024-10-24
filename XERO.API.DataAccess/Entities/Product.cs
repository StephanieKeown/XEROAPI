using System;
using System.Collections.Generic;

namespace XERO.API.DataAccess.Entities;

public partial class Product
{
    public Guid Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public decimal DeliveryPrice { get; set; }

    public virtual ICollection<ProductOption> ProductOptions { get; set; } = new List<ProductOption>();
}
