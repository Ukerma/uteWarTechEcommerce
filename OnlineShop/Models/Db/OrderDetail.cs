using System;
using System.Collections.Generic;

namespace OnlineShop.Models.Db;

public partial class OrderDetail
{
    public int Id { get; set; }

    public string ProductTitle { get; set; } = null!;

    public decimal ProductPrice { get; set; }

    public int Count { get; set; }

    public int OrderId { get; set; }

    public int ProductId { get; set; }
}
