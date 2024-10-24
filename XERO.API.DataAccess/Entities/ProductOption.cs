using System;
using System.Collections.Generic;

namespace XERO.API.DataAccess.Entities;

public partial class ProductOption
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public virtual Product Product { get; set; } = null!;
}
