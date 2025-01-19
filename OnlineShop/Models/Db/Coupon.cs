using System;
using System.Collections.Generic;

namespace OnlineShop.Models.Db;

public partial class Coupon
{
    public int Id { get; set; }

    public string? Code { get; set; }

    public decimal? Discount { get; set; }
}
